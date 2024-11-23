using System;
using _GameFolders.Scripts.Helpers;
using Cinemachine;
using UnityEngine;

namespace _GameFolders.Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera canvasVirtualCamera, defaultVirtualCamera;

        private void OnEnable()
        {
            GameEventManager.OnStart += OnStartHandler;
        }

        private void OnStartHandler()
        {
            ChangeCamera(VirtualCameraType.Default);
        }

        private void OnDisable()
        {
            GameEventManager.OnStart -= OnStartHandler;
        }
        
        public void ChangeCamera(VirtualCameraType virtualCameraType)
        {
            canvasVirtualCamera.Priority = virtualCameraType == VirtualCameraType.Canvas ? 1 : 0;
            defaultVirtualCamera.Priority = virtualCameraType == VirtualCameraType.Default ? 1 : 0;
        }
    }
}
