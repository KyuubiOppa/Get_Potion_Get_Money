using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(SO_Order))]
public class SO_Order_Editor : Editor
{
    private SO_Order soOrder;

    private void OnEnable()
    {
        soOrder = (SO_Order)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Order Image", EditorStyles.boldLabel);

        if (soOrder.orderSprite != null)
        {
            GUILayout.Label(soOrder.orderSprite.texture, GUILayout.Width(100), GUILayout.Height(100));
        }
        else
        {
            GUILayout.Label("No Image", GUILayout.Width(100), GUILayout.Height(100));
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Recipe Images", EditorStyles.boldLabel);

        if (soOrder.recipes != null && soOrder.recipes.Length > 0)
        {
            for (int i = soOrder.recipes.Length - 1; i >= 0; i--)
            {
                SO_Recipe recipe = soOrder.recipes[i];
                if (recipe != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(recipe.recipeName, GUILayout.Width(150));

                    // ตรวจสอบว่ามี imageRecipe หรือไม่
                    if (recipe.recipeSprite != null)
                    {
                        GUILayout.Label(recipe.recipeSprite.texture, GUILayout.Width(100), GUILayout.Height(100));
                    }
                    else
                    {
                        GUILayout.Label("No Image", GUILayout.Width(50), GUILayout.Height(50));
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("No Recipes Assigned.");
        }
    }
}
#endif