using System;
using System.Threading;
using System.Threading.Tasks;
using _GameFolders.Scripts.Helpers;
using _GameFolders.Scripts.Interfaces;
using UnityEngine;

namespace _GameFolders.Scripts.ObjectAnimationSystem
{
    public class JumpAnimationCommand : IAnimation
    {
        private readonly IJumpObject _iJumpObject;
        private readonly float _jumpHeight;
        private readonly Action _onComplete;
        

        public JumpAnimationCommand(IJumpObject jumpObject, float jumpHeight, Action onComplete = null)
        {
            _iJumpObject = jumpObject;
            _jumpHeight = jumpHeight;
            _onComplete = onComplete;
        }

        public async Task ExecuteAsync()
        {
            _iJumpObject.IsContinueJump = true;

            _iJumpObject.JumpCancellationTokenSource?.Cancel();
            _iJumpObject.JumpCancellationTokenSource = new CancellationTokenSource();


            _iJumpObject.AnimatorController.SetTrigger(Constants.Animation.Jump);
            
            await Task.Delay(700);
            
            _iJumpObject.Rb.velocity = Vector3.zero;
            _iJumpObject.Rb.AddForce(Vector3.up * _jumpHeight);

            await Task.Yield();
            
            _iJumpObject.IsContinueJump = false;
            _onComplete?.Invoke();
        }
    }
}