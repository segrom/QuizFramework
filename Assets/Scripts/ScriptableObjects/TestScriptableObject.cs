using Models;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTest", menuName = "TestScriptableObject", order = 0)]
    public class TestScriptableObject : ScriptableObject
    {
        public string title;
        public string description;
        public QuestionGroup[] groups;
        
    }
}