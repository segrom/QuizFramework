using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;

namespace Components.BottomBar.ResultState
{
    public class BottomBarResultGroup : MonoBehaviour
    {
        [SerializeField] private RectTransform resultsContainer;
        [SerializeField] private TMP_Text title;

        private ResultGroupItem itemPrefab;
        private List<ResultGroupItem> items;
        
        public IEnumerator Setup(QuestionGroupModel groupModel)
        {
            var task = AddressableManager.GetAsset<GameObject>(AddressableManager.ResultGroupItem);
            yield return new WaitUntil(() => task.IsCompleted);
            itemPrefab = task.Result.GetComponent<ResultGroupItem>();

            title.text = groupModel.Name;
            
            items = new List<ResultGroupItem>();

            int number = 1;
            foreach (TestQuestionModel modelQuestion in groupModel.Questions)
            {
                var item = Instantiate(itemPrefab, resultsContainer);
                yield return item.Setup(modelQuestion, number++);
                items.Add(item);
            }
        }
    }
}