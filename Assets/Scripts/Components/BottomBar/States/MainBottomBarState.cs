using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Components.BottomBar.MainState;
using Models;
using UnityEngine;

namespace Components.BottomBar.States
{
    public class MainBottomBarState: BaseBottomBarState
    {
        
        public override BottomBarStateType StateType => BottomBarStateType.Main;

        private BottomBarGroup _groupPrefab;
        private List<BottomBarGroup> _groups;
        private BottomBarGroup _currentGroup;
        
        public override IEnumerator Setup(TestModel test)
        {
            var task = AddressableManager.GetAsset<GameObject>(AddressableManager.BottomBarGroup);
            yield return new WaitUntil(() => task.IsCompleted);
            _groupPrefab = task.Result.GetComponent<BottomBarGroup>();

            _groups = new List<BottomBarGroup>();
            foreach (QuestionGroupModel groupModel in test.Groups)
            {
                var group = Instantiate(_groupPrefab, transform);
                yield return group.Setup(groupModel);
                _groups.Add(group);
            }
        }

        public override IEnumerator Hide()
        {
            yield return _currentGroup.ChangeActivity(false, _groups.Count);
            yield return base.Hide();
        }

        public IEnumerator ChangeCurrentQuestion(TestQuestionModel newQuestion)
        {
            if (_currentGroup is null)
            {
                _currentGroup = _groups.First();
                yield return _currentGroup.ChangeActivity(true, _groups.Count);
                yield break;
            }
            
            if (newQuestion.Group == _currentGroup.GroupModel)
            {
                yield return _currentGroup.ChangeQuestion(newQuestion);
            }
            else
            {
                yield return _currentGroup.ChangeActivity(false, _groups.Count);
                _currentGroup = _groups.First(g => g.GroupModel == newQuestion.Group);
                yield return _currentGroup.ChangeActivity(true, _groups.Count);
                yield return _currentGroup.ChangeQuestion(newQuestion);
            }
        }
    }
}