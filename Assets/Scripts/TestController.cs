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

public class TestController : MonoBehaviour
{
    [SerializeField] private TestScriptableObject test;
    [Space]
    [SerializeField] private TMP_Text title;
    [SerializeField] private Transform cardOrigin;
    [SerializeField] private BottomBar bottomBar;

    private BaseCard currentCard;
    private QuestionCard questionCardPrefab;
    private TestModel currentTest;
    
    private IEnumerator Start()
    {
        currentTest = new TestModel(test);
        
        title.text = currentTest.Title;
        
        yield return bottomBar.Setup(currentTest);
        
        yield return AddressableManager.GetAssetCoroutine<GameObject>(AddressableManager.StartCardAsset, startCardPref =>
        {
            var startCard = Instantiate(startCardPref, cardOrigin).GetComponent<StartCard>();
            currentCard = startCard;
            startCard.Setup(currentTest);
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
        
        yield return currentCard.Hide();
        AddressableManager.ReleaseAsset(AddressableManager.StartCardAsset);
        
        var task =  AddressableManager.GetAsset<GameObject>(AddressableManager.QuestionCardAsset);
        yield return new WaitUntil(() => task.IsCompleted);
        questionCardPrefab = task.Result.GetComponent<QuestionCard>();
        
        yield return GoQuestion(currentTest.AllQuestions.First());
        yield return a;
    }

    private IEnumerator NextQuestion()
    {
        currentTest.CurrentQuestionIndex++;

        if (currentTest.CurrentQuestionIndex >= currentTest.QuestionCount)
        {
            yield return bottomBar.ChangeState(BottomBarStateType.Results);
            yield return GoResults();
            yield break;
        }
        
        yield return GoQuestion(currentTest.AllQuestions[currentTest.CurrentQuestionIndex]);
    }

    private IEnumerator GoQuestion(TestQuestionModel question)
    {
        currentCard.OnNext -= OnNextCard;
        
        var a = StartCoroutine(bottomBar.QuestionChanged(question));
        yield return currentCard.Hide();
        Destroy(currentCard);
        
        var questionCard = Instantiate(questionCardPrefab, cardOrigin).GetComponent<QuestionCard>();
        currentCard = questionCard;

        currentCard.OnNext += OnNextCard;
        
        yield return questionCard.Setup(question, currentTest);
        
        
        yield return questionCard.Show();
        yield return a;
    }

    private void OnNextCard(BaseCard sender)
    {
        if (sender is QuestionCard questionCard)
        {
            title.text = sender.Title ?? currentTest.Title;
            StartCoroutine(NextQuestion());
        }
    }

    private IEnumerator GoResults()
    {
        currentCard.OnNext -= OnNextCard;
        yield return currentCard.Hide();
        Destroy(currentCard);

        AddressableManager.ReleaseAsset(AddressableManager.QuestionCardAsset);
        var task = AddressableManager.GetAsset<GameObject>(AddressableManager.ResultsCardAsset);
        yield return new WaitUntil(() => task.IsCompleted);
        
        var questionCard = Instantiate(task.Result.GetComponent<ResultsCard>(), cardOrigin);
        currentCard = questionCard;
        title.text = currentCard.Title ?? currentTest.Title;

        yield return questionCard.Setup(currentTest);
        yield return questionCard.Show();
    }
}