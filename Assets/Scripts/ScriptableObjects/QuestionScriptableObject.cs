using Models;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Question", menuName = "QuestionScriptableObject", order = 0)]
    public class QuestionScriptableObject : ScriptableObject
    {
        public string imagePath;
        public string question;
        public QuestionOptionModel[] options;
    }
}