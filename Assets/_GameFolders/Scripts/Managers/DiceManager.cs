using System;
using System.Collections;
using System.Collections.Generic;
using _GameFolders.Scripts.Components;
using _GameFolders.Scripts.Controllers;
using _GameFolders.Scripts.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _GameFolders.Scripts.Managers
{
    public class DiceManager : MonoBehaviour
    {
        [Header("{-- Normal Dice Variables --}")]
        [SerializeField] private Button normalDiceButton;
        [SerializeField] private Dice firstDice, secondDice;
        [SerializeField] private TMP_InputField firstDiceInputField, secondDiceInputField;


        [Header("{-- Bonus Dice Variables --}")]
        [SerializeField] private Dice bonusDice;
        [SerializeField] private Transform bonusDiceParent;
        [SerializeField] private TMP_Dropdown bonusDropdown;


        [Header("{-- Dice Camera Managers --}")]
        [SerializeField] private DiceCameraManager bonusDiceCameraManager;
        
        private readonly List<Dice> _dices = new();

        private void Awake()
        {
            normalDiceButton.onClick.AddListener(RollDice);
        }

        private void Update()
        {
            bonusDropdown.interactable = DiceButtonsClickable();
            normalDiceButton.interactable = DiceButtonsClickable();
        }

        private void RollDice()
        {
            _dices.Clear();

            _dices.Add(firstDice);
            _dices.Add(secondDice);

            if (GameManager.Instance.GameState == GameState.Win || PlayerController.Instance.IsGoing) return;

            StartCoroutine(NormalRollDiceAsync());
        }

        private IEnumerator NormalRollDiceAsync()
        {
            int firstDiceValue = int.Parse(firstDiceInputField.text);
            int secondDiceValue = int.Parse(secondDiceInputField.text);

            if (!AcceptableDiceValue(firstDiceValue) || !AcceptableDiceValue(secondDiceValue)) yield return null;

            GameEventManager.RollDiceStart?.Invoke(RollDiceType.Normal);

            firstDice.SelectNumberAndRoll(firstDiceValue);
            secondDice.SelectNumberAndRoll(secondDiceValue);

            yield return new WaitForSeconds(0.250f);

            yield return new WaitUntil(() => !_dices[0].IsRolling);

            GameEventManager.OnMoveTrigger?.Invoke(firstDiceValue + secondDiceValue);
            
            _dices.Clear();
            
            yield return new WaitForSeconds(2f);

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
                foreach (Transform dice in bonusDiceParent)
                {
                    Destroy(dice.gameObject);
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

            StartCoroutine(BonusRollDiceAsync());
        }


        private IEnumerator BonusRollDiceAsync()
        {
            int totalAllDiceValue = 0;

            foreach (Dice dice in _dices)
            {
                int randomNumber = Random.Range(1, 7);
                totalAllDiceValue += randomNumber;
                dice.SelectNumberAndRoll(randomNumber);
            }

            yield return new WaitForSeconds(0.250f);

            yield return new WaitUntil(() => !_dices[0].IsRolling);


            GameEventManager.OnMoveTrigger?.Invoke(totalAllDiceValue);
            _dices.Clear();

            yield return new WaitForSeconds(2f);

            GameEventManager.RollDiceEnd?.Invoke(RollDiceType.Bonus);
        }
        

        private bool AcceptableDiceValue(int value)
        {
            return value is > 0 and < 7;
        }
        private bool DiceButtonsClickable()
        {
            return (_dices.Count <= 0 || !_dices[0].IsRolling) && !PlayerController.Instance.IsGoing;
        }
    }
}