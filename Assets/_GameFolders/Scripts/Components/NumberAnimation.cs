using System.Collections;
using _GameFolders.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace _GameFolders.Scripts.Components
{
    public class NumberAnimation : MonoSingleton<NumberAnimation>
    {
        [Header("Animation Settings")]
        [SerializeField] float animationDuration = 2f;

        private float _elapsedTime = 0f;

        public void Animate(TextMeshProUGUI tmp, int startValue, int targetValue)
        {
            StartCoroutine(AnimateNumber(tmp, startValue, targetValue));
        }

        private IEnumerator AnimateNumber(TextMeshProUGUI tmp, int startValue, int targetValue)
        {
            _elapsedTime = 0f;

            while (_elapsedTime < animationDuration)
            {
                _elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(_elapsedTime / animationDuration);

                int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, progress));
                tmp.text = currentValue.ToString();

                yield return null;
            }

            tmp.text = targetValue.ToString();
        }
    }
}