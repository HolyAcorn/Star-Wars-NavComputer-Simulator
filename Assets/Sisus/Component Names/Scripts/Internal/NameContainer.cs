//#define SHOW_NAME_CONTAINERS

using System;
using System.Collections.Concurrent;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("RenameComponents.Editor")]
#endif

namespace Sisus.ComponentNames.EditorOnly
{
	/// <summary>
	/// Component that acts as a container for the custom name given to a component.
	/// </summary>
	[AddComponentMenu(DontShowInMenu)]
	internal class NameContainer : MonoBehaviour
	{
		#pragma warning disable CS0414

		private const string DontShowInMenu = "";

		internal static bool NowRenaming;
		internal static Component StartingToRename;

		private static readonly ConcurrentDictionary<Component, NameContainer> instances = new ConcurrentDictionary<Component, NameContainer>();

		[SerializeField]
		private string nameOverride = "";

		[SerializeField]
		private string tooltipOverride = "";

		[SerializeField]
		private Component target = null;

		#pragma warning restore CS0414

		#if UNITY_EDITOR
		public string NameOverride
		{
			get => nameOverride;

			set
			{
				if(value == nameOverride)
				{
					return;
				}

				nameOverride = value;
				EditorUtility.SetDirty(this);
			}
		}

		public string TooltipOverride
		{
			get => tooltipOverride;

			set
			{
				if(value == tooltipOverride)
				{
					return;
				}

				tooltipOverride = value;
				EditorUtility.SetDirty(this);
			}
		}

		private void Awake() => OnValidate();

		private void OnValidate()
        {
			#if DEV_MODE && SHOW_NAME_CONTAINERS
			hideFlags = HideFlags.None;
			gameObject.hideFlags = HideFlags.None;
			#else
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			#endif

			if(NowRenaming)
			{
				return;
			}

			if(target == null)
            {
				if(!CanBeDestroyed())
				{
					return;
				}

				EditorApplication.delayCall += () =>
				{
					if(this != null && target == null)
					{
						TryDestroyImmediate();
					}
				};
				return;
			}

            if(transform.parent != target.transform)
            {
                if(transform == target.transform)
                {
					target = null;
					TryDestroyDelayed();
                    return;
                }

				EditorApplication.delayCall += () =>
				{
					if(this != null)
					{
						GameObjectUtility.TrySetParent(transform, transform.transform, false);
					}
				};
            }

			if(instances.TryGetValue(target, out NameContainer existingContainer) && existingContainer != this && existingContainer != null && existingContainer.target == target)
			{
				if(CanBeDestroyed())
				{
					// Copy over name and tooltip from this container to the other one and destroy this one.
					// It is likely that this container contains the name/tooltip for a prefab instance and the other
					// one contains it for the prefab asset.
					// In this situation we want to convert the name and tooltips into instance value overrides
					// instead of having two different name containers for one target.
					// In any case we never want to have two different name containers when it can be avoided.
					existingContainer.NameOverride = NameOverride;
					existingContainer.TooltipOverride = TooltipOverride;
					TryDestroyDelayed();
					return;
				}

				if(existingContainer.CanBeDestroyed())
				{
					// Copy over name and tooltip from the other container to the this one and destroy the other one.
					// It is likely that the other container contains the name/tooltip for a prefab instance and this
					// one contains it for the prefab asset.
					// In this situation we want to convert the name and tooltips into instance value overrides
					// instead of having two different name containers for one target.
					// In any case we never want to have two different name containers when it can be avoided.
					NameOverride = existingContainer.NameOverride;
					TooltipOverride = existingContainer.TooltipOverride;
					existingContainer.TryDestroyDelayed();
					return;
				}

				#if DEV_MODE
				Debug.LogWarning($"Detected multiple name containers for target {target}. Could be because of renaming same GameObject in prefab instance first and in then prefab asset. Unable to fix the issue.", target);
				#endif
			}
			instances[target] = this;

			ComponentTooltip.Set(target, tooltipOverride);

			if(IsEmptyOrDefaultName(nameOverride))
            {
				ComponentName.ResetToDefault(target);
            }
			else
			{
				ComponentName.Set(target, nameOverride);
			}
		}

		public bool CanBeDestroyed() => !PrefabUtility.IsPartOfPrefabInstance(gameObject) || PrefabUtility.IsAddedGameObjectOverride(gameObject);

		public bool TryDestroyDelayed()
		{
			if(!CanBeDestroyed())
			{
				return false;
			}

			#if DEV_MODE
			Debug.Log($"Destroying NameContainer(\"{nameOverride}\")");
			#endif

			EditorApplication.delayCall += TryDestroyImmediate;
			return true;
		}

		public void TryDestroyImmediate()
		{
			if(this == null || !CanBeDestroyed())
			{
				return;
			}

			if(gameObject.GetComponents<Component>().Length > 2)
			{
				#if DEV_MODE
				Debug.LogWarning(GetComponents<Component>()[2]);
				#endif
				DestroyImmediate(this);
				return;
			}

			GameObjectUtility.TryDestroy(gameObject);
		}

		private bool IsEmptyOrDefaultName(string name) => name.Length == 0 || string.Equals(name, ComponentName.GetDefaultName(target));

        public static void StartRenaming(Component component)
        {
			NowRenaming = true;
			StartingToRename = component;

			Debug.Assert(component != null);
			Debug.Assert(!(component is NameContainer));

			StartingToRename = component;
			EditorGUIUtility.editingTextField = true;
		}

		public static NameContainer Create(Component component, string initialName = null, string initialTooltip = "")
        {
			var nameContainer = new GameObject("NameContainer(EditorOnly)").AddComponent<NameContainer>();

			#if DEV_MODE && SHOW_NAME_CONTAINERS
			nameContainer.gameObject.hideFlags = HideFlags.None;
			nameContainer.hideFlags = HideFlags.None;
			#else
			nameContainer.gameObject.hideFlags = HideFlags.HideInHierarchy;
			#endif

			nameContainer.gameObject.tag = "EditorOnly";
			nameContainer.target = component;

			var componentType = component.GetType();
			int componentIndex = Array.IndexOf(component.gameObject.GetComponents(componentType), component);

			if(!GameObjectUtility.TryModify(component.gameObject, ModificationType.AddContent, (setParent) =>
			{
				try
				{
					nameContainer.transform.SetParent(setParent.transform, false);
					var target = setParent.GetComponents(componentType)[componentIndex];
					nameContainer.target = target;

					if(initialName != null)
					{
						nameContainer.NameOverride = initialName;
					}

					if(initialTooltip != null)
					{
						nameContainer.TooltipOverride = initialTooltip;
					}
				}
				catch(Exception e)
				{
					Debug.LogWarning(e);
					DestroyImmediate(nameContainer.gameObject);
				}
			}))
			{
				#if DEV_MODE
				Debug.LogWarning("SaveAsPrefabAsset failed...");
				#endif
				DestroyImmediate(nameContainer.gameObject);
				return null;
			}

			return nameContainer;
        }

		public static bool TryGet(Component component, out NameContainer nameContainer)
        {
			var transform = component.transform;
			for(int i = transform.childCount -  1; i >= 0; i--)
            {
				if(transform.GetChild(i).TryGetComponent(out NameContainer someNameContainer) && someNameContainer.target == component)
                {
					nameContainer = someNameContainer;
					return true;
                }
            }

			nameContainer = null;
			return false;
        }
		#endif
	}
}