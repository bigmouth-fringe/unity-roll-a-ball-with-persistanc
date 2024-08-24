using Persistence;
using Shared;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main
{
    public class Player : MonoBehaviour, IPersisted
    {
        public float speed;
        public TextMeshProUGUI collectedCountText;

        private Rigidbody _rigidBody;
        private Rigidbody Rigidbody => _rigidBody ??= GetComponent<Rigidbody>();

        private float _movementX;
        private float _movementY;

        private int _collectedCount;

        // Start is called before the first frame update
        private void Start()
        {
            SetCountText();
        }

        private void OnMove(InputValue movementValue)
        {
            var movement = movementValue.Get<Vector2>();
            _movementX = movement.x;
            _movementY = movement.y;
        }

        private void FixedUpdate()
        {
            var movement = new Vector3(_movementX, 0.0f, _movementY);
            Rigidbody.AddForce(movement * speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Collectable))
            {
                other.gameObject.SetActive(false);
                _collectedCount++;
                SetCountText();
            }
        }

        public void Load(GameState state)
        {
            Rigidbody.position = state.playerPosition;
            _collectedCount = state.collectedCount;
            SetCountText();
        }

        public void Save(ref GameState state)
        {
            state.playerPosition = Rigidbody.position;
            state.collectedCount = _collectedCount;
        }

        private void SetCountText() => collectedCountText.text = $"Count: {_collectedCount}";
    }
}
