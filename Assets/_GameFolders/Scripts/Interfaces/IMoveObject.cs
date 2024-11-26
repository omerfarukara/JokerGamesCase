using System.Threading;
using UnityEngine;

namespace _GameFolders.Scripts.Interfaces
{
    public interface IMoveObject
    {
        public bool IsContinueMove { get; set; }
        public Transform MoveTransform { get; set; }

        public CancellationTokenSource MoveCancellationTokenSource { get; set; }
    }
}