using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(Vector2Reference))]
public class Vector2Drawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        bool useConstant = property.FindPropertyRelative("UseConstant").boolValue;

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var rect = new Rect(position.position, Vector2.one * 20);

        if (EditorGUI.DropdownButton(rect,
            new GUIContent(GetTexture())
            , FocusType.Keyboard, new GUIStyle()
            {
                fixedWidth = 50f,
                border = new RectOffset(1, 1, 1, 1)
            }))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Constant"), useConstant, () => SetProperty(property, true));

            menu.AddItem(new GUIContent("Variable"), !useConstant, () => SetProperty(property, false));

            menu.ShowAsContext();
        }

        position.position += Vector2.right * 15;
        Vector2 value = property.FindPropertyRelative("ConstantValue").vector2Value;

        if (useConstant)
        {
            position.width = 125f;
            string newValue1 = EditorGUI.TextField(position, value.x.ToString());
            float.TryParse(newValue1, out value.x);

            position.position += Vector2.right * 125;
            string newValue2 = EditorGUI.TextField(position, value.y.ToString());
            float.TryParse(newValue2, out value.y);
            property.FindPropertyRelative("ConstantValue").vector2Value = value;
        }
        else
        {
            EditorGUI.ObjectField(position, property.FindPropertyRelative("Variable"), GUIContent.none);
        }

        EditorGUI.EndProperty();
    }

    private void SetProperty(SerializedProperty property, bool value)
    {
        var propRelative = property.FindPropertyRelative("UseConstant");
        propRelative.boolValue = value;
        property.serializedObject.ApplyModifiedProperties();
    }

    private Texture GetTexture()
    {
        return EditorGUIUtility.IconContent("animationdopesheetkeyframe").image;
    }
}
