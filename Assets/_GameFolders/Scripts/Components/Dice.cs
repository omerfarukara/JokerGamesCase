using System.Collections;
using _GameFolders.Scripts.Helpers;
using UnityEngine;

namespace _GameFolders.Scripts.Components
{
    public class Dice : MonoBehaviour
    {
        [SerializeField] private float throwForce = 10f;

        [SerializeField] private Vector3[] diceFaces =
        {
            new(180, 0, 0),
            new(-90, 0, 0),
            new(0, 0, -90),
            new(0, 0, 90),
            new(90, 0, 0),
            new(0, 0, 0)
        };

        [SerializeField] private float settleDelay = 2f;
        [SerializeField] private int selectedNumber = 1;
        [SerializeField] private float rotationLerpSpeed = 5f;
        [SerializeField] private float bounceForce = 2f;

        private Rigidbody _diceRb;

        private bool _isRolling;
        public bool IsRolling => _isRolling;

        private bool _hasBounced;
        private bool _isLastRotate;

        private void Awake()
        {
            _diceRb = GetComponent<Rigidbody>();
        }

        private void RollDice()
        {
            if (_isRolling)
            {
                return;
            }

            _isRolling = true;
            _hasBounced = false;

            StartCoroutine(RollDiceAsync(selectedNumber));
        }

        private void Update()
        {
            if (_diceRb && _diceRb.velocity.y < -7)
            {
                _diceRb.velocity = new Vector2(_diceRb.velocity.x, -7);
            }

            if (_hasBounced && _diceRb.velocity.y < 0)
            {
                _isLastRotate = true;
            }
        }

        private IEnumerator RollDiceAsync(int number)
        {
            ThrowDice();

            yield return new WaitForSeconds(settleDelay);
            yield return new WaitUntil(() => _isLastRotate);


            _diceRb.isKinematic = true;
            yield return RotateToTargetFace(number);
            _diceRb.isKinematic = false;
            
            _isRolling = false;
        }

        private void ThrowDice()
        {
            _diceRb.isKinematic = false;
            transform.rotation = Random.rotation;

            Vector3 randomTorque = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );

            _diceRb.AddTorque(randomTorque, ForceMode.Impulse);
            _diceRb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
        }

        private IEnumerator RotateToTargetFace(int number)
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

                yield return null;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_hasBounced && collision.gameObject.CompareTag(Constants.Tags.Ground))
            {
                _diceRb.isKinematic = false;
                _diceRb.velocity = Vector3.zero;
                _diceRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
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