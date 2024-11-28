using _GameFolders.Scripts.Helpers;
using Cinemachine;
using UnityEngine;

namespace _GameFolders.Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera canvasVirtualCamera, defaultVirtualCamera,winVirtualCamera;

        private void OnEnable()
        {
            GameEventManager.OnStart += OnStartHandler;
            GameEventManager.OnWin += OnWinHandler;
        }

        private void OnDisable()
        {
            GameEventManager.OnStart -= OnStartHandler;
            GameEventManager.OnWin -= OnWinHandler;
        }
        
        private void OnStartHandler()
        {
            ChangeCamera(VirtualCameraType.Default);
        }

        private void OnWinHandler()
        {
            ChangeCamera(VirtualCameraType.Win);
        }

        private void ChangeCamera(VirtualCameraType virtualCameraType)
        {
            canvasVirtualCamera.Priority = virtualCameraType == VirtualCameraType.Canvas ? 1 : 0;
            defaultVirtualCamera.Priority = virtualCameraType == VirtualCameraType.Default ? 1 : 0;
            winVirtualCamera.Priority = virtualCameraType == VirtualCameraType.Win ? 1 : 0;
        }
    }
}
