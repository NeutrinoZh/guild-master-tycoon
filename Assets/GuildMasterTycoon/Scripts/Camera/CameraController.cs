using UnityEngine;
using UnityEngine.InputSystem;

namespace GMT.GamePlay
{
    public class CameraController : MonoBehaviour
    {
        private PlayerInput _playerInput;

        private bool _drag = false;
        private Vector2 _touchPosition = Vector2.zero;
        private Vector3 _startPosition = Vector3.zero;

        [SerializeField] private float _sensitivity = 5;

        public void Init(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        private void Start()
        {
            _playerInput.InputActions.Camera.Touch.performed += EnableDrag;
            _playerInput.InputActions.Camera.Touch.canceled += DisableDrag;
        }

        private void OnDisable()
        {
            _playerInput.InputActions.Camera.Touch.performed -= EnableDrag;
            _playerInput.InputActions.Camera.Touch.canceled -= DisableDrag;
        }

        private void Update()
        {
            if (!_drag)
                return;

            var position = _playerInput.InputActions.Camera.Position.ReadValue<Vector2>();
            transform.position = _startPosition - Camera.main.ScreenToViewportPoint(position - _touchPosition) * _sensitivity;
        }

        private void DisableDrag(InputAction.CallbackContext _) => _drag = false;
        private void EnableDrag(InputAction.CallbackContext ctx)
        {
            _drag = true;
            _touchPosition = _playerInput.InputActions.Camera.Position.ReadValue<Vector2>();
            _startPosition = transform.position;
        }
    }
}