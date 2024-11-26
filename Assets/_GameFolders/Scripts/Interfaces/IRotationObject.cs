using System.Threading;
using UnityEngine;

namespace _GameFolders.Scripts.Interfaces
{
        public interface IRotationObject
        {
            public bool IsContinueRotate { get; set; }
            public Transform RotateTransform { get; set; }
            public CancellationTokenSource RotateCancellationTokenSource { get; set; }

        }
}