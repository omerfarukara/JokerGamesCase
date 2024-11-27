using System.Collections.Generic;
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
        [Header("{-- Normal Dice Variables --}")]
        [SerializeField] private Dice firstDice, secondDice;
        [SerializeField] private TMP_InputField firstDiceInputField, secondDiceInputField;


        [Header("{-- Bonus Dice Variables --}")]
        [SerializeField] private Dice bonusDice;
        [SerializeField] private Transform bonusDiceParent;
        [SerializeField] private TMP_Dropdown bonusDropdown;


        [Header("{-- Dice Camera Managers --}")]
        [SerializeField] private DiceCameraManager normalDiceCameraManager;
        [SerializeField] private DiceCameraManager bonusDiceCameraManager;
        
        private readonly List<Dice> _dices = new();


        public void RollDice()
        {
            _dices.Clear();

            _dices.Add(firstDice);
            _dices.Add(secondDice);

            if (GameManager.Instance.GameState == GameState.Win || PlayerController.Instance.IsGoing) return;

            NormalRollDiceAsync().ConfigureAwait(true);
        }

        private async Task NormalRollDiceAsync()
        {
            int firstDiceValue = int.Parse(firstDiceInputField.text);
            int secondDiceValue = int.Parse(secondDiceInputField.text);

            if (!AcceptableDiceValue(firstDiceValue) || !AcceptableDiceValue(secondDiceValue)) return;

            GameEventManager.RollDiceStart?.Invoke(RollDiceType.Normal);

            firstDice.SelectNumberAndRoll(firstDiceValue);
            secondDice.SelectNumberAndRoll(secondDiceValue);

            await Task.Delay(250);

            await WaitUntilFalse(() => secondDice.IsRolling);

            GameEventManager.OnMoveTrigger?.Invoke(firstDiceValue + secondDiceValue);
            
            await Task.Delay(2000);

            GameEventManager.RollDiceEnd?.Invoke(RollDiceType.Normal);
        }


        public void BonusDicesSpawnAndRoll()
        {
            bonusDropdown.Hide();
            
            if (GameManager.Instance.GameState == GameState.Win || PlayerController.Instance.IsGoing) return;

            int value = bonusDropdown.value + 1;
            

            _dices.Clear();

            if (bonusDiceParent.childCount > 0)
            {
                foreach (GameObject dice in bonusDiceParent)
                {
                    Destroy(dice);
                }
            }
            
            GameEventManager.RollDiceStart?.Invoke(RollDiceType.Bonus);

            float radius = 1;
            for (int i = 0; i < value; i++)
            {
                float angle = i * Mathf.PI * 2 / value;
                Vector3 position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

                Dice dice = Instantiate(bonusDice, bonusDiceParent);
                dice.transform.localPosition = position;
                dice.transform.localRotation = Quaternion.identity;

                _dices.Add(dice);

                if (i == 0)
                    bonusDiceCameraManager.LookAtDice = dice;
            }

            BonusRollDiceAsync().ConfigureAwait(true);
        }


        private async Task BonusRollDiceAsync()
        {
            int totalAllDiceValue = 0;

            foreach (Dice dice in _dices)
            {
                int randomNumber = Random.Range(1, 7);
                totalAllDiceValue += randomNumber;
                dice.SelectNumberAndRoll(randomNumber);
            }

            await Task.Delay(250);

            await WaitAllDice();

            GameEventManager.OnMoveTrigger?.Invoke(totalAllDiceValue);
            
            await Task.Delay(2000);
            GameEventManager.RollDiceEnd?.Invoke(RollDiceType.Bonus);
        }


        private async Task WaitUntilFalse(System.Func<bool> condition)
        {
            while (condition())
            {
                await Task.Yield();
            }
        }

        private async Task WaitAllDice()
        {
            foreach (Dice dice in _dices)
            {
                await WaitUntilFalse(() => dice.IsRolling);
            }
        }

        private bool AcceptableDiceValue(int value)
        {
            return value is > 0 and < 7;
        }
    }
}