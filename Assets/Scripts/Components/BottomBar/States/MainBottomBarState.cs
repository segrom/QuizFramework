using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace Components.BottomBar.States
{
    public class MainBottomBarState: BaseBottomBarState
    {
        
        public override BottomBarStateType StateType => BottomBarStateType.Main;

        private BottomBarGroup groupPrefab;
        private List<BottomBarGroup> groups;
        private BottomBarGroup currentGroup;
        
        public override IEnumerator Setup(TestModel test)
        {
            var task = AddressableManager.GetAsset<GameObject>(AddressableManager.BottomBarGroup);
            yield return new WaitUntil(() => task.IsCompleted);
            groupPrefab = task.Result.GetComponent<BottomBarGroup>();

            groups = new List<BottomBarGroup>();
            foreach (QuestionGroupModel groupModel in test.Groups)
            {
                var group = Instantiate(groupPrefab, transform);
                yield return group.Setup(groupModel);
                groups.Add(group);
            }
        }

        public IEnumerator ChangeCurrentQuestion(TestQuestionModel newQuestion)
        {
            if (currentGroup is null)
            {
                currentGroup = groups.First();
                yield return currentGroup.ChangeActivity(true);
                yield break;
            }
            
            if (newQuestion.Group == currentGroup.GroupModel)
            {
                yield return currentGroup.ChangeQuestion(newQuestion);
            }
            else
            {
                yield return currentGroup.ChangeActivity(false);
                currentGroup = groups.First(g => g.GroupModel == newQuestion.Group);
                yield return currentGroup.ChangeActivity(true);
                yield return currentGroup.ChangeQuestion(newQuestion);
            }
        }
    }
}