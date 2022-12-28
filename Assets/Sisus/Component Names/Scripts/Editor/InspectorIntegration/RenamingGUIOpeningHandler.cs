using UnityEditor;
using UnityEngine;
using Sisus.Shared.EditorOnly;

namespace Sisus.ComponentNames.EditorOnly
{
    [InitializeOnLoad]
	internal static class RenamingGUIOpeningHandler
	{
        private static readonly GUIContent tooltip = new GUIContent("", "");
        private static readonly GUIContent label = new GUIContent("");

        static RenamingGUIOpeningHandler()
		{
            ComponentHeader.BeforeHeaderGUI -= BeginComponentHeader;
            ComponentHeader.BeforeHeaderGUI += BeginComponentHeader;
		}

        internal static float BeginComponentHeader(Component component, Rect headerRect, bool headerIsSelected, bool headerSupportsRichText)
		{
            HandleOpeningRenamingGUI(component, headerRect, headerIsSelected);

            if(headerSupportsRichText)
			{
                UpdateInternalCachedTitleForComponentType(component);
            }
            else
            {
                UpdateInternalCachedTitleForComponentTypeAsPlainText(component);
            }

            var tooltipRect = headerRect;
            tooltipRect.x += 60f;
            #if POWER_INSPECTOR
            tooltipRect.width -= 185f;
            #else
            tooltipRect.width -= 125f;
            #endif

            label.text = ComponentName.GetInspectorTitleAsPlainText(component);
            float titleWidth = EditorStyles.boldLabel.CalcSize(label).x;
            if(titleWidth < tooltipRect.width)
			{
                tooltipRect.width = titleWidth;
			}


            tooltip.tooltip = ComponentTooltip.Get(component);
            GUI.Label(tooltipRect, tooltip);

            return 0f;
        }

        internal static void HandleOpeningRenamingGUI(Component component, Rect headerRect, bool headerIsSelected)
        {
            switch(Event.current.rawType)
            {
                case EventType.ValidateCommand:
                    if(Event.current.commandName == "Rename" && headerIsSelected)
                    {
                        Event.current.Use();
                        BeginRenamingComponent(component, headerRect);
                    }
                    break;
                case EventType.KeyDown:
                    if(!headerIsSelected)
                    {
                        break;
                    }

                    switch(Event.current.keyCode)
                    {
                        case KeyCode.F2:
                        case KeyCode.KeypadEnter:
                        case KeyCode.Return:
                            Event.current.Use();
                            BeginRenamingComponent(component, headerRect);
                            break;
                    }
                    break;
            }

            if(NameContainer.StartingToRename == component)
			{
                NameContainer.StartingToRename = null;
                BeginRenamingComponent(component, headerRect);
            }
        }

        internal static void BeginRenamingComponent(Component component, Rect headerRect)
        {
            var openAt = InScreenSpace(GetLabelRect(headerRect));
            RenameComponentWindow.Open(openAt, component);
        }

        internal static void UpdateInternalCachedTitleForComponentType(Component component)
		{
			string title = ComponentName.GetInspectorTitle(component);
			ObjectNamesUtility.InternalInspectorTitlesCache[component.GetType()] = title;
		}

        internal static void UpdateInternalCachedTitleForComponentTypeAsPlainText(Component component)
		{
			string title = ComponentName.GetInspectorTitleAsPlainText(component);
			ObjectNamesUtility.InternalInspectorTitlesCache[component.GetType()] = title;
		}

        private static Rect InScreenSpace(Rect rect)
        {
            rect.position = GUIUtility.GUIToScreenPoint(rect.position);
            return rect;
        }

        private static Rect GetLabelRect(Rect headerRect)
        {
            var headerLabelRect = headerRect;

            headerLabelRect.x = 54f;

            // Fixes Transform header label rect position.
            // For some reason the Transform header rect starts
            // lower and is shorter than all other headers.
            if(headerLabelRect.height < 22f)
            {
                headerLabelRect.y -= 22f - 15f;
            }

            headerLabelRect.width = Screen.width - 123f;
            headerLabelRect.height = 20f;

            return headerLabelRect;
        }
    }
}