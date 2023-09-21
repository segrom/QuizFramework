using System;
using System.Collections;
using System.Linq;
using Cards;
using Components;
using Components.BottomBar;
using DG.Tweening;
using Models;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public enum TestStage
{
    Start, Main, Results,
}

public class TestController : MonoBehaviour
{
    [SerializeField] private TestScriptableObject test;
    [Space]
    [SerializeField] private TMP_Text title;
    [SerializeField] private Transform cardOrigin;
    [SerializeField] private BottomBar bottomBar;
    
    private BaseCard _currentCard;
    private QuestionCard _questionCardPrefab;
    private TestModel _currentTest;
    private TestStage _stage;
    private bool _adWasShown;
    
    private IEnumerator Start()
    {
        _stage = TestStage.Start;
        var bottomBarGroup = bottomBar.gameObject.AddComponent<CanvasGroup>();
        bottomBarGroup.alpha = 0;
        title.transform.DOScale(0.8f,0);
        title.DOFade(0, 0);
        
        _currentTest = new TestModel(test);
        title.text = _currentTest.Title; 
        
        yield return bottomBar.Setup(_currentTest);
        var s = DOTween.Sequence();
        s.Join( title.DOFade(1, 2f));
        s.Join( title.transform.DOScale(1,2f).SetEase(Ease.OutCirc));
        yield return s.WaitForCompletion();
        yield return bottomBarGroup.DOFade(1,1f).WaitForCompletion();
        
        yield return AddressableManager.GetAssetCoroutine<GameObject>(AddressableManager.StartCardAsset, startCardPref =>
        {
            var startCard = Instantiate(startCardPref, cardOrigin).GetComponent<StartCard>();
            _currentCard = startCard;
            startCard.Setup(_currentTest);
            startCard.OnNext += (_) =>
            {
                StartCoroutine(StartTest());
            };
            StartCoroutine(startCard.Show());
        });
    }

    private IEnumerator StartTest()
    {
        var a = StartCoroutine(bottomBar.ChangeState(BottomBarStateType.Main));
        
        yield return _currentCard.Hide();
        AddressableManager.ReleaseAsset(AddressableManager.StartCardAsset);
        
        var task =  AddressableManager.GetAsset<GameObject>(AddressableManager.QuestionCardAsset);
        yield return new WaitUntil(() => task.IsCompleted);
        _questionCardPrefab = task.Result.GetComponent<QuestionCard>();
        _stage = TestStage.Main;
        
        yield return GoQuestion(_currentTest.AllQuestions.First());
        yield return a;
    }

    private IEnumerator NextQuestion()
    {
        _currentTest.CurrentQuestionIndex++;

        if (_currentTest.CurrentQuestionIndex >= _currentTest.QuestionCount)
        {
            yield return GoResults();
            yield break;
        }
        
        yield return GoQuestion(_currentTest.AllQuestions[_currentTest.CurrentQuestionIndex]);
    }

    private IEnumerator GoQuestion(TestQuestionModel question)
    {
        _currentCard.OnNext -= OnNextCard;
        
        var a = StartCoroutine(bottomBar.QuestionChanged(question));
        yield return _currentCard.Hide();
        Destroy(_currentCard);
        
        var questionCard = Instantiate(_questionCardPrefab, cardOrigin).GetComponent<QuestionCard>();
        _currentCard = questionCard;

        _currentCard.OnNext += OnNextCard;
        
        yield return questionCard.Setup(question, _currentTest);
        
        yield return questionCard.Show();
        yield return a;
    }

    private void OnNextCard(BaseCard sender)
    {
        if (sender is QuestionCard)
        {
            title.text = sender.Title ?? _currentTest.Title;
            StartCoroutine(NextQuestion());
        }
    }

    private IEnumerator GoResults()
    {
        _stage = TestStage.Results;
        
        yield return _currentCard.Hide();
        Destroy(_currentCard);

        _adWasShown = false;
        YandexGame.ScriptsYG.YandexGame.FullscreenShow();
        
        _currentCard.OnNext -= OnNextCard;
        AddressableManager.ReleaseAsset(AddressableManager.QuestionCardAsset);
        
        var task = AddressableManager.GetAsset<GameObject>(AddressableManager.ResultsCardAsset);
        yield return new WaitUntil(() => task.IsCompleted);

        yield return new WaitWhile(() => _adWasShown);
        _adWasShown = false;
        
        var resultsCard = Instantiate(task.Result.GetComponent<ResultsCard>(), cardOrigin);
        _currentCard = resultsCard;
        title.text = _currentCard.Title ?? _currentTest.Title;

        var a = StartCoroutine(bottomBar.SetupResults(_currentTest));
        yield return resultsCard.Setup(_currentTest);
        yield return resultsCard.Show();
        
        yield return a;
    }

    public void OnFullscreenClosed()
    {
        _adWasShown = true;
        Debug.Log("Ads was shown");
    }
    
}