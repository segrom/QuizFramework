using System;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.BottomBar.ResultState
{
    public class ResultTooltip : MonoBehaviour
    {
        private static ResultTooltip _instance;
        
        public static Color RightAnswerColor => _instance.rightAnswerColor;
        public static Color WrongAnswerColor => _instance.wrongAnswerColor;
        
        [SerializeField] private RawImage image;
        [SerializeField] private TMP_Text questionText;
        [SerializeField] private TMP_Text rightAnswer;
        [SerializeField] private TMP_Text userAnswer;
        [Space]
        [SerializeField] private Color rightAnswerColor;
        [SerializeField] private Color wrongAnswerColor;

        private void Awake()
        {
            _instance = this;
            gameObject.SetActive(false);
        }

        public static void SetData(Vector3 pos,TestQuestionModel modelQuestion)
        {
           _instance.transform.position = pos;
           _instance.questionText.text = modelQuestion.Question;
           _instance.rightAnswer.text = modelQuestion.RightOption.name;
           _instance.userAnswer.text = modelQuestion.SelectedOption.name;
           _instance.userAnswer.color = modelQuestion.IsRightSelected ? RightAnswerColor : WrongAnswerColor;
           _instance.gameObject.SetActive(true);
            
           _instance.StartCoroutine(AddressableManager.GetAssetCoroutine<Texture2D>(modelQuestion.ImagePath, texture2D =>
            {
                _instance.image.texture = texture2D;
            }));
        }

        public static void Hide()
        {
            _instance.gameObject.SetActive(false);
        }
    }
}