using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Recipe))]
public class RecipeDataEditor : Editor
{
    SerializedProperty cookTypeProp;
    SerializedProperty BoilingSettingProp;
    SerializedProperty TossingSettingProp;
    SerializedProperty CuttingSettingProp;
    SerializedProperty FryingSettingProp;

    private void OnEnable()
    {
        cookTypeProp = serializedObject.FindProperty("cookType");
        BoilingSettingProp = serializedObject.FindProperty("boilingSetting");
        TossingSettingProp = serializedObject.FindProperty("tossingSetting");
        CuttingSettingProp = serializedObject.FindProperty("cuttingSetting");
        FryingSettingProp = serializedObject.FindProperty("fryingSetting");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true); 

        while (iterator.NextVisible(false)) 
        {
            if (iterator.name != "boilingSetting" && iterator.name != "tossingSetting"
            && iterator.name != "cuttingSetting" && iterator.name != "fryingSetting")
            {
                EditorGUILayout.PropertyField(iterator, true);
            }

        }

        // 선택한 타입에 따라 다른 Enum을 표시
        switch((CookType)cookTypeProp.enumValueIndex) {
            case CookType.Boiling:
                EditorGUILayout.PropertyField(BoilingSettingProp);
                break;
            case CookType.Cutting:
                EditorGUILayout.PropertyField(CuttingSettingProp);
                break;
            case CookType.Tossing:
                EditorGUILayout.PropertyField(TossingSettingProp);
                break;
            case CookType.Frying:
                EditorGUILayout.PropertyField(FryingSettingProp);
                break;
            default:
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

    }
}