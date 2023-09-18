using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Results;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [CustomEditor(typeof(TestScriptableObject))]
    public class TestEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(serializedObject.targetObjects.Length > 1) return;
            if(serializedObject.targetObject is not TestScriptableObject test) return;
            
            GUILayout.Label($"Question count: {test.groups.Sum(g=>g.questions.Length)}");
            if (GUILayout.Button("Recreate result groups"))
            {
                test.resultFields = new List<ResultFieldModel>();
                test.OnValidate();
            }
            
            /*GUILayout.Space(10);
            
            GUILayout.Label("Result fields");
            test.ResultFields = new List<ResultFieldModel>();
            var mainField = new ResultFieldModel("Main");
            test.ResultFields.Add(mainField);

            foreach (QuestionGroup testGroup in test.groups)
            {
                var groupFiled = new ResultFieldModel(testGroup.name);
                test.ResultFields.Add(groupFiled);
            }
            
            
            EditorGUILayout.BeginVertical();

            foreach (ResultFieldModel resultFieldModel in test.ResultFields)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(1);
                EditorGUILayout.LabelField(resultFieldModel.GroupName);
                EditorGUILayout.TextArea(resultFieldModel.ResultTitle);
                EditorGUILayout.TextArea(resultFieldModel.ResultDescription);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
            }
            
            EditorGUILayout.EndVertical();*/
        }
    }
}