using System;
using System.Collections;
using System.Linq;
using Components.BottomBar.States;
using Models;
using UnityEngine;

namespace Components.BottomBar
{
    public enum BottomBarStateType
    {
        Start, Main, Results
    }
    public class BottomBar : MonoBehaviour
    {
        public BottomBarStateType StateType { get; private set; }

        [SerializeField] private StartBottomBarState startState;
        [SerializeField] private MainBottomBarState mainState;
        [SerializeField] private ResultsBottomBarState resultsState;
        private BaseBottomBarState[] states;
        
        public IEnumerator Setup(TestModel test)
        {
            states = new BaseBottomBarState[] { startState, mainState, resultsState };
            
            StateType = BottomBarStateType.Results; // shitty but
            yield return ChangeState(BottomBarStateType.Start);
            
            yield return states.Select(state => state.Setup(test)).GetEnumerator();
        }
        
        public IEnumerator ChangeState(BottomBarStateType newStateType)
        {
            if(newStateType == StateType) yield break;

            foreach (BaseBottomBarState state in states)
            {
                state.gameObject.SetActive(state.StateType == newStateType);
            }
        }

        
        public IEnumerator QuestionChanged(TestQuestionModel newQuestion)
        {
           yield return mainState.ChangeCurrentQuestion(newQuestion);
        }
        
        
    }
}