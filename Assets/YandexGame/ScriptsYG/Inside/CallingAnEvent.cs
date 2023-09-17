#if UNITY_EDITOR
using System.Collections;
using UnityEngine;

namespace YandexGame.ScriptsYG.Inside
{
    public class CallingAnEvent : MonoBehaviour
    {
        public IEnumerator CallingAd(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            YandexGame.Instance.CloseFullAd();
            Destroy(gameObject);
        }

        public IEnumerator CallingAd(float duration, int id)
        {
            yield return new WaitForSecondsRealtime(duration);
            YandexGame.Instance.RewardVideo(id);
            YandexGame.Instance.CloseVideo();
            Destroy(gameObject);
        }
    }
}
#endif