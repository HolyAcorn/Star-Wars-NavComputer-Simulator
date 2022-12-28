#if UNITY_EDITOR
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
#if DEV_MODE
using Debug = UnityEngine.Debug;
#endif

namespace Sisus.ComponentNames.EditorOnly
{
	/// <summary>
	/// Utility class for getting and setting component names.
	/// </para>
	/// </summary>
	internal static class ComponentName
	{
		private static readonly Dictionary<Type, string> defaults = new Dictionary<Type, string>();
		private static readonly Dictionary<Object, string> overrides = new Dictionary<Object, string>();
		private static readonly Dictionary<Type, string> defaultInspectorTitles = new Dictionary<Type, string>();
		private static readonly Dictionary<Object, string> inspectorTitleOverrides = new Dictionary<Object, string>();
		private static readonly Dictionary<Object, string> tooltips = new Dictionary<Object, string>();

		/// <summary>
		/// Should class name be added as suffix in the inspector by default?
		/// </summary>
		public static bool AddClassNameAsInspectorSuffixByDefault
		{
			get => EditorPrefs.GetBool("ComponentNames.AddClassNameInspectorSuffix", true);
			set => EditorPrefs.SetBool("ComponentNames.AddClassNameInspectorSuffix", value);
		}

		/// <summary>
		/// The format for inspector suffixes.
		/// </summary>
		public static string InspectorSuffixFormat
		{
			get => EditorPrefs.GetString("ComponentNames.InspectorSuffixFormat", " <color=grey>({0})</color>");
			set => EditorPrefs.SetString("ComponentNames.InspectorSuffixFormat", value);
		}

		/// <summary>
		/// Remove the default "(Script)" suffix from MonoBehaviour inspector titles?
		/// </summary>
		public static bool RemoveDefaultScriptSuffix
		{
			get => EditorPrefs.GetBool("ComponentNames.RemoveDefaultScriptSuffix", true);
			set => EditorPrefs.GetBool("ComponentNames.RemoveDefaultScriptSuffix", value);
		}

		[NotNull]
        public static string GetDefaultName([NotNull] Component component)
			=> defaults.TryGetValue(component.GetType(), out string defaultName)
			? defaultName
			: GetDefaultInspectorTitle(component, true);

        public static bool HasNameOverride([NotNull] Component component)
			=> overrides.ContainsKey(component);

		public static void SetTooltip([NotNull] Component component, string tooltip)
		{
			tooltips[component] = tooltip;
		}

		public static string GetTooltip([NotNull] Component component)
			=> tooltips.TryGetValue(component, out string tooltip) ? tooltip : "";

		public static bool TryGetNameOverride([NotNull] Component component, out string nameOverride)
			=> overrides.TryGetValue(component, out nameOverride);

		[NotNull]
		public static string Get([NotNull] Component component)
			=> TryGetNameOverride(component, out string nameOverride) ? nameOverride : GetDefaultName(component);

		public static void Set([NotNull] Component component, [CanBeNull] string name)
		{
			#if DEV_MODE
			Debug.Assert(component != null, name);
			#endif

			if(IsNullEmptyOrDefault(component, name))
			{
				ResetToDefault(component);
				return;
			}

			if(name.EndsWith(")"))
            {
				int i = name.IndexOf('(');
				if(i >= 0)
                {
					string suffix = name.Substring(i + 1, name.Length - i - 2);

					if(i == 0)
                    {
						name = GetDefaultName(component);
						Set(component, name, suffix);
						return;
					}

					name = name.Substring(0, i).Trim();
					Set(component, name, suffix);
					return;
                }
            }

			Set(component, name, AddClassNameAsInspectorSuffixByDefault);
		}

