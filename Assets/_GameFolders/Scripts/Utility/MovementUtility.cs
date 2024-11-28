using System;
using System.Collections;
using UnityEngine;

namespace _GameFolders.Scripts.Utility
{
    public static class MovementUtility
    {
        public static IEnumerator Move(
            Transform target,
            Vector3 targetPosition,
            float duration,
            IEnumerator onStart = null,
            Action onComplete = null)
        {
            yield return onStart;

            float elapsedTime = 0f;
            Vector3 startPosition = target.position;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                Vector3 forwardPos = Vector3.Lerp(startPosition, targetPosition, t);

                target.position = forwardPos;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.position = targetPosition;

            onComplete?.Invoke();
        }
        
        
        public static IEnumerator JumpMove(
            Transform target,
            Vector3 targetPosition,
            float duration,
            float jumpHeight,
            IEnumerator onStart = null,
            Action onComplete = null)
        {
            yield return onStart;

            float elapsedTime = 0f;
            Vector3 startPosition = target.position;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                Vector3 forwardPos = Vector3.Lerp(startPosition, targetPosition, t);

                float height = 4 * jumpHeight * t * (1 - t);

                target.position = new Vector3(forwardPos.x, startPosition.y + height, forwardPos.z);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.position = targetPosition;

            onComplete?.Invoke();
        }
    }
}