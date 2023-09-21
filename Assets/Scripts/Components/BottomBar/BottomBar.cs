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
        public BottomBarStateType StateType => _currentState.StateType;

        [SerializeField] private StartBottomBarState startState;
        [SerializeField] private MainBottomBarState mainState;
        [SerializeField] private ResultsBottomBarState resultsState;
        private BaseBottomBarState[] _states;
        private BaseBottomBarState _currentState;
        
        public IEnumerator Setup(TestModel test)
        {
            _states = new BaseBottomBarState[] { startState, mainState, resultsState };
            
            yield return ChangeState(BottomBarStateType.Start);
            
            yield return _states.Select(state => state.Setup(test)).GetEnumerator();
        }
        
        public IEnumerator ChangeState(BottomBarStateType newStateType)
        {
            if(_currentState) yield return _currentState.Hide();
            _currentState = _states.First(s => s.StateType == newStateType);
            yield return _currentState.Show();
        }
        
        public IEnumerator QuestionChanged(TestQuestionModel newQuestion)
        {
           yield return mainState.ChangeCurrentQuestion(newQuestion);
        }
        
        public IEnumerator SetupResults(TestModel test)
        {
            yield return resultsState.Setup(test);
            yield return ChangeState(BottomBarStateType.Results);
        }
        
    }
}