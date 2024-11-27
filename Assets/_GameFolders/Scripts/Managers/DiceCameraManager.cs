using _GameFolders.Scripts.Components;
using UnityEngine;

namespace _GameFolders.Scripts.Managers
{
    public class DiceCameraManager : MonoBehaviour
    {
        [SerializeField] private Dice lookAtDice;
        [SerializeField] private Vector3 offset;

        public Dice LookAtDice
        {
            get => lookAtDice;
            set => lookAtDice = value;
        }

        private void Update()
        {
            if (LookAtDice != null)
            {
                transform.LookAt(LookAtDice.transform.position + offset, transform.up);
            }
        }
    }
}