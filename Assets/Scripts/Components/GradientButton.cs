using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Gradient = Tools.Gradient;

namespace Components
{
    public class GradientButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Gradient gradient;
        [SerializeField] private Color defaultFirstColor;
        [SerializeField] private Color defaultSecondColor;
        [Space]
        [SerializeField] private Color hoverFirstColor;
        [SerializeField] private Color hoverSecondColor;
        [Space]
        public Button.ButtonClickedEvent onClick;

        public bool IsInteractive
        {
            get => _isInteractive;
            set
            {
                if(value == _isInteractive) return;
                _currentMotion?.Kill();
                _currentMotion = transform.DOScaleX(value ? 1f : 0f, 0.5f).SetEase(value? Ease.OutBack : Ease.OutCirc);
                _isInteractive = value;
            }
        }
        private bool _isInteractive = true;
        
        private Tweener _currentFirst;
        private Tweener _currentSecond;
        private Tweener _currentMotion;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!_isInteractive) return;
            _currentMotion?.Kill();
            _currentMotion = transform.DOScaleX(1.05f, 0.5f).SetEase(Ease.OutCirc);
            
            _currentSecond?.Kill();
            _currentSecond = DOVirtual.Color(gradient.SecondColor, hoverSecondColor, 0.5f, value => gradient.SecondColor = value);
            
            _currentFirst?.Kill();
            _currentFirst = DOVirtual.Color(gradient.FirstColor, hoverFirstColor, 0.5f, value => gradient.FirstColor = value);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!_isInteractive) return;
            _currentMotion?.Kill();
            _currentMotion = transform.DOScaleX(1f, 1f).SetEase(Ease.OutQuint);
            
            _currentSecond?.Kill();
            _currentSecond = DOVirtual.Color(gradient.SecondColor, defaultSecondColor, 1f, value => gradient.SecondColor = value);
            
            _currentFirst?.Kill();
            _currentFirst = DOVirtual.Color(gradient.FirstColor, defaultFirstColor, 1f, value => gradient.FirstColor = value);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!_isInteractive) return;
            _currentMotion?.Kill();
            _currentMotion = transform.DOScaleX(0.9f, 0.1f).SetEase(Ease.OutExpo);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(!_isInteractive) return;
            _currentMotion?.Kill();
            transform.DOScaleX(1f, 0.3f).SetEase(Ease.OutBack).OnComplete(() => onClick?.Invoke() );
        }
    }
}