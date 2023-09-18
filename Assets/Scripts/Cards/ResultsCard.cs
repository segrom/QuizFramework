using System.Collections;
using System.Linq;
using DG.Tweening;
using Models;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class ResultsCard: BaseCard
    {
        public override string Title => "Итог";
        
        [SerializeField] private TMP_Text successPercentText;
        [SerializeField] private TMP_Text testResultText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image image;
        
        private int rightnessPercent;
        
        protected override void Awake()
        {
            base.Awake();
            successPercentText.DOFade(0, 0);
            testResultText.DOFade(0, 0);
            descriptionText.DOFade(0, 0);
            testResultText.transform.DOScaleY(0.5f, 0);
            descriptionText.transform.DOScaleY(0.5f, 0);
        }

        public IEnumerator Setup(TestModel test)
        {
            var (rightness, title, desc)= TestResultCalculator.CalculateResults(test);
            rightnessPercent = (int)(rightness * 100f);
            Debug.Log($"Success percent is {rightnessPercent}%");

            testResultText.text = title;
            descriptionText.text = desc;
            
            var s = DOTween.Sequence();
            s.Append(image.transform.DOLocalMoveY(50, 30));
            s.Append(image.transform.DOLocalMoveY(-50, 30));
            s.SetLoops(-1).Play();
            
            yield break;
        }

        public override IEnumerator Show()
        {
            yield return base.Show();
            
            var s = DOTween.Sequence();
            s.Join(DOVirtual.Int(0, rightnessPercent, 5, value => successPercentText.text = $"{value}%").SetEase(Ease.OutExpo));
            s.Join(successPercentText.DOFade(1, 5));
            yield return s.WaitForCompletion();

            s = DOTween.Sequence();
            s.Join( testResultText.DOFade(1, 1.5f));
            s.Join( testResultText.transform.DOScaleY(1, 1).SetEase(Ease.OutCirc));
            yield return s.WaitForCompletion();
            
            s = DOTween.Sequence();
            s.Join( descriptionText.DOFade(1, 1.5f));
            s.Join( descriptionText.transform.DOScaleY(1, 1).SetEase(Ease.OutCirc));
            yield return s.WaitForCompletion();
            
        }
    }
}