using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Sisus.Shared.EditorOnly.InspectorContents;

namespace Sisus.Shared.EditorOnly
{
	[InitializeOnLoad]
	internal static class ComponentHeaderWrapperToInspectorInjector
	{
		private static readonly List<Component> components = new List<Component>();

		static ComponentHeaderWrapperToInspectorInjector()
		{
			Editor.finishedDefaultHeaderGUI -= AfterInspectorRootEditorHeaderGUI;
			Editor.finishedDefaultHeaderGUI += AfterInspectorRootEditorHeaderGUI;
		}

		private static void AfterInspectorRootEditorHeaderGUI(Editor editor)
		{
			if(editor.target is GameObject)
			{
				// Handle InspectorWindow
				AfterGameObjectHeaderGUI(editor);
			}
			else if(editor.target is Component)
			{
				// Handle PropertyEditor window opened via "Properties..." context menu item
				AfterPropertiesHeaderGUI(editor);
			}
		}

		private static void AfterGameObjectHeaderGUI([NotNull] Editor gameObjectEditor)
		{
			foreach((Editor editor, IMGUIContainer header) editorAndHeader in GetComponentHeaderElementsFromInspector(gameObjectEditor))
			{
				var onGUIHandler = editorAndHeader.header.onGUIHandler;
				if(onGUIHandler.Method is MethodInfo onGUI && onGUI.Name == nameof(ComponentHeaderWrapper.DrawWrappedHeaderGUI))
				{
					continue;
				}

				var component = editorAndHeader.editor.target as Component;
				var renameableComponentEditor = new ComponentHeaderWrapper(editorAndHeader.header, component, true);
				editorAndHeader.header.onGUIHandler = renameableComponentEditor.DrawWrappedHeaderGUI;
			}
		}

		private static void AfterPropertiesHeaderGUI([NotNull] Editor componentEditor)
		{
			var found = GetComponentHeaderElementFromPropertyEditorOf(componentEditor);
			if(!found.HasValue)
			{
				return;
			}
			
			(Editor editor, IMGUIContainer header) editorAndHeader = found.Value;
			var onGUIHandler = editorAndHeader.header.onGUIHandler;
			if(onGUIHandler.Method is MethodInfo onGUI && onGUI.Name == nameof(ComponentHeaderWrapper.DrawWrappedHeaderGUI))
			{
				return;
			}

			var component = editorAndHeader.editor.target as Component;
			var renameableComponentEditor = new ComponentHeaderWrapper(editorAndHeader.header, component, false);

			editorAndHeader.header.onGUIHandler = renameableComponentEditor.DrawWrappedHeaderGUI;
		}
	}
}