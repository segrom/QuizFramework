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

        public QuestionOptionModel Model => _model;
        private QuestionOptionModel _model;
        
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if(value == _isActive) return;
                _currentToggle?.Kill();
                _currentToggle = toggleSelection.DOScale(_isActive? 0 : 1, 0.2f).SetEase(_isActive ? Ease.InBack : Ease.OutExpo);
                _isActive = value;
            }
        }
        private bool _isActive;
        
        private Tweener _currentUnderline;
        private Tweener _currentToggle;
        
        public IEnumerator Setup(QuestionOptionModel optionModel)
        {
            _model = optionModel;
            
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.DOFade(0,0);
            
            label.text = _model.name;
            underline.DOFade(0, 0f);
            toggleSelection.DOScale(0, 0f);
            
            yield return canvasGroup.DOFade(1,0.4f).SetEase(Ease.InCirc).WaitForCompletion();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _currentUnderline?.Kill();
            _currentUnderline = underline.DOFade(1, 0.1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _currentUnderline?.Kill();
            _currentUnderline = underline.DOFade(0, 0.4f);
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