using System.Threading;
using UnityEngine;

namespace _GameFolders.Scripts.Interfaces
{
    public interface IJumpObject
    {
        public bool IsContinueJump { get; set; }
        public Rigidbody Rb { get; set; }
        public Animator AnimatorController { get; set; }
        public CancellationTokenSource JumpCancellationTokenSource { get; set; }
    }
}