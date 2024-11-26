using _GameFolders.Scripts.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _GameFolders.Scripts.Components
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private Image fruitImage;
        [SerializeField] private TextMeshProUGUI countTmp;

        public FruitType FruitType { get; set; }
        public int Count { get; set; }

        public void Init(Sprite fruitSprite, FruitType fruitType, int fruitCount)
        {
            transform.localPosition = Vector3.right * transform.GetSiblingIndex() * 3;

            FruitType = fruitType;
            Count = fruitCount;

            fruitImage.sprite = fruitSprite;
            countTmp.text = $"{(fruitType != FruitType.Empty ? Count : "---")}";
        }
    }
}