using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _GameFolders.Scripts.Components;
using _GameFolders.Scripts.Helpers;
using UnityEngine;

namespace _GameFolders.Scripts.Managers
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [Header("-- Level Initialize --")] [SerializeField]
        private Transform stepsParent;

        [SerializeField] private Fruit fruitObject;
        [SerializeField] private Sprite appleSprite;
        [SerializeField] private Sprite pearSprite;
        [SerializeField] private Sprite strawberrySprite;

        [Header("-- Json File --")] [SerializeField]
        private TextAsset levelJson;

        private void OnEnable()
        {
            GameEventManager.OnStart += OnStartHandler;
        }

        private void OnDisable()
        {
            GameEventManager.OnStart -= OnStartHandler;
        }

        private async void OnStartHandler()
        {
            await Task.Delay(100);
            Level level = JsonUtility.FromJson<Level>(levelJson.text);
            for (var index = 0; index < level.steps.Count; index++)
            {
                StepObject stepObject = level.steps[index];

                Debug.Log($"Fruit Type {stepObject.fruitType} \n Fruit Count {stepObject.count}");

                Fruit spawnFruit = Instantiate(fruitObject, stepsParent).GetComponent<Fruit>();
                FruitType fruitType = (FruitType)stepObject.fruitType;

                switch (fruitType)
                {
                    case FruitType.Apple:
                        spawnFruit.Init(appleSprite, fruitType, stepObject.count);
                        break;
                    case FruitType.Pear:
                        spawnFruit.Init(pearSprite, fruitType, stepObject.count);
                        break;
                    case FruitType.Strawberry:
                        spawnFruit.Init(strawberrySprite, fruitType, stepObject.count);
                        break;
                }
                
                await Task.Delay(100);

            }
        }
    }

    [Serializable]
    public class Level
    {
        public List<StepObject> steps;
    }

    [Serializable]
    public class StepObject
    {
        public int fruitType;
        public int count;
    }
}