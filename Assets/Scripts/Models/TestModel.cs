using ScriptableObjects;

namespace Models
{
    public class TestModel
    {
        public string Title { get; }
        public string Description { get; }
        
        public int CurrentQuestionIndex { get; set; }
        public readonly int QuestionCount;

        public TestQuestionModel[] Questions { get; private set; }

        public TestModel(TestScriptableObject testScriptableObject)
        {
            Title = testScriptableObject.title;
            Description = testScriptableObject.description;
            CurrentQuestionIndex = 0;
            QuestionCount = testScriptableObject.questions.Length;

            Questions = new TestQuestionModel[QuestionCount];

            for (int i = 0; i < QuestionCount; i++)
            {
                Questions[i] = new TestQuestionModel(testScriptableObject.questions[i]);
            }
        }
    }
}