using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(StringReference))]
public class StringDrawer : PropertyDrawer
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
        string value = property.FindPropertyRelative("ConstantValue").stringValue;

        position.width = 250f;
        if (useConstant)
        {

            string newValue = EditorGUI.TextField(position, value.ToString());
            property.FindPropertyRelative("ConstantValue").stringValue = value;
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
