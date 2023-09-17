using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YandexGame.ScriptsYG.Leaderboard;

namespace YandexGame.DebuggingMode.Scripts
{
    [HelpURL("https://www.notion.so/PluginYG-d457b23eee604b7aa6076116aab647ed#4968547185c2460fb70fd6eceaf101d4")]
    public class DebuggingModeYG : MonoBehaviour
    {
        [Tooltip("?payload=\nЭто значение, которое Вы будете передавать с помощью Deep Linking. Можете написать слово, например, debug и добавить свой пароль, например, 123. Получится debug123.")]
        public string payloadPassword = "debug123";
        [Tooltip("Отображение панели управления в Unity Editor")]
        public bool debuggingInEditor;

        [Serializable]
        public class LeaderboardTest
        {
            public LeaderboardYG leaderboardYG;
            public InputField nameLbInputField;
            public InputField scoreLbInputField;
        }
        public LeaderboardTest leaderboard;

        public static DebuggingModeYG Instance { get; private set; }

        private Canvas canvas;
        private Transform tr;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                if (!canvas) canvas = GetComponent<Canvas>();
                canvas.enabled = false;

                if (ScriptsYG.YandexGame.SDKEnabled) GetDataEvent();
            }
        }

        private void OnEnable() => ScriptsYG.YandexGame.GetDataEvent += GetDataEvent;
        private void OnDisable() => ScriptsYG.YandexGame.GetDataEvent -= GetDataEvent;

        public void GetDataEvent()
        {
            bool draw = false;
#if UNITY_EDITOR
            if (debuggingInEditor) draw = true;
#else
            if (YandexGame.EnvironmentData.payload == payloadPassword) draw = true;
#endif
            if (draw)
            {
                if (!canvas) canvas = GetComponent<Canvas>();
                canvas.enabled = true;

                if (!tr) tr = transform;

                tr.Find("Panel").Find("LanguageDebug").GetChild(0).GetComponent<Text>().text = ScriptsYG.YandexGame.savesData.language;

                string playerId = ScriptsYG.YandexGame.playerId;
                if (playerId.Length > 10)
                    playerId = playerId.Remove(10) + "...";

                tr.Find("Panel").Find("DebugData").GetChild(0).GetComponent<Text>().text = "playerName - " + ScriptsYG.YandexGame.playerName +
                    "\nplayerId - " + playerId +
                    "\nauth - " + ScriptsYG.YandexGame.auth +
                    "\nSDKEnabled - " + ScriptsYG.YandexGame.SDKEnabled +
                    "\ninitializedLB - " + ScriptsYG.YandexGame.initializedLB +
                    "\nphotoSize - " + ScriptsYG.YandexGame.photoSize +
                    "\ndomain - " + ScriptsYG.YandexGame.EnvironmentData.domain +
                    "\ndeviceType - " + ScriptsYG.YandexGame.EnvironmentData.deviceType +
                    "\nisMobile - " + ScriptsYG.YandexGame.EnvironmentData.isMobile +
                    "\nisDesktop - " + ScriptsYG.YandexGame.EnvironmentData.isDesktop +
                    "\nisTablet - " + ScriptsYG.YandexGame.EnvironmentData.isTablet +
                    "\nisTV - " + ScriptsYG.YandexGame.EnvironmentData.isTV +
                    "\nisTablet - " + ScriptsYG.YandexGame.EnvironmentData.isTablet +
                    "\nappID - " + ScriptsYG.YandexGame.EnvironmentData.appID +
                    "\nbrowserLang - " + ScriptsYG.YandexGame.EnvironmentData.browserLang +
                    "\npayload - " + ScriptsYG.YandexGame.EnvironmentData.payload +
                    "\npromptCanShow - " + ScriptsYG.YandexGame.EnvironmentData.promptCanShow +
                    "\nreviewCanShow - " + ScriptsYG.YandexGame.EnvironmentData.reviewCanShow;
            }
        }

        public void GetDataButton()
        {
            ScriptsYG.YandexGame.GetDataEvent?.Invoke();
        }

        public void AuthCheckButton()
        {
            GameObject.FindObjectOfType<ScriptsYG.YandexGame>().InitializationSDK();
        }

        public void AuthDialogButton()
        {
            GameObject.FindObjectOfType<ScriptsYG.YandexGame>()._OpenAuthDialog();
        }

        public void FullAdButton()
        {
            ScriptsYG.YandexGame.FullscreenShow();
        }

        public void VideoAdButton()
        {
            if (!tr) tr = transform;
            int id = int.Parse(tr.Find("Panel").Find("RewardAd").GetChild(0).GetComponent<InputField>().text);
            ScriptsYG.YandexGame.RewVideoShow(id);
        }

        public void StickyAdShowButton() => ScriptsYG.YandexGame.StickyAdActivity(true);
        public void StickyAdHideButton() => ScriptsYG.YandexGame.StickyAdActivity(false);


        public static Action onRBTRecalculate;
        public void RBTRecalculateButton()
        {
            onRBTRecalculate?.Invoke();
        }

        public static Action onRBTExecuteCode;
        public void RBTExecuteCodeButton()
        {
            onRBTExecuteCode?.Invoke();
        }

        public static Action<bool> onRBTActivity;
        public void RBTActivityButton(bool ativity)
        {
            onRBTActivity?.Invoke(ativity);
        }

        public void RedefineLangButton()
        {
            GameObject.FindObjectOfType<ScriptsYG.YandexGame>()._LanguageRequest();
        }

        public void SwitchLanguage(Text text)
        {
            ScriptsYG.YandexGame.SwitchLanguage(text.text);
        }

        public void PromptDialogButton()
        {
            ScriptsYG.YandexGame.PromptShow();
        }

        public void ReviewButton()
        {
            ScriptsYG.YandexGame.ReviewShow(false);
        }

        public void BuyPurchaseButton()
        {
            if (!tr) tr = transform;
            string id = tr.Find("Panel").Find("PurchaseID").GetChild(0).GetComponent<InputField>().text;
            ScriptsYG.YandexGame.BuyPayments(id);
        }

        public void DeletePurchaseButton()
        {
            if (!tr) tr = transform;
            string id = tr.Find("Panel").Find("PurchaseID").GetChild(0).GetComponent<InputField>().text;
            ScriptsYG.YandexGame.ConsumePurchaseByID(id);
        }

        public void DeleteAllPurchasesButton()
        {
            ScriptsYG.YandexGame.ConsumePurchases();
        }

        public void SaveButton()
        {
            ScriptsYG.YandexGame.SaveProgress();
        }

        public void LoadButton()
        {
            ScriptsYG.YandexGame.LoadProgress();
        }

        public void ResetSaveButton()
        {
            ScriptsYG.YandexGame.ResetSaveProgress();
        }

        public void SceneButton(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void NewNameLB()
        {
            leaderboard.leaderboardYG.nameLB = leaderboard.nameLbInputField.text;
            leaderboard.leaderboardYG.UpdateLB();
        }

        public void NewScoreLB()
        {
            ScriptsYG.YandexGame.NewLeaderboardScores(leaderboard.leaderboardYG.nameLB,
                int.Parse(leaderboard.scoreLbInputField.text));
        }

        public void NewScoreLBTimeConvert()
        {
            ScriptsYG.YandexGame.NewLBScoreTimeConvert(leaderboard.leaderboardYG.nameLB,
                float.Parse(leaderboard.scoreLbInputField.text));
        }
    }
}
