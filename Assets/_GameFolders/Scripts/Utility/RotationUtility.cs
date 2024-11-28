using System;
using System.Collections;
using UnityEngine;

namespace _GameFolders.Scripts.Utility
{
    public static class RotationUtility
    {
        public static IEnumerator RotateOverTime(
            Transform target,
            Vector3 rotation,
            float duration,
            IEnumerator onStart = null,
            IEnumerator onComplete = null)
        {
            yield return onStart;

            Quaternion startRotation = target.localRotation;
            Quaternion targetRotation = startRotation * Quaternion.Euler(rotation);

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                target.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.localRotation = targetRotation;
            
            yield return onComplete;
        }
    }
}