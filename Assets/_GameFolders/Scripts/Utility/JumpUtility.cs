using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GameFolders.Scripts.Utility
{
    public static class JumpUtility
    {
        public static IEnumerator Jump(
            Transform target,
            float jumpHeight,
            float duration,
            IEnumerator onStart = null,
            IEnumerator onComplete = null)
        {
            yield return onStart;

            float elapsedTime = 0f;
            Vector3 startPosition = target.position;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                float height = 4 * jumpHeight * t * (1 - t);

                Vector3 currentPosition = startPosition;
                currentPosition.y = startPosition.y + height;

                target.position = currentPosition;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Vector3 finalPosition = target.position;
            finalPosition.y = startPosition.y;
            target.position = finalPosition;

            yield return onComplete;
        }
    }
}