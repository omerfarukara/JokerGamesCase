using System;
using System.Collections;
using _GameFolders.Scripts.Components;
using _GameFolders.Scripts.Helpers;
using _GameFolders.Scripts.Managers;
using _GameFolders.Scripts.Utility;
using UnityEngine;

namespace _GameFolders.Scripts.Controllers
{
    public class PlayerController : MonoSingleton<PlayerController>
    {
        [Header("[-- Movement Variables --]")] [SerializeField]
        private float moveDuration = 0.5f;

        [Header("[-- Jump Variables --]")] [SerializeField]
        private float jumpHeight = 2f;

        [Header("[-- Rotation Variables --]")] [SerializeField]
        private float winRotateDuration;

        [SerializeField] private Vector3 winRotateLocalAngle;

        [Header("[-- Ground Check Variables --]")] [SerializeField]
        private float isGroundedRayDistance = 0.1f;

        [SerializeField] private LayerMask isGroundedLayerMask;

        [Header("[-- Animator --]")] [SerializeField]
        private Animator animator;

        private Vector3 _basePosition;

        private bool _isGrounded;
        private int _currentStepIndex;

        public bool IsGoing { get; private set; }

        private void Start()
        {
            _basePosition = transform.position;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out Fruit fruit))
            {
                if (IsGoing) return;

                switch (fruit.FruitType)
                {
                    case FruitType.Apple:
                        InventoryManager.Instance.Apple += fruit.Count;
                        break;
                    case FruitType.Pear:
                        InventoryManager.Instance.Pear += fruit.Count;
                        break;
                    case FruitType.Strawberry:
                        InventoryManager.Instance.Strawberry += fruit.Count;
                        break;
                }
            }
        }

        private void Update()
        {
            CheckIfGrounded();
        }

        private void OnEnable()
        {
            GameEventManager.OnMoveTrigger += OnMoveTriggerHandler;
        }

        private void OnDisable()
        {
            GameEventManager.OnMoveTrigger -= OnMoveTriggerHandler;
        }

        private void OnMoveTriggerHandler(int stepCount)
        {
            if (!IsGoing)
            {
                StartCoroutine(HandleMovement(stepCount));
            }
        }

        private IEnumerator HandleMovement(int stepCount)
        {
            IsGoing = true;

            for (int i = 0; i < stepCount; i++)
            {
                Vector3 targetPosition = GetNormalStep();

                yield return MovementUtility.JumpMove(
                    transform,
                    targetPosition,
                    moveDuration,
                    jumpHeight,
                    onStart: JumpAnimation(),
                    onComplete: null
                );

                _currentStepIndex++;
                
                if (i == stepCount - 1) // Is Last Jump
                {
                    IsGoing = false;
                }

                yield return new WaitUntil(() => _isGrounded);

                if (ReturnBase())
                {
                    yield return MovementUtility.JumpMove(
                        transform,
                        _basePosition,
                        moveDuration * 2,
                        jumpHeight * 1.5f,
                        onStart: JumpAnimation(),
                        onComplete: null
                    );

                    _currentStepIndex = 0;
                    i--;
                }
            }

            if (IsLevelCompleted())
            {
                yield return HandleWin();
            }
        }

        private IEnumerator HandleWin()
        {
            yield return new WaitForSeconds(1f);

            GameManager.Instance.GameState = GameState.Win;

            Vector3 winPosition = GetNormalStep();
            yield return MovementUtility.JumpMove(
                transform,
                winPosition,
                moveDuration,
                jumpHeight,
                onStart: JumpAnimation(),
                onComplete: null
            );

            yield return RotationUtility.RotateOverTime(
                transform,
                winRotateLocalAngle,
                winRotateDuration,
                onStart: null,
                onComplete: null
            );

            GameEventManager.OnWin?.Invoke();
            animator.SetTrigger(Constants.Animation.Dance);
        }

        private IEnumerator JumpAnimation()
        {
            animator.SetTrigger(Constants.Animation.Jump);
            yield return new WaitForSeconds(0.750f);
        }

        private void CheckIfGrounded()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, isGroundedRayDistance, isGroundedLayerMask);
        }

        private Vector3 GetNormalStep()
        {
            return new Vector3(
                Mathf.Round(transform.position.x + transform.forward.x * 3),
                transform.position.y,
                Mathf.Round(transform.position.z + transform.forward.z * 3)
            );
        }

        private bool ReturnBase()
        {
            return _currentStepIndex > LevelManager.Instance.AllStepCountInLevel;
        }

        private bool IsLevelCompleted()
        {
            return _currentStepIndex == LevelManager.Instance.AllStepCountInLevel;
        }
    }
}