using System.Linq;
using ScriptableObjects;

namespace Models
{
    public class TestQuestionModel
    {
        public string ImagePath { get; }
        public string Question  { get; }
        public QuestionGroupModel Group  { get; }
        public QuestionOptionModel[] Options  { get; }
        public QuestionOptionModel SelectedOption  { get; set; }
        public QuestionOptionModel RightOption  { get;}
        public bool IsRightSelected => SelectedOption == RightOption;
        
        public TestQuestionModel(QuestionScriptableObject questionScriptableObject, QuestionGroupModel group)
        {
            ImagePath = questionScriptableObject.imagePath;
            Question = questionScriptableObject.question;
            Group = group;
            Options = questionScriptableObject.options.Clone() as QuestionOptionModel[];
            RightOption = Options?.FirstOrDefault(o => o.isRight);
        }
    }
}