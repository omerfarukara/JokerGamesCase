using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _GameFolders.Scripts.Components;
using _GameFolders.Scripts.Helpers;
using _GameFolders.Scripts.Interfaces;
using _GameFolders.Scripts.Managers;
using _GameFolders.Scripts.ObjectAnimationSystem;

namespace _GameFolders.Scripts.Controllers
{
    public class PlayerController : MonoSingleton<PlayerController>, IMoveObject, IJumpObject, IRotationObject
    {
        [Header("[-- IsGround Ray Variables --]")] [Space] [SerializeField]
        private float isGroundedRayDistance;

        [SerializeField] private LayerMask isGroundedLayerMask;

        [Space] [Space] [Header("[-- Model --]")] [Space] [SerializeField]
        private PlayerAnimatorController playerAnimatorController;

        #region Move

        public bool IsContinueMove { get; set; }
        public Transform MoveTransform { get; set; }
        public CancellationTokenSource MoveCancellationTokenSource { get; set; }

        #endregion

        #region Jump

        public bool IsContinueJump { get; set; }
        public Rigidbody Rb { get; set; }
        public Animator AnimatorController { get; set; }
        public CancellationTokenSource JumpCancellationTokenSource { get; set; }

        #endregion

        #region Rotate

        public bool IsContinueRotate { get; set; }
        public Transform RotateTransform { get; set; }
        public CancellationTokenSource RotateCancellationTokenSource { get; set; }

        #endregion

        public int CurrentStepIndex { get; set; } = 0;
        public int StartStepIndex { get; set; } = 0;

        private RaycastHit _hit;
        private Vector3 _basePosition;

        private bool _isGrounded;
        private bool _isGoing;
        public bool IsGoing => _isGoing;


        private void OnEnable()
        {
            GameEventManager.OnMoveTrigger += OnMoveTriggerHandler;
        }

        protected override void Awake()
        {
            base.Awake();
            
            MoveTransform = transform;
            RotateTransform = transform;

            Rb = GetComponent<Rigidbody>();
            AnimatorController = playerAnimatorController.Animator;
            _basePosition = transform.position;
        }
        

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out Fruit fruit))
            {
                if (_isGoing) return;

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
        
        private void OnMoveTriggerHandler(int stepCount)
        {
            StepMove(stepCount, Vector3.forward * 3, 0.8f, 225, true).ConfigureAwait(true);
        }

        private async Task StepMove(int stepCount, Vector3 position, float moveDuration, float jumpForce, bool isLocalMove)
        {
            if (IsContinueJump || IsContinueMove) return;

            _isGoing = true;
            StartStepIndex = CurrentStepIndex;
            
            for (int i = 0; i < stepCount; i++)
            {
                JumpAnimationCommand jumpAnimation = new JumpAnimationCommand(this, jumpForce);
                MoveAnimationCommand moveAnimation = new MoveAnimationCommand(this, position, moveDuration, isLocalMove, () => CurrentStepIndex++);

                await ObjectAnimation.Instance.ExecuteAnimationsAsync(new List<IAnimation>() { jumpAnimation, moveAnimation });

                await WaitForJumpToLand();

                if (CheckReturnBase())
                {
                    int temporaryIndex = StartStepIndex + stepCount;
                    await ReturnBase(0.8f, 225, false, () => { StepMove(temporaryIndex - LevelManager.Instance.AllStepCountInLevel, Vector3.forward * 3, 0.8f, 225, true).ConfigureAwait(true); }).ConfigureAwait(true);
                }
                else if (i == stepCount - 1)
                {
                    _isGoing = false;

                    if (CheckWin())
                    {
                        GameManager.Instance.GameState = GameState.Win;
                        await WinMoveAndRotate(Vector3.forward * 3, 0.8f, 225f, new Vector3(0, 55, 0), 1, true).ConfigureAwait(true);
                    }
                }


                if (MoveCancellationTokenSource.IsCancellationRequested || JumpCancellationTokenSource.IsCancellationRequested)
                {
                    _isGoing = false;
                    break;
                }
            }
        }

        private async Task WinMoveAndRotate(Vector3 position, float moveDuration, float jumpForce, Vector3 localRotate, int rotateDuration, bool isLocalMove)
        {
            StopMoveAndJump();

            await Task.Delay(1000);

            JumpAnimationCommand jumpAnimation = new JumpAnimationCommand(this, jumpForce);
            MoveAnimationCommand moveAnimation = new MoveAnimationCommand(this, position, moveDuration, isLocalMove);
            RotateAnimationCommand rotateCommand = new RotateAnimationCommand(this, localRotate, rotateDuration);

            await ObjectAnimation.Instance.ExecuteAnimationsAsync(new List<IAnimation>() { jumpAnimation, moveAnimation, rotateCommand });
            await WaitForJumpToLand();

            GameEventManager.OnWin?.Invoke();
            GameEventManager.AnimatorSetTrigger?.Invoke(Constants.Animation.Dance);
        }

        private async Task ReturnBase(float moveDuration, float jumpForce, bool isLocalMove, Action newLapStepAction)
        {
            StopMoveAndJump();

            CurrentStepIndex = 0;

            JumpAnimationCommand jumpAnimation = new JumpAnimationCommand(this, jumpForce);
            MoveAnimationCommand moveAnimation = new MoveAnimationCommand(this, _basePosition, moveDuration, isLocalMove, () =>
            {
                StopMoveAndJump();
                newLapStepAction?.Invoke();
            });

            await ObjectAnimation.Instance.ExecuteAnimationsAsync(new List<IAnimation>() { jumpAnimation, moveAnimation });
        }


        #region Helper Methods

        private async Task WaitForJumpToLand()
        {
            while (!_isGrounded)
            {
                await Task.Yield();
            }
        }

        private void CheckIfGrounded()
        {
            Physics.Raycast(transform.position, Vector3.down, out _hit, isGroundedRayDistance, isGroundedLayerMask);
            _isGrounded = _hit.collider != null;
        }

        private bool CheckWin()
        {
            return CurrentStepIndex == LevelManager.Instance.AllStepCountInLevel;
        }

        private bool CheckReturnBase()
        {
            return CurrentStepIndex > LevelManager.Instance.AllStepCountInLevel;
        }

        private void StopMoveAndJump()
        {
            MoveCancellationTokenSource?.Cancel();
            JumpCancellationTokenSource?.Cancel();

            MoveCancellationTokenSource = null;
            JumpCancellationTokenSource = null;

            IsContinueMove = false;
            IsContinueJump = false;
        }

        #endregion

        private void OnDisable()
        {
            GameEventManager.OnMoveTrigger -= OnMoveTriggerHandler;

            MoveCancellationTokenSource?.Cancel();
            JumpCancellationTokenSource?.Cancel();

            MoveCancellationTokenSource?.Dispose();
            JumpCancellationTokenSource?.Dispose();
        }
    }
}