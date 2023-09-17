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
            get => isInteractive;
            set
            {
                if(value == isInteractive) return;
                currentMotion?.Kill();
                currentMotion = transform.DOScaleX(value ? 1f : 0f, 0.5f).SetEase(value? Ease.OutBack : Ease.OutCirc);
                isInteractive = value;
            }
        }
        private bool isInteractive = true;
        
        private Tweener currentFirst;
        private Tweener currentSecond;
        private Tweener currentMotion;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!isInteractive) return;
            currentMotion?.Kill();
            currentMotion = transform.DOScaleX(1.05f, 0.5f).SetEase(Ease.OutCirc);
            
            currentSecond?.Kill();
            currentSecond = DOVirtual.Color(gradient.SecondColor, hoverSecondColor, 0.5f, value => gradient.SecondColor = value);
            
            currentFirst?.Kill();
            currentFirst = DOVirtual.Color(gradient.FirstColor, hoverFirstColor, 0.5f, value => gradient.FirstColor = value);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!isInteractive) return;
            currentMotion?.Kill();
            currentMotion = transform.DOScaleX(1f, 1f).SetEase(Ease.OutQuint);
            
            currentSecond?.Kill();
            currentSecond = DOVirtual.Color(gradient.SecondColor, defaultSecondColor, 1f, value => gradient.SecondColor = value);
            
            currentFirst?.Kill();
            currentFirst = DOVirtual.Color(gradient.FirstColor, defaultFirstColor, 1f, value => gradient.FirstColor = value);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!isInteractive) return;
            currentMotion?.Kill();
            currentMotion = transform.DOScaleX(0.9f, 0.1f).SetEase(Ease.OutExpo);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(!isInteractive) return;
            currentMotion?.Kill();
            transform.DOScaleX(1f, 0.3f).SetEase(Ease.OutBack).OnComplete(() => onClick?.Invoke() );
        }
    }
}