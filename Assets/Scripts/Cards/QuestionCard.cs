using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Components;
using DG.Tweening;
using Models;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class QuestionCard : BaseCard
    {
        [SerializeField] private TMP_Text questionText;
        [SerializeField] private TMP_Text questionNumberText;
        [SerializeField] private Image questionImage;
        [SerializeField] private Transform optionsContainer;
        [SerializeField] private GradientButton nextButton, backButton;

        private TestQuestionModel question;
        private List<QuestionOption> options;

        protected override void Awake()
        {
            base.Awake();
            nextButton.IsInteractive = false;
            nextButton.onClick.AddListener(OnNextInternal);
            backButton.onClick.AddListener(OnBackInternal);
        }

        public IEnumerator Setup(TestQuestionModel questionModel, TestModel test)
        {
            question = questionModel;
            questionText.text = question.Question;
            questionNumberText.text = $"{test.CurrentQuestionIndex + 1}/{test.QuestionCount}";
            
            nextButton.IsInteractive = question.SelectedOption is not null;
            
            var optionPrefabTask = AddressableManager.GetAsset<GameObject>(AddressableManager.QuestionOptionAsset);
            yield return new WaitUntil(() => optionPrefabTask.IsCompleted);
            var optionPrefab = optionPrefabTask.Result .GetComponent<QuestionOption>();

            options = new List<QuestionOption>();
            foreach (QuestionOptionModel optionModel in question.Options)
            {
                var option = Instantiate(optionPrefab, optionsContainer);
                
                yield return option.Setup(optionModel);
                
                option.OnSelectionChange += OnOptionSelected;
                options.Add(option);
            }
            
            var questionImageTask = AddressableManager.GetAsset<Texture2D>(question.ImagePath);
            yield return new WaitUntil(() => questionImageTask.IsCompleted);
            var imageTexture = questionImageTask.Result;
            
            questionImage.sprite = Sprite.Create(imageTexture, new Rect(0,0, imageTexture.width, imageTexture.height), Vector2.zero);
            if (questionImage.TryGetComponent<AspectRatioFitter>(out var fitter))
            {
                fitter.aspectRatio = imageTexture.width / (float) imageTexture.height;
            }

            var s = DOTween.Sequence();
            s.Append(questionImage.transform.DOLocalMoveY(50, 30));
            s.Append(questionImage.transform.DOLocalMoveY(-50, 30));
            s.SetLoops(-1).Play();

        }

        private void OnOptionSelected(QuestionOption changed)
        {
            foreach (QuestionOption option in options)
            {
                option.IsActive = option == changed && changed.IsActive;
            } 
            
            question.SelectedOption = changed.IsActive ? changed.Model : null;
            nextButton.IsInteractive = question.SelectedOption is not null;
        }

        private void OnDestroy()
        {
            AddressableManager.ReleaseAsset(question.ImagePath);
        }
    }
}