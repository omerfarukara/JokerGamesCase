using _GameFolders.Scripts.Managers;
using UnityEngine;

namespace _GameFolders.Scripts.Controllers
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private Animator _animator;
        public Animator Animator => _animator;
        
        private void OnEnable()
        {
            GameEventManager.AnimatorSetTrigger += AnimatorSetTriggerHandler;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnDisable()
        {
            GameEventManager.AnimatorSetTrigger -= AnimatorSetTriggerHandler;
        }

        private void AnimatorSetTriggerHandler(string param)
        {
            Animator.SetTrigger(param);
        }
    }
}
