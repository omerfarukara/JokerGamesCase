using _GameFolders.Scripts.Helpers;
using UnityEngine;

namespace _GameFolders.Scripts.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public GameState GameState { get; set; } = GameState.InGame;
    }
}