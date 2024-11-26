using System;
using System.Threading.Tasks;
using UnityEngine;

namespace _GameFolders.Scripts.Components
{
    public class Dice : MonoBehaviour
    {
        [SerializeField] private Rigidbody diceRigidbody;
        [SerializeField] private float throwForce = 10f;
        [SerializeField] private Vector3[] diceFaces = 
        {
            new Vector3(180, 0, 0),
            new Vector3(-90, 0, 0),
            new Vector3(0, 0, -90),
            new Vector3(0, 0, 90),
            new Vector3(90, 0, 0),
            new Vector3(0, 0, 0)
        };

        [SerializeField] private float settleDelay = 2f;
        [SerializeField] private int selectedNumber = 1;
        [SerializeField] private float rotationLerpSpeed = 5f;

        private bool _isRolling;
        public bool IsRolling => _isRolling;

        private void Start()
        {
            if (diceRigidbody == null)
                diceRigidbody = GetComponent<Rigidbody>();
        }

        private async void RollDice()
        {
            if (_isRolling)
            {
                Debug.Log("Zar şu anda atılıyor, lütfen bekleyin.");
                return;
            }

            _isRolling = true;

            await RollDiceAsync(selectedNumber);

            _isRolling = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RollDice();
            }
        }

        private async Task RollDiceAsync(int number)
        {
            ThrowDice();
        
            await Task.Delay(TimeSpan.FromSeconds(settleDelay));

            diceRigidbody.isKinematic = true;

            await RotateToTargetFaceAsync(number);

            diceRigidbody.isKinematic = false;
        }

        private void ThrowDice()
        {
            diceRigidbody.isKinematic = false;
            transform.rotation = UnityEngine.Random.rotation;

            Vector3 randomTorque = new Vector3(
                UnityEngine.Random.Range(-10f, 10f),
                UnityEngine.Random.Range(-10f, 10f),
                UnityEngine.Random.Range(-10f, 10f)
            );

            diceRigidbody.AddTorque(randomTorque, ForceMode.Impulse);
            diceRigidbody.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
        }

        private async Task RotateToTargetFaceAsync(int number)
        {
            Vector3 targetRotationEuler = diceFaces[number - 1];
            Quaternion targetRotation = Quaternion.Euler(targetRotationEuler);

            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.05f)
            {
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    targetRotation,
                    rotationLerpSpeed * Time.deltaTime
                );

                await Task.Yield();
            }
        }

        public void SelectNumberAndRoll(int number)
        {
            selectedNumber = number;
            RollDice();
        }
    }
}
