using System;
using Components;
using Models;
using TMPro;
using UnityEngine;

namespace Cards
{
    public class StartCard: BaseCard
    {
        [SerializeField] private GradientButton startButton;
        [SerializeField] private TMP_Text descText;

        protected override void Awake()
        {
            base.Awake();
            startButton.onClick.AddListener(OnNextInternal);
            startButton.IsInteractive = true;
        }

        public void Setup(TestModel test)
        {
            descText.text = test.Description;
        }
    }
}