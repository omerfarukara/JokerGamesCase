using System;
using System.Threading;
using System.Threading.Tasks;
using _GameFolders.Scripts.Interfaces;
using UnityEngine;

namespace _GameFolders.Scripts.ObjectAnimationSystem
{
    public class RotateAnimationCommand : IAnimation
    {
        private readonly IRotationObject _iRotateObject;
        private readonly Action _onComplete;
        private readonly Vector3 _rotationDelta;
        private readonly float _duration;

        public RotateAnimationCommand(IRotationObject iRotateObject, Vector3 rotationDelta, float duration, Action onComplete = null)
        {
            _iRotateObject = iRotateObject;
            _rotationDelta = rotationDelta;
            _duration = duration;
            _onComplete = onComplete;
        }

        public async Task ExecuteAsync()
        {
            _iRotateObject.IsContinueRotate = true;

            _iRotateObject.RotateCancellationTokenSource?.Cancel();
            _iRotateObject.RotateCancellationTokenSource = new CancellationTokenSource();

            CancellationToken token = _iRotateObject.RotateCancellationTokenSource.Token;

            Quaternion startRotation = _iRotateObject.RotateTransform.localRotation;
            Quaternion targetRotation = startRotation * Quaternion.Euler(_rotationDelta);

            if (_duration <= 0)
            {
                return;
            }
            
            float elapsedTime = 0f;

            while (elapsedTime < _duration)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                float t = elapsedTime / _duration;
                _iRotateObject.RotateTransform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);

                elapsedTime += Time.deltaTime;

                await Task.Yield();
            }

            _iRotateObject.RotateTransform.localRotation = targetRotation;

            _iRotateObject.IsContinueRotate = false;
            _onComplete?.Invoke();
        }
    }
}
