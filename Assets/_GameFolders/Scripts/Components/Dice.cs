using System;
using System.Threading.Tasks;
using _GameFolders.Scripts.Helpers;
using UnityEngine;

namespace _GameFolders.Scripts.Components
{
    public class Dice : MonoBehaviour
    {
        [SerializeField] private Rigidbody diceRigidbody;
        [SerializeField] private float throwForce = 10f;

        [SerializeField] private Vector3[] diceFaces =
        {
            new (180, 0, 0),
            new (-90, 0, 0),
            new (0, 0, -90),
            new (0, 0, 90),
            new (90, 0, 0),
            new (0, 0, 0)
        };

        [SerializeField] private float settleDelay = 2f;
        [SerializeField] private int selectedNumber = 1;
        [SerializeField] private float rotationLerpSpeed = 5f;
        [SerializeField] private float bounceForce = 2f;

        private bool _isRolling;
        public bool IsRolling => _isRolling;

        private bool _hasBounced;
        private bool _isLastRotate;

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
            _hasBounced = false;

            await RollDiceAsync(selectedNumber);

            _isRolling = false;
        }

        private void Update()
        {
            if (diceRigidbody && diceRigidbody.velocity.y < -7)
            {
                diceRigidbody.velocity = new Vector2(diceRigidbody.velocity.x, -7);
            }

            if (_hasBounced && diceRigidbody.velocity.y < 0)
            {
                _isLastRotate = true;
            }
        }

        private async Task RollDiceAsync(int number)
        {
            ThrowDice();

            await Task.Delay(TimeSpan.FromSeconds(settleDelay));

            while (!_isLastRotate)
            {
                await Task.Yield();
            }

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

        private void OnCollisionEnter(Collision collision)
        {
            if (!_hasBounced && collision.gameObject.CompareTag(Constants.Tags.Ground))
            {
                diceRigidbody.isKinematic = false;
                diceRigidbody.velocity = Vector3.zero;
                diceRigidbody.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
                _hasBounced = true;
            }
        }

        public void SelectNumberAndRoll(int number)
        {
            selectedNumber = number;
            RollDice();
        }
    }
}