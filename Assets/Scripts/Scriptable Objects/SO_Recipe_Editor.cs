using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_EDITOR

[CustomEditor(typeof(SO_Recipe))]
public class SO_RecipeEditor : Editor
{
    private SerializedProperty recipeTypeProp;
    private SerializedProperty recipeIDProp;
    private SerializedProperty recipeNameProp;
    private SerializedProperty recipeSpriteProp;
    private SerializedProperty recipePrefabProp;

    private SerializedProperty recipColorPotionProp;

    private void OnEnable()
    {
        recipeTypeProp = serializedObject.FindProperty("recipeType");
        recipeIDProp = serializedObject.FindProperty("recipeID");
        recipeNameProp = serializedObject.FindProperty("recipeName");
        recipeSpriteProp = serializedObject.FindProperty("recipeSprite");
        recipePrefabProp = serializedObject.FindProperty("recipePrefab");
        recipColorPotionProp = serializedObject.FindProperty("colorPotion");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(recipeTypeProp);
        EditorGUILayout.PropertyField(recipeIDProp);
        EditorGUILayout.PropertyField(recipeNameProp);
        EditorGUILayout.PropertyField(recipeSpriteProp);
        EditorGUILayout.PropertyField(recipePrefabProp);
        EditorGUILayout.PropertyField(recipColorPotionProp);

        if (recipeSpriteProp.objectReferenceValue != null)
        {
            Sprite sprite = (Sprite)recipeSpriteProp.objectReferenceValue;
            GUILayout.Label(sprite.texture, GUILayout.Width(350), GUILayout.Height(350));
        }
        else
        {
            GUILayout.Label("No Image", GUILayout.Width(100), GUILayout.Height(100));
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif