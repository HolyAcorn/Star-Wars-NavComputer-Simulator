using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sisus.ComponentNames.EditorOnly
{
	#if UNITY_EDITOR
	public delegate void ModifyGameObjectEventHandler(GameObject gameObjectOrPrefabContentsRoot);
	public delegate void ParentSetEventHandler(Transform child, Transform parent, bool isPrefabOrPrefabInstance);

	public enum ModificationType
	{
		AddContent,
		RemoveGameObject,
		RemoveComponent
	}

	public static class GameObjectUtility
	{
		public static bool TryDestroy([NotNull] GameObject gameObject) => CanDestroy(gameObject) && TryModify(gameObject, ModificationType.RemoveGameObject, (destroy) => Destroy(destroy));

		public static bool CanDestroy([NotNull] GameObject gameObject) => !PrefabUtility.IsPartOfPrefabInstance(gameObject) || PrefabUtility.IsAddedGameObjectOverride(gameObject) || PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject);

		public static bool CanSetParentOf([NotNull] GameObject gameObject) => !PrefabUtility.IsPartOfPrefabInstance(gameObject) || PrefabUtility.IsAddedGameObjectOverride(gameObject) || PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject);

		public static bool TrySetParent([NotNull] Transform child, [CanBeNull] Transform parent, bool worldPositionStays) => CanSetParentOf(child.gameObject) && TryModify(parent != null ? parent.gameObject : child.gameObject, ModificationType.AddContent, (setParent) => child.SetParent(setParent != null ? setParent.transform : null, worldPositionStays));

		private static void Destroy(Object target)
		{
			if(!Application.isPlaying)
			{
				Object.DestroyImmediate(target);
				return;
			}

			Object.Destroy(target);
		}

		private static readonly Stack<int> childIndexes = new Stack<int>();

		private static void GetChildIndexStackIgnoringAddedGameObjectOverrides(Transform root, Transform leaf, Stack<int> childIndexes)
		{
			for(var transform = leaf; transform != root; transform = transform.parent)
			{
				childIndexes.Push(GetSiblingIndexIgnoringAddedGameObjectOverrides(transform));
			}
		}

		private static int GetSiblingIndexIgnoringAddedGameObjectOverrides(Transform transform)
		{
			var parent = transform.parent;
			if(parent == null)
			{
				return -1;
			}

			int index = 0;
			for(int i = 0, childCount = parent.hierarchyCapacity; i < childCount; i++)
			{
				var child = parent.GetChild(i);

				if(transform == child)
				{
					return index;
				}

				if(PrefabUtility.IsAddedGameObjectOverride(child.gameObject))
				{
					continue;
				}

				index++;
			}

			return -1;
		}

		public static bool TryModify([CanBeNull] GameObject gameObjectToModify, ModificationType type, ModifyGameObjectEventHandler applyModifications)
		{
			if(gameObjectToModify == null)
			{
				return false;
			}

			if(PrefabUtility.IsPartOfPrefabAsset(gameObjectToModify))
			{
				var root = gameObjectToModify.transform.root;

				string prefabPath = AssetDatabase.GetAssetPath(root);
				var prefabContents = PrefabUtility.LoadPrefabContents(prefabPath);

				#if DEV_MODE
				Debug.Assert(string.Equals(prefabContents.name, root.name), prefabContents.name + " != " + root.name);
				#endif

				GetChildIndexStackIgnoringAddedGameObjectOverrides(root, gameObjectToModify.transform, childIndexes);
				Transform equivalentTransformInPrefab = prefabContents.transform;
				while(childIndexes.Count > 0)
				{
					int childIndex = childIndexes.Pop();
					equivalentTransformInPrefab = equivalentTransformInPrefab.GetChild(childIndex);
				}
				childIndexes.Clear();
				applyModifications(equivalentTransformInPrefab.gameObject);

				PrefabUtility.SaveAsPrefabAsset(prefabContents, prefabPath, out bool success);
				#if DEV_MODE
				if(!success)
				{
					Debug.LogError("Failed to save changes to prefab @ "+prefabPath);
				}
				#endif
				PrefabUtility.UnloadPrefabContents(prefabContents);
				return success;
			}

			if(type == ModificationType.RemoveGameObject && PrefabUtility.IsPartOfPrefabInstance(gameObjectToModify) && !PrefabUtility.IsAddedGameObjectOverride(gameObjectToModify))
			{
				return false;
			}

			applyModifications(gameObjectToModify);
			return true;
		}
	}
	#endif
}