        public static void Set([NotNull] Component component, [CanBeNull] string name, bool addClassNameSuffix)
        {
			#if DEV_MODE
			Debug.Assert(component != null, name);
			#endif

			if(IsNullEmptyOrDefault(component, name))
            {
				ResetToDefault(component);
				return;
            }

            if(!addClassNameSuffix)
            {
                Set(component, name, null);
				return;
            }

            var type = component.GetType();
            if(!defaults.TryGetValue(type, out string suffix))
            {
                suffix = GetDefaultName(component);
            }

			// Avoid situations like "Cube" => "Cube (Cube (Mesh Filter))"
			if(suffix.EndsWith(")"))
            {
				int i = suffix.IndexOf('(') + 1;
				if(i > 0)
                {
					suffix = suffix.Substring(i, suffix.Length - i - 1);
				}
            }

			if(name == suffix)
			{
				Set(component, name, null);
				return;
			}

			Set(component, name, suffix);
        }

		[CanBeNull]
		public static string GetJoinedName([NotNull] Component component, [CanBeNull] string name, [CanBeNull] string suffix, [CanBeNull] out string inspectorTitle)
		{
			#if DEV_MODE
			Debug.Assert(component != null, name);
			#endif

			var type = component.GetType();

			if(!defaults.ContainsKey(type))
			{
				defaults[type] = GetDefaultName(component);
				defaultInspectorTitles[type] = GetDefaultInspectorTitle(component, false);
			}

			#if DEV_MODE
			Debug.Assert(defaultInspectorTitles.ContainsKey(type));
			#endif

			if(IsNullEmptyOrDefault(component, name, suffix))
            {
				inspectorTitle = null;
				return null;
            }

			inspectorTitle = !string.IsNullOrEmpty(suffix) ? name + string.Format(InspectorSuffixFormat, suffix) : name;

			if(NameContainer.TryGet(component, out var nameContainer) && string.Equals(nameContainer.NameOverride,  name))
			{
				return nameContainer.NameOverride;
            }

			return !string.Equals(suffix, GetDefaultName(component)) ? name + " (" + suffix + ")" : name;
		}

		public static void Set([NotNull] Component component, [CanBeNull] string name, [CanBeNull] string suffix)
		{
			#if DEV_MODE
			Debug.Assert(component != null, name);
			#endif

			var type = component.GetType();

			if(!defaults.ContainsKey(type))
			{
				defaults[type] = GetDefaultName(component);
				defaultInspectorTitles[type] = GetDefaultInspectorTitle(component, false);
			}

			#if DEV_MODE
			Debug.Assert(defaultInspectorTitles.ContainsKey(type));
			#endif

			if(IsNullEmptyOrDefault(component, name, suffix))
            {
				ResetToDefault(component);
				return;
            }

			string setNameOverride = GetJoinedName(component, name, suffix, out string inspectorTitle);
			if(string.IsNullOrEmpty(setNameOverride))
			{
				ResetToDefault(component);
				return;
			}

			if(!NameContainer.TryGet(component, out var nameContainer))
			{
				// Delay adding of NameContainer to avoid exceptions when called from OnValidate.
				EditorApplication.delayCall += ()=>
				{
					if(component == null)
					{
						return;
					}

					if(NameContainer.TryGet(component, out _))
					{
						Set(component, name, suffix);
						return;
					}

					overrides[component] = setNameOverride;
					inspectorTitleOverrides[component] = inspectorTitle;
					NameContainer.Create(component, setNameOverride);

				};
				return;
			}

			overrides[component] = setNameOverride;
			inspectorTitleOverrides[component] = inspectorTitle;

			if(nameContainer.NameOverride == setNameOverride)
            {
				return;
            }

			nameContainer.NameOverride = setNameOverride;

			if(PrefabUtility.IsPartOfPrefabAsset(nameContainer) && !PrefabUtility.IsPartOfPrefabInstance(nameContainer))
			{
				// Delay to avoid exceptions when called from OnValidate.
				EditorApplication.delayCall += ()=> 
				{
					if(nameContainer != null)
					{
						PrefabUtility.SavePrefabAsset(nameContainer.transform.root.gameObject);
					}
				};
			}
		}

