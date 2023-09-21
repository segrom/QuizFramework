using System.Collections;
using System.Collections.Generic;
using Components.BottomBar.ResultState;
using Models;
using UnityEngine;

namespace Components.BottomBar.States
{
    public class ResultsBottomBarState: BaseBottomBarState
    {
        public override BottomBarStateType StateType => BottomBarStateType.Results;

        private BottomBarResultGroup _resultGroupPrefab;
        private List<BottomBarResultGroup> _groups;
        private TestModel _test;
        
        public override IEnumerator Setup(TestModel testModel)
        {
            _test = testModel;
            var task = AddressableManager.GetAsset<GameObject>(AddressableManager.ResultGroup);
            yield return new WaitUntil(() => task.IsCompleted);
            _resultGroupPrefab = task.Result.GetComponent<BottomBarResultGroup>();
        }

        public override IEnumerator Show()
        {
            _groups = new List<BottomBarResultGroup>();
            foreach (QuestionGroupModel questionGroupModel in _test.Groups)
            {
                var group = Instantiate(_resultGroupPrefab, transform);
                yield return group.Setup(questionGroupModel);
                _groups.Add(group);
            }
            yield return base.Show();
        }
    }
}