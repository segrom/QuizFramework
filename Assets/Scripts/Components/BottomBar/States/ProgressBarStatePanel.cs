using System;
using System.Collections;
using DG.Tweening;
using Models;
using UnityEngine;

namespace Components.BottomBar.States
{
    public abstract class BaseBottomBarState: MonoBehaviour
    {
        public abstract BottomBarStateType StateType { get; }

        public abstract IEnumerator Setup(TestModel test);

        private CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        private CanvasGroup _canvasGroup;

        public virtual IEnumerator Show()
        {
            CanvasGroup.alpha = 0;
            gameObject.SetActive(true);
            yield return new WaitForEndOfFrame();
            yield return CanvasGroup.DOFade(1, 1f).WaitForCompletion();
            CanvasGroup.interactable = true;
        }
        
        public virtual IEnumerator Hide()
        {
            CanvasGroup.interactable = false;
            CanvasGroup.alpha = 1;
            yield return CanvasGroup.DOFade(0, 1f).WaitForCompletion();
            gameObject.SetActive(false);
        }
    }
}