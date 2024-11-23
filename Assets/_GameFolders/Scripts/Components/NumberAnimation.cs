using System;
using System.Collections;
using System.Threading.Tasks;
using _GameFolders.Scripts.Helpers;
using _GameFolders.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _GameFolders.Scripts.Components
{
    public class NumberAnimation : MonoSingleton<NumberAnimation>
    {
        [Header("Animation Settings")]
        [SerializeField] float animationDuration = 2f;

        private float _elapsedTime = 0f;

        public async void Animate(TextMeshProUGUI tmp, int startValue, int targetValue)
        {
            await AnimateNumber(tmp, startValue, targetValue);
        }

        private async Task AnimateNumber(TextMeshProUGUI tmp, int startValue, int targetValue)
        {
            _elapsedTime = 0f;

            while (_elapsedTime < animationDuration)
            {
                _elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(_elapsedTime / animationDuration);

                int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, progress));
                tmp.text = currentValue.ToString();

                await Task.Yield();
            }

            tmp.text = targetValue.ToString();
        }
    }
}