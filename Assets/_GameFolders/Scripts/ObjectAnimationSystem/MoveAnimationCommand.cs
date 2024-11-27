using System;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using _GameFolders.Scripts.Interfaces;

namespace _GameFolders.Scripts.ObjectAnimationSystem
{
    public class MoveAnimationCommand : IAnimation
    {
        private readonly IMoveObject _moveObject;
        private readonly Vector3 _position;
        private readonly Action _onComplete;
        private readonly float _duration;
        private readonly bool _isLocalMove;

        public MoveAnimationCommand(IMoveObject moveObject, Vector3 position, float duration, bool isLocalMove, Action onComplete = null)
        {
            _moveObject = moveObject;
            _position = position;
            _duration = duration;
            _isLocalMove = isLocalMove;
            _onComplete = onComplete;
        }

        public async Task ExecuteAsync()
        {
            _moveObject.IsContinueMove = true;

            _moveObject.MoveCancellationTokenSource?.Cancel();
            _moveObject.MoveCancellationTokenSource = new CancellationTokenSource();


            Vector3 startPosition = _moveObject.MoveTransform.position;
            Vector3 targetPosition = _isLocalMove
                ? _moveObject.MoveTransform.TransformPoint(_position)
                : _position;

            float totalDistance = Vector3.Distance(startPosition, targetPosition);

            if (_duration <= 0)
            {
                Debug.LogError("Duration must be greater than zero.");
                return;
            }

            float speed = totalDistance / _duration;
            Vector3 direction = (targetPosition - startPosition).normalized;

            float elapsedTime = 0f;

            while (elapsedTime < _duration)
            {
                if (_moveObject.MoveCancellationTokenSource.IsCancellationRequested)
                {
                    Debug.Log("Animation cancelled.");
                    return;
                }

                float step = speed * Time.deltaTime;
                _moveObject.MoveTransform.position += direction * step;

                elapsedTime += Time.deltaTime;

                if (Vector3.Distance(_moveObject.MoveTransform.position, targetPosition) <= 0.001f)
                {
                    break;
                }

                await Task.Yield();
            }

            _moveObject.IsContinueMove = false;
            _onComplete?.Invoke();
        }
    }
}
