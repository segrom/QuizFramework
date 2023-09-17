using System;
using System.Collections;
using DG.Tweening;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Components
{
    public class QuestionOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private RectTransform toggleSelection;
        [SerializeField] private Image underline;
        
        public event Action<QuestionOption> OnSelectionChange;

        public QuestionOptionModel Model => model;
        private QuestionOptionModel model;
        
        public bool IsActive
        {
            get => isActive;
            set
            {
                if(value == isActive) return;
                currentToggle?.Kill();
                currentToggle = toggleSelection.DOScale(isActive? 0 : 1, 0.2f).SetEase(isActive ? Ease.InBack : Ease.OutExpo);
                isActive = value;
            }
        }
        private bool isActive;
        
        private Tweener currentUnderline;
        private Tweener currentToggle;
        
        public IEnumerator Setup(QuestionOptionModel optionModel)
        {
            model = optionModel;
            
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.DOFade(0,0);
            
            label.text = model.name;
            underline.DOFade(0, 0f);
            toggleSelection.DOScale(0, 0f);
            
            yield return canvasGroup.DOFade(1,0.4f).SetEase(Ease.InCirc).WaitForCompletion();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            currentUnderline?.Kill();
            currentUnderline = underline.DOFade(1, 0.1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            currentUnderline?.Kill();
            currentUnderline = underline.DOFade(0, 0.4f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsActive = !IsActive;
            OnSelectionChange?.Invoke(this);
        }
    }
}