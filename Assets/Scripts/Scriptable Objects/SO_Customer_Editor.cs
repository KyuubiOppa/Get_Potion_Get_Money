#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_Customer))]
public class SO_Customer_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        SO_Customer customer = (SO_Customer)target;

        // แสดงค่าเริ่มต้นของ Inspector ยกเว้น Sprite
        DrawDefaultInspectorExceptSprites();

        GUILayout.Space(10);
        GUILayout.Label("____ Customer Sprites ____", EditorStyles.boldLabel);

        EditorGUIUtility.labelWidth = 100; // กำหนดความกว้างของ Label

        DrawSpriteField("ตอนมีความสุข", customer.customerSmile);
        DrawSpriteField("ตอนอารมณ์ตึงๆ", customer.customerTight);
        DrawSpriteField("ตอนโกรธ", customer.customerAngry);

        EditorGUIUtility.labelWidth = 0; // รีเซ็ตค่า

        if (GUI.changed)
        {
            EditorUtility.SetDirty(customer);
        }
    }

    private void DrawDefaultInspectorExceptSprites()
    {
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.GetIterator();
        property.NextVisible(true);

        while (property.NextVisible(false))
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference && property.type == "PPtr<Sprite>")
                continue; // ข้าม property ที่เป็น Sprite

            EditorGUILayout.PropertyField(property, true);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSpriteField(string label, Sprite sprite)
    {
        if (sprite != null)
        {
            GUILayout.Label(label, EditorStyles.boldLabel);

            Rect rect = GUILayoutUtility.GetRect(275, 275, GUILayout.ExpandWidth(false));
            GUI.DrawTexture(rect, sprite.texture, ScaleMode.ScaleToFit);
        }
    }
}
#endif
