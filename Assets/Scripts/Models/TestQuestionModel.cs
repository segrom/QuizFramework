using System.Linq;
using ScriptableObjects;

namespace Models
{
    public class TestQuestionModel
    {
        public string ImagePath { get; }
        public string Question  { get; }
        public QuestionOptionModel[] Options  { get; }
        public QuestionOptionModel SelectedOption  { get; set; }
        public QuestionOptionModel RightOption  { get;}
        public bool IsRightSelected => SelectedOption == RightOption;
        
        public TestQuestionModel(QuestionScriptableObject questionScriptableObject)
        {
            ImagePath = questionScriptableObject.imagePath;
            Question = questionScriptableObject.question;
            Options = questionScriptableObject.options.Clone() as QuestionOptionModel[];
            RightOption = Options?.FirstOrDefault(o => o.isRight);
        }
    }
}