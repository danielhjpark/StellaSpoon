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
        iterator.NextVisible(true); // ù ��° �ʵ�� �̵�

        while (iterator.NextVisible(false)) // ������ �ʵ� ��ȸ
        {
            // boilType�� �⺻ UI���� �׸��� ����
            if (iterator.name != "boilingType")
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }
        // ������ Ÿ�Կ� ���� �ٸ� Enum�� ǥ��
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