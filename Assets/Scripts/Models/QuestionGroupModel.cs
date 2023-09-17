using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine.Serialization;

namespace Models
{
    public class QuestionGroupModel
    {
        public string Name { get; private set; }
        public TestQuestionModel[] Questions { get; }
        public int Size => Questions.Length;

        public QuestionGroupModel(QuestionGroup obj)
        {
            Name = obj.name;
            var q = new List<TestQuestionModel>();
            foreach (QuestionScriptableObject questionScriptableObject in obj.questions)
            {
                q.Add(new TestQuestionModel(questionScriptableObject, this));
            }
            Questions = q.ToArray();
        }
    }
}