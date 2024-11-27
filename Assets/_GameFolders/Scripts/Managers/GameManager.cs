using System;
using _GameFolders.Scripts.Helpers;
using UnityEngine;

namespace _GameFolders.Scripts.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public GameState GameState { get; set; } = GameState.InGame;

        [SerializeField] private ParticleSystem conffectiParticleSystem;

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
            conffectiParticleSystem.Play();
        }
    }
}