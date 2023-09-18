using System;
using System.Collections;
using DG.Tweening;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.BottomBar.MainState
{
    public class BottomBarGroup : MonoBehaviour
    {
        [SerializeField] private LayoutElement onActiveLayoutElement;
        [SerializeField]private CanvasGroup onActiveCanvasGroup;
        [Space]
        [SerializeField] private LayoutElement onInactiveLayoutElement;
        [SerializeField] private CanvasGroup onInactiveCanvasGroup;
        
        [Space] 
        [Header("active")] 
        [SerializeField] private TMP_Text titleActiveText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private LayoutElement progressTransform;
        
        [Space] 
        [Header("Inactive")] 
        [SerializeField] private TMP_Text titleInactiveText;

        public QuestionGroupModel GroupModel { get; private set; }
        
        public bool IsActive { get; private set; }

        private const float transitionDuration = 1f;
        private float lastPreferredWidth;
        private int lastProgressPercent;
        private RectTransform rectTransform;

        public IEnumerator Setup(QuestionGroupModel groupModel)
        {
            rectTransform = GetComponent<RectTransform>();
            progressTransform.preferredWidth = lastPreferredWidth = progressTransform.minWidth;
            GroupModel = groupModel;
            titleActiveText.text = groupModel.Name;
            titleInactiveText.text = groupModel.Name;
            
            onActiveCanvasGroup.gameObject.SetActive(IsActive);
            onActiveLayoutElement.enabled = IsActive;
            onActiveCanvasGroup.alpha = IsActive ? 1 : 0;
            
            onInactiveCanvasGroup.gameObject.SetActive(!IsActive);
            onInactiveLayoutElement.enabled = !IsActive;
            onInactiveCanvasGroup.alpha = !IsActive ? 1 : 0;
            
            yield break;
        }

        public IEnumerator ChangeActivity(bool value, int groupsCount, bool fast = false)
        {
            IsActive = value;

            if (fast)
            {
                
                onActiveCanvasGroup.gameObject.SetActive(value);
                onActiveCanvasGroup.alpha = value ? 1 : 0;
                onActiveLayoutElement.enabled = value;
            
                onInactiveCanvasGroup.gameObject.SetActive(!value);
                onInactiveCanvasGroup.alpha = value ? 1 : 0;
                onInactiveLayoutElement.enabled = !value;
                
                yield break;
            }

            var maxSize = transform.parent.GetComponentInParent<RectTransform>().rect.width -
                          onInactiveLayoutElement.preferredWidth * (groupsCount - 1) - 16f;
            
            if (!value)
            {
                yield return MoveProgress(1);

                onInactiveCanvasGroup.gameObject.SetActive(true);
            
                var oldPreferredWidth = onInactiveLayoutElement.preferredWidth;
                onInactiveLayoutElement.preferredWidth = maxSize;
                onActiveLayoutElement.enabled = false;
                onInactiveLayoutElement.enabled = true;

                var s1 = DOTween.Sequence();
                s1.Join(DOVirtual.Float(maxSize,
                        oldPreferredWidth, transitionDuration,
                        v => onInactiveLayoutElement.preferredWidth = v)
                    .SetEase(Ease.InOutCubic));
                s1.Join(DOVirtual.Float(0,
                    1, transitionDuration,
                    v => onInactiveCanvasGroup.alpha = v));
                s1.Join(DOVirtual.Float(1,
                    0, transitionDuration,
                    v => onActiveCanvasGroup.alpha = v));
                yield return s1.WaitForCompletion();
                
                onInactiveLayoutElement.preferredWidth = oldPreferredWidth;
                
                onActiveCanvasGroup.gameObject.SetActive(false);
                
                yield break;
            }
            
            onActiveCanvasGroup.gameObject.SetActive(true);
            
            var preferredWidth = onInactiveLayoutElement.preferredWidth;

            var s = DOTween.Sequence();
            s.Join(DOVirtual.Float(preferredWidth,
                    maxSize, transitionDuration,
                    v => onInactiveLayoutElement.preferredWidth = v)
                .SetEase(Ease.InOutCubic));
            s.Join(DOVirtual.Float(1,
                0, transitionDuration,
                v => onInactiveCanvasGroup.alpha = v));
            s.Join(DOVirtual.Float(0,
                1, transitionDuration,
                v => onActiveCanvasGroup.alpha = v));
            yield return s.WaitForCompletion();

            onActiveLayoutElement.enabled = true;
            onInactiveLayoutElement.enabled = false;
            onInactiveLayoutElement.preferredWidth = preferredWidth;

            onInactiveCanvasGroup.gameObject.SetActive(false);
        }
        
        public IEnumerator ChangeQuestion(TestQuestionModel newQuestion)
        {
            var index = -1;
            for (int i = 0; i < GroupModel.Size; i++)
            {
                if(GroupModel.Questions[i] != newQuestion) continue;
                index = i;
                break;
            }

            if (index < 0) throw new Exception("Question not found");

            yield return MoveProgress(index / (float)GroupModel.Size);

        }

        private IEnumerator MoveProgress(float progress)
        {
            var progressPercent = (int)(progress * 100f);
            var preferredWidth = progress * rectTransform.rect.width +
                                 Mathf.Lerp(progressTransform.minWidth, 0, progress);

            var s = DOTween.Sequence();
            
            s.Join(DOVirtual.Int(lastProgressPercent, 
                    progressPercent, transitionDuration, 
                    value => progressText.text = $"{value}%")
                .SetEase(Ease.InOutCubic));
            
            s.Join(DOVirtual.Float(lastPreferredWidth, 
                    preferredWidth, transitionDuration, 
                    value => progressTransform.preferredWidth = value).
                SetEase(Ease.InOutCubic));
            
            yield return s.WaitForCompletion();
            
            lastProgressPercent = progressPercent;
            lastPreferredWidth = preferredWidth;
        }
    }
}