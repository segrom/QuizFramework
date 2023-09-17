using System.Linq;
using ScriptableObjects;

namespace Models
{
    public class TestModel
    {
        public string Title { get; }
        public string Description { get; }
        
        public int CurrentQuestionIndex { get; set; }
        public readonly int QuestionGroupCount;
        public readonly int QuestionCount;

        public QuestionGroupModel[] Groups { get; private set; }
        public TestQuestionModel[] AllQuestions => Groups.SelectMany(g => g.Questions).ToArray();

        public TestModel(TestScriptableObject testScriptableObject)
        {
            Title = testScriptableObject.title; 
            Description = testScriptableObject.description;
            CurrentQuestionIndex = 0;
            
            QuestionGroupCount = testScriptableObject.groups.Length;
            Groups = new QuestionGroupModel[QuestionGroupCount];

            for (int i = 0; i < QuestionGroupCount; i++)
            {
                Groups[i] = new QuestionGroupModel(testScriptableObject.groups[i]);
            }
            
            QuestionCount = Groups.Sum(g=>g.Size);
        }
    }
}