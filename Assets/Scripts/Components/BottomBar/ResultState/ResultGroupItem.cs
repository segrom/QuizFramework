using System.Collections;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Components.BottomBar.ResultState
{
    public class ResultGroupItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private Image bg;
        [SerializeField] private Transform tooltipPos;

        private TestQuestionModel question;
        
        public IEnumerator Setup(TestQuestionModel modelQuestion, int number)
        {
            numberText.text = number.ToString();
            question = modelQuestion;

            bg.color = modelQuestion.IsRightSelected ? ResultTooltip.RightAnswerColor : ResultTooltip.WrongAnswerColor;
            
            yield break;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ResultTooltip.SetData(tooltipPos.position, question);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ResultTooltip.Hide();
        }
    }
}