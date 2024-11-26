using System.Threading.Tasks;
using _GameFolders.Scripts.Components;
using _GameFolders.Scripts.Controllers;
using _GameFolders.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace _GameFolders.Scripts.Managers
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] private Dice firstDice, secondDice;
        [SerializeField] private TMP_InputField firstDiceInputField, secondDiceInputField;

        public void RollDice()
        {
            if (firstDice.IsRolling ||
                secondDice.IsRolling || 
                GameManager.Instance.GameState == GameState.Win ||
                PlayerController.Instance.IsGoing) return;

            RollDiceAsync().ConfigureAwait(true);
        }

        private async Task RollDiceAsync()
        {
            int firstDiceValue = int.Parse(firstDiceInputField.text);
            int secondDiceValue = int.Parse(secondDiceInputField.text);

            firstDice.SelectNumberAndRoll(firstDiceValue);
            secondDice.SelectNumberAndRoll(secondDiceValue);

            await Task.Delay(250);

            await WaitUntilFalse(() => secondDice.IsRolling);

            GameEventManager.OnMoveTrigger?.Invoke(firstDiceValue + secondDiceValue);
        }


        private async Task WaitUntilFalse(System.Func<bool> condition)
        {
            while (condition())
            {
                await Task.Yield();
            }
        }
    }
}