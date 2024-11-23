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

        private FruitType _fruitType;

        public void Init(Sprite fruitSprite, FruitType fruitType, int fruitCount)
        {
            transform.localPosition = Vector3.right * transform.GetSiblingIndex() * 2;
            
            countTmp.text = $"{fruitCount}";
            fruitImage.sprite = fruitSprite;
            _fruitType = fruitType;
        }
    }
}