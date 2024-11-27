using System;
using _GameFolders.Scripts.Components;
using _GameFolders.Scripts.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _GameFolders.Scripts.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [Header("-- Main Menu --")] [SerializeField]
        private GameObject mainMenuPanel;

        [SerializeField] private Button startButton;

        [Header("-- Inventory Menu --")] [SerializeField]
        private TextMeshProUGUI appleCountTMP;

        [SerializeField] private TextMeshProUGUI pearCountTMP;
        [SerializeField] private TextMeshProUGUI strawberryCountTMP;


        [Header("{-- Normal Dice Variables --}")] [SerializeField]
        private GameObject dicePanel;

        [SerializeField] private GameObject diceCamera;
        [SerializeField] private GameObject diceFrame;

        [Header("{-- Bonus Dice Variables --}")] [SerializeField]
        private GameObject bonusDiceCamera;

        [SerializeField] private GameObject bonusDiceFrame;


        protected override void Awake()
        {
            base.Awake();
            ButtonListenerInit();
        }

        private void OnEnable()
        {
            GameEventManager.OnStart += OnStartHandler;
            GameEventManager.UpdateInventory += OnUpdateInventoryHandler;
            GameEventManager.OnWin += OnWinHandler;
            GameEventManager.RollDiceStart += OnRollDiceStartHandler;
            GameEventManager.RollDiceEnd += OnRollDiceEndHandler;
        }

        private void OnRollDiceEndHandler(RollDiceType rollDiceType)
        {
            switch (rollDiceType)
            {
                case RollDiceType.Normal:
                    diceCamera.SetActive(false);
                    diceFrame.SetActive(false);
                    break;
                case RollDiceType.Bonus:
                    bonusDiceCamera.SetActive(false);
                    bonusDiceFrame.SetActive(false);
                    break;
            }
        }

        private void OnRollDiceStartHandler(RollDiceType rollDiceType)
        {
            switch (rollDiceType)
            {
                case RollDiceType.Normal:
                    diceCamera.SetActive(true);
                    diceFrame.SetActive(true);
                    break;
                case RollDiceType.Bonus:
                    bonusDiceCamera.SetActive(true);
                    bonusDiceFrame.SetActive(true);
                    break;
            }
        }

        private void OnWinHandler()
        {
            dicePanel.SetActive(false);
        }

        private void OnUpdateInventoryHandler(FruitType fruitType, int startValue, int value)
        {
            switch (fruitType)
            {
                case FruitType.Apple:
                    NumberAnimation.Instance.Animate(appleCountTMP, startValue, value);
                    break;
                case FruitType.Pear:
                    NumberAnimation.Instance.Animate(pearCountTMP, startValue, value);
                    break;
                case FruitType.Strawberry:
                    NumberAnimation.Instance.Animate(strawberryCountTMP, startValue, value);
                    break;
            }
        }

        private void OnDisable()
        {
            GameEventManager.OnStart -= OnStartHandler;
            GameEventManager.UpdateInventory -= OnUpdateInventoryHandler;
            GameEventManager.OnWin -= OnWinHandler;
            GameEventManager.RollDiceStart -= OnRollDiceStartHandler;
            GameEventManager.RollDiceEnd -= OnRollDiceEndHandler;
        }

        private void OnStartHandler()
        {
            mainMenuPanel.SetActive(false);
        }

        private void ButtonListenerInit()
        {
            startButton.onClick.AddListener(() => GameEventManager.OnStart?.Invoke());
        }
    }
}