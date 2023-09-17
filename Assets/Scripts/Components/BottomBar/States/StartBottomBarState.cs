using System.Collections;
using Models;
using TMPro;
using UnityEngine;

namespace Components.BottomBar.States
{
    public class StartBottomBarState: BaseBottomBarState
    {
        public override BottomBarStateType StateType => BottomBarStateType.Start;

        public string Title
        {
            get => title.text;
            set => title.text = value;
        }
        
        [SerializeField] private TMP_Text title;

        public override IEnumerator Setup(TestModel test)
        {
            Title = $"{test.QuestionCount} {DeclineNoun(test.QuestionCount)}";
            yield break;
        }

        private string DeclineNoun(int count)
        {
            if (count == 1 || (count > 20 && count % 10 == 1)) return "вопрос";
            if ((count is 2 or 3 or 4) || (count > 20 && count % 10 is 2 or 3 or 4)) return "вопроса";
            return "вопросов";
        }
    }
}