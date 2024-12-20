using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private Sprite appleSprite;
        [SerializeField] private Sprite pearSprite;
        [SerializeField] private Sprite strawberrySprite;

        [Header("-- Json File --")] [SerializeField]
        private TextAsset levelJson;

        public int AllStepCountInLevel { get; private set; }

        private void OnEnable()
        {
            GameEventManager.OnStart += () => StartCoroutine(OnStartHandler());
        }

        private void OnDisable()
        {
            GameEventManager.OnStart -= () => StartCoroutine(OnStartHandler());
        }

        private IEnumerator OnStartHandler()
        {
            yield return new WaitForSeconds(0.100f);

            Level level = JsonUtility.FromJson<Level>(levelJson.text);

            AllStepCountInLevel = level.steps.Count;

            for (var index = 0; index < AllStepCountInLevel; index++)
            {
                StepObject stepObject = level.steps[index];

                Fruit spawnFruit = Instantiate(fruitObject, stepsParent).GetComponent<Fruit>();
                FruitType fruitType = (FruitType)stepObject.fruitType;

                switch (fruitType)
                {
                    case FruitType.Empty:
                        spawnFruit.Init(emptySprite, fruitType, stepObject.count);
                        break;
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

                yield return new WaitForSeconds(0.100f);
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