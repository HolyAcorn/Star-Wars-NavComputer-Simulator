#if UNITY_EDITOR
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sisus.ComponentNames.EditorOnly
{
	/// <summary>
	/// Utility class for getting and setting tooltips.
	/// </para>
	/// </summary>
	internal static class ComponentTooltip
	{
		private static readonly Dictionary<Object, string> tooltips = new Dictionary<Object, string>();
		private static readonly HashSet<Component> hasOverride = new HashSet<Component>();

		public static bool HasOverride([NotNull] Component component)
			=> hasOverride.Contains(component);

		public static void Set([NotNull] Component component, [CanBeNull] string tooltip)
		{
			if(string.IsNullOrEmpty(tooltip))
			{
				tooltips.Remove(component);
				hasOverride.Remove(component);

				if(NameContainer.TryGet(component, out var clearTooltipOverrideFromContainer))
				{
					clearTooltipOverrideFromContainer.TooltipOverride = "";
				}
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
						Set(component, tooltip);
						return;
					}

					hasOverride.Add(component);
					tooltips[component] = tooltip;
					NameContainer.Create(component, null, tooltip);
				};
				return;
			}

			hasOverride.Add(component);
			tooltips[component] = tooltip;

			if(nameContainer.TooltipOverride == tooltip)
            {
				return;
            }

			nameContainer.TooltipOverride = tooltip;

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

		public static string Get([NotNull] Component component)
		{
			if(tooltips.TryGetValue(component, out string tooltip))
			{
				return tooltip;
			}

			tooltip = GetSummary(component);
			tooltips.Add(component, tooltip);
			return tooltip;
		}

		private static string GetSummary(Component component)
		{
			if(component is MonoBehaviour monoBehaviour && MonoScriptSummaryParser.TryParseSummary(monoBehaviour, out string tooltipFromMonoScript))
			{
				return tooltipFromMonoScript;
			}
			else if(DLLSummaryParser.TryParseSummary(component.GetType(), out string tooltipFromDll))
			{
				return tooltipFromDll;
			}

			return "";
		}
	}
}
#endif