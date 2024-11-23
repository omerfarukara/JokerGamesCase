using System;
using _GameFolders.Scripts.Helpers;
using UnityEngine;

namespace _GameFolders.Scripts.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                InventoryManager.Instance.Apple += 10;
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                InventoryManager.Instance.Pear += 50;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                InventoryManager.Instance.Strawberry += 100;
            }
        }
    }
}