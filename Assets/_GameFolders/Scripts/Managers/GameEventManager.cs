using System;
using _GameFolders.Scripts.Helpers;
using TMPro;

namespace _GameFolders.Scripts.Managers
{
    public static class GameEventManager
    {
        public static Action OnStart { get; set; }
        public static Action<FruitType, int, int> UpdateInventory { get; set; }
    }
}