using _GameFolders.Scripts.Components;
using UnityEngine;

namespace _GameFolders.Scripts.Managers
{
    public class DiceCameraManager : MonoBehaviour
    {
        [SerializeField] private Dice dice;
        [SerializeField] private Vector3 offset;

        private void Update()
        {
            transform.LookAt(dice.transform.position+offset,transform.up);
        }
    }
}
