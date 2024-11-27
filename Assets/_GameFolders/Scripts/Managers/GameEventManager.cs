using System;
using _GameFolders.Scripts.Helpers;

namespace _GameFolders.Scripts.Managers
{
    public static class GameEventManager
    {
        public static Action OnStart { get; set; }
        public static Action OnWin { get; set; }
        public static Action<FruitType, int, int> UpdateInventory { get; set; }
        
        public static Action<string> AnimatorSetTrigger { get; set; }
        public static Action<int> OnMoveTrigger { get; set; }
        
        public static Action<RollDiceType> RollDiceStart { get; set; }
        public static Action<RollDiceType> RollDiceEnd { get; set; }
        
    }
}