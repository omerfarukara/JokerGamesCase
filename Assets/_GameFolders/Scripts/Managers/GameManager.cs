using _GameFolders.Scripts.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GameFolders.Scripts.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public GameState GameState { get; set; } = GameState.InGame;

        [SerializeField] private ParticleSystem confettiParticleSystem;

        private void OnEnable()
        {
            GameEventManager.OnWin += OnWinHandler;
        }

        private void OnDisable()
        {
            GameEventManager.OnWin -= OnWinHandler;
        }

        private void OnWinHandler()
        {
            confettiParticleSystem.Play();
        }

        public void ResetScene()
        {
            SceneManager.LoadScene(Constants.SceneNames.GameScene);
        }
    }
}