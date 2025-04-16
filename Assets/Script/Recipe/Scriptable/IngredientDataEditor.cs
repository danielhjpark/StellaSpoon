using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ingredient))]
public class IngredientDataEditor : Editor
{
    SerializedProperty isPublicIngredientProp;
    SerializedProperty ingredientCookTypeProp;
    SerializedProperty ingredientCookTypesProp;

    private void OnEnable()
    {
        isPublicIngredientProp = serializedObject.FindProperty("isPublicIngredient");
        ingredientCookTypeProp = serializedObject.FindProperty("ingredientCookType");
        ingredientCookTypesProp = serializedObject.FindProperty("ingredientCookTypes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true); 

        while (iterator.NextVisible(false)) 
        {
            if (iterator.name != "ingredientCookType" && iterator.name != "ingredientCookTypes")
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }


        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

}
}
