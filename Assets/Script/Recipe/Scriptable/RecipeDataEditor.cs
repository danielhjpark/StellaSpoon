using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Recipe))]
public class RecipeDataEditor : Editor
{
    SerializedProperty cookTypeProp;
    SerializedProperty boilTypeProp;

    private void OnEnable()
    {
        cookTypeProp = serializedObject.FindProperty("recipeCookType");
        boilTypeProp = serializedObject.FindProperty("boilingType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true); // 첫 번째 필드로 이동

        while (iterator.NextVisible(false)) // 나머지 필드 순회
        {
            // boilType은 기본 UI에서 그리지 않음
            if (iterator.name != "boilingType")
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }
        // 선택한 타입에 따라 다른 Enum을 표시
        if ((CookType)cookTypeProp.enumValueIndex == CookType.Boiling)
        {
            EditorGUILayout.PropertyField(boilTypeProp);
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

    }
}