        public static string GetInspectorTitle([NotNull] Component component)
		{
			if(inspectorTitleOverrides.TryGetValue(component, out string inspectorTitle))
            {
				return inspectorTitle;
            }

			return GetDefaultInspectorTitle(component, RemoveDefaultScriptSuffix);
		}

		public static string GetInspectorTitleAsPlainText([NotNull] Component component)
		{
			return WithoutRichTextTags(GetInspectorTitle(component));
		}

		public static string GetDefaultInspectorTitle([NotNull] Component component, bool removeScriptSuffix)
		{
			if(defaultInspectorTitles.TryGetValue(component.GetType(), out string inspectorTitle))
			{
				return inspectorTitle;
			}

			if(TryGetCustomNameFromAddComponentMenuAttribute(component, out inspectorTitle))
			{
				return inspectorTitle;
			}
			
			inspectorTitle = ObjectNames.GetInspectorTitle(component);

			if(removeScriptSuffix && inspectorTitle.EndsWith(" (Script)"))
			{
				inspectorTitle = inspectorTitle.Substring(0, inspectorTitle.Length - " (Script)".Length);
			}

			return inspectorTitle;
		}

		private static bool TryGetCustomNameFromAddComponentMenuAttribute(Component component, out string inspectorTitle)
		{
			foreach(AddComponentMenu addComponentMenu in component.GetType().GetCustomAttributes(typeof(AddComponentMenu), false))
			{
				string menuName = addComponentMenu.componentMenu;
				if(!string.IsNullOrEmpty(menuName))
				{
					int lastCategoryEnd = menuName.LastIndexOf('/');
					inspectorTitle = lastCategoryEnd != -1 ? menuName.Substring(lastCategoryEnd + 1) : menuName;
					return true;
				}
			}

			inspectorTitle = "";
			return false;
		}

		public static bool ResetToDefault([NotNull] Component component)
		{
			bool test1 = NameContainer.TryGet(component, out NameContainer nc);
			bool test2 = !string.IsNullOrEmpty(nc?.TooltipOverride);

			if(NameContainer.TryGet(component, out NameContainer nameContainer) && (!string.IsNullOrEmpty(nameContainer.TooltipOverride) || !nameContainer.TryDestroyDelayed()))
			{
				nameContainer.NameOverride = "";

				if(PrefabUtility.IsPartOfPrefabAsset(nameContainer) && !PrefabUtility.IsPartOfPrefabInstance(nameContainer))
				{
					// Delay to avoid exceptions when called from OnValidate.
					EditorApplication.delayCall += () =>
					{
						if(nameContainer != null)
						{
							PrefabUtility.SavePrefabAsset(nameContainer.transform.root.gameObject);
						}
					};
				}
			}

			inspectorTitleOverrides.Remove(component);
			return overrides.Remove(component);
		}

		private static bool IsNullEmptyOrDefault(Component component, string name)
        {
			string defaultName = GetDefaultName(component);
			return string.IsNullOrEmpty(name) || string.Equals(name, defaultName);
        }

		private static bool IsNullEmptyOrDefault(Component component, string name, string suffix)
        {
			string defaultName = GetDefaultName(component);
			return (string.IsNullOrEmpty(name) || string.Equals(name, defaultName)) && (string.IsNullOrEmpty(suffix) || string.Equals(suffix, defaultName));
        }

		private static string WithoutRichTextTags(string text)
		{
			const int maxIterations = 10;
			for(int iteration = 1; iteration < maxIterations; iteration++)
            {
                int index = text.IndexOf("<color=", StringComparison.OrdinalIgnoreCase);
                if(index == -1)
                {
                    return text;
                }

                int endIndex = text.IndexOf('>', index + "<color=".Length);
                if(endIndex > 0)
                {
                    text = text.Remove(index, endIndex + 1 - index);
                }

				index = text.IndexOf("</color>", StringComparison.OrdinalIgnoreCase);
				if(index == -1)
				{
					return text;
				}

				text = text.Remove(index, "</color>".Length);
			}

			return text;
        }
	}
}
#endif