using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Sisus.ComponentNames.EditorOnly
{
    [CustomPropertyDrawer(typeof(UnityEventBase), true)]
    public class RenameableUnityEventDrawer : PropertyDrawer
    {
        private UnityEventDrawer wrappedDrawer;
        
        private UnityEventDrawer WrappedDrawer
        {
            get
            {
                if(wrappedDrawer == null)
                {
                    CreateWrapperDrawer();
                }
                
                return wrappedDrawer;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
			{
                OnLeftMouseDown(position, property);
            }

            WrappedDrawer.OnGUI(position, property, label);

            #if DEV_MODE
            if(Event.current.control)
			{
                var c = Color.red;
                c.a = 0.5f;

                var functionDropdownRect = GetFirstFunctionDropdownRect(position);
                float elementHeight = GetElementHeight();
                float yMax = position.yMax - elementHeight;

                for(float y = functionDropdownRect.y; y < yMax; y += elementHeight)
                {
                    functionDropdownRect.y = y;
                    EditorGUI.DrawRect(functionDropdownRect, c);
                }
			}
            #endif
        }

        private Rect GetFirstFunctionDropdownRect(Rect propertyRect)
		{
            Rect functionDropdownRect = propertyRect;
            functionDropdownRect.y += 27f;
            float indent = EditorGUI.IndentedRect(propertyRect).x;
            float firstDropdownWidth = (propertyRect.width - indent) * 0.3f;
            functionDropdownRect.xMin = indent + firstDropdownWidth + 23f;
            functionDropdownRect.width -= 7f;
            functionDropdownRect.height = EditorGUIUtility.singleLineHeight;
            return functionDropdownRect;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
			return WrappedDrawer.GetPropertyHeight(property, label);
        }

		public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return WrappedDrawer.CanCacheInspectorGUI(property);
        }

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return WrappedDrawer.CreatePropertyGUI(property);
        }

		private void CreateWrapperDrawer()
        {
            wrappedDrawer = new UnityEventDrawer();

			var attributeField = typeof(PropertyDrawer).GetField("m_Attribute", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			if(attributeField != null)
			{
				attributeField.SetValue(wrappedDrawer, attribute);
			}

            var fieldInfoField = typeof(PropertyDrawer).GetField("m_FieldInfo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if(fieldInfoField != null)
            {
                fieldInfoField.SetValue(wrappedDrawer, fieldInfo);
            }
        }

        private void OnLeftMouseDown(Rect propertyRect, SerializedProperty property)
        {
            float elementHeight = GetElementHeight();
            Rect functionDropdownRect = GetFirstFunctionDropdownRect(propertyRect);
            float yMax = propertyRect.yMax - elementHeight;

            int index = 0;
            for(float y = functionDropdownRect.y; y < yMax; y += elementHeight)
            {
                functionDropdownRect.y = y;
                if(functionDropdownRect.Contains(Event.current.mousePosition))
                {
                    var genericMenu = UnityEventDrawerUtility.BuildFunctionSelectDropdownMenu(property, index);
                    if(genericMenu != null)
                    {
                        Event.current.Use();
                        genericMenu.DropDown(functionDropdownRect);
                        GUIUtility.ExitGUI();
                        return;
                    }
                }

                index++;
            }
        }

        private float GetElementHeight()
        {
            const float extraSpacing = 9f;
            return EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing + extraSpacing;
        }
	}
}