using _GameFolders.Scripts.Helpers;
using UnityEngine;

namespace _GameFolders.Scripts.Managers
{
    public class InventoryManager : MonoSingleton<InventoryManager>
    {
        private void Start()
        {
            GameEventManager.UpdateInventory?.Invoke(FruitType.Apple, 0, Apple);
            GameEventManager.UpdateInventory?.Invoke(FruitType.Pear, 0, Pear);
            GameEventManager.UpdateInventory?.Invoke(FruitType.Strawberry, 0, Strawberry);
        }

        public int Apple
        {
            get => PlayerPrefs.GetInt(Constants.PlayerPref.Apple, 0);
            set
            {
                int startValue = Apple;
                
                PlayerPrefs.SetInt(Constants.PlayerPref.Apple, value);
                GameEventManager.UpdateInventory?.Invoke(FruitType.Apple, startValue, value);
            }
        }

        public int Pear
        {
            get => PlayerPrefs.GetInt(Constants.PlayerPref.Pear, 0);
            set
            {
                int startValue = Pear;
                PlayerPrefs.SetInt(Constants.PlayerPref.Pear, value);
                GameEventManager.UpdateInventory?.Invoke(FruitType.Pear, startValue, value);
            }
        }

        public int Strawberry
        {
            get => PlayerPrefs.GetInt(Constants.PlayerPref.Strawberry, 0);
            set
            {
                int startValue = Strawberry;

                PlayerPrefs.SetInt(Constants.PlayerPref.Strawberry, value);
                GameEventManager.UpdateInventory?.Invoke(FruitType.Strawberry, startValue, value);
            }
        }
    }
}