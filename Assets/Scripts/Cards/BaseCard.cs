using System;
using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using Models;
using UnityEngine;

namespace Cards
{
    public abstract class BaseCard : MonoBehaviour
    {
        [CanBeNull] public virtual string Title { get; } = null;
        
        public event Action<BaseCard> OnNext;
        public event Action<BaseCard> OnBack;
        
        private CanvasGroup canvasGroup;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        protected void OnNextInternal()
        {
            OnNext?.Invoke(this);
        }
        
        protected void OnBackInternal()
        {
            OnBack?.Invoke(this);
        }

        
        public virtual IEnumerator Show()
        {
            transform.DOScale(0.6f, 0);
            
            var s = DOTween.Sequence();
            s.Join( canvasGroup.DOFade(1, 0.5f));
            s.Join( transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
            yield return s.WaitForCompletion();
        }
        
        public virtual IEnumerator Hide()
        {
            var s = DOTween.Sequence();
            s.Join(canvasGroup.DOFade(0, 0.3f));
            s.Join( transform.DOScale(0.5f, 0.8f).SetEase(Ease.InQuart));
            yield return s.WaitForCompletion();
        }
        
    }
}