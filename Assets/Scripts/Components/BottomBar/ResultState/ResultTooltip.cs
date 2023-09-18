using System;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.BottomBar.ResultState
{
    public class ResultTooltip : MonoBehaviour
    {
        private static ResultTooltip instance;
        
        public static Color RightAnswerColor => instance.rightAnswerColor;
        public static Color WrongAnswerColor => instance.wrongAnswerColor;
        
        [SerializeField] private RawImage image;
        [SerializeField] private TMP_Text questionText;
        [SerializeField] private TMP_Text rightAnswer;
        [SerializeField] private TMP_Text userAnswer;
        [Space]
        [SerializeField] private Color rightAnswerColor;
        [SerializeField] private Color wrongAnswerColor;

        private void Awake()
        {
            instance = this;
            gameObject.SetActive(false);
        }

        public static void SetData(Vector3 pos,TestQuestionModel modelQuestion)
        {
           instance.transform.position = pos;
           instance.questionText.text = modelQuestion.Question;
           instance.rightAnswer.text = modelQuestion.RightOption.name;
           instance.userAnswer.text = modelQuestion.SelectedOption.name;
           instance.userAnswer.color = modelQuestion.IsRightSelected ? RightAnswerColor : WrongAnswerColor;
           instance.gameObject.SetActive(true);
            
           instance.StartCoroutine(AddressableManager.GetAssetCoroutine<Texture2D>(modelQuestion.ImagePath, texture2D =>
            {
                instance.image.texture = texture2D;
            }));
        }

        public static void Hide()
        {
            instance.gameObject.SetActive(false);
        }
    }
}