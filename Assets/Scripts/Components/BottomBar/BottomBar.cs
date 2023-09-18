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
        public BottomBarStateType StateType => currentState.StateType;

        [SerializeField] private StartBottomBarState startState;
        [SerializeField] private MainBottomBarState mainState;
        [SerializeField] private ResultsBottomBarState resultsState;
        private BaseBottomBarState[] states;
        private BaseBottomBarState currentState;
        
        public IEnumerator Setup(TestModel test)
        {
            states = new BaseBottomBarState[] { startState, mainState, resultsState };
            
            yield return ChangeState(BottomBarStateType.Start);
            
            yield return states.Select(state => state.Setup(test)).GetEnumerator();
        }
        
        public IEnumerator ChangeState(BottomBarStateType newStateType)
        {
            if(currentState) yield return currentState.Hide();
            currentState = states.First(s => s.StateType == newStateType);
            yield return currentState.Show();
        }
        
        public IEnumerator QuestionChanged(TestQuestionModel newQuestion)
        {
           yield return mainState.ChangeCurrentQuestion(newQuestion);
        }
        
        public IEnumerator SetupResults(TestModel test)
        {
            yield return resultsState.Setup(test);
        }
        
    }
}