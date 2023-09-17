using System;

namespace ScriptableObjects
{
    [Serializable]
    public class QuestionGroup
    {
        public string name;
        public QuestionScriptableObject[] questions;
    }
}