using UnityEngine.InputSystem;
using UnityEngine;

namespace GMT.GamePlay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _minCameraScale = 5f;
        [SerializeField] private float _maxCameraScale = 40f;

        [SerializeField] private float _moveSensitivity = 1f;
        [SerializeField] private float _scaleSensitivity = 2f;

        [SerializeField]
        private Bounds _bounds = new(
            new Vector3(0, 0, 0),
             new Vector3(60, 60, 0)
        );

        private Camera _camera;
        private PlayerInput _playerInput;

        private bool _drag = false;
        private Vector2 _touchPosition = Vector2.zero;
        private Vector3 _startPosition = Vector3.zero;

        private Vector2 _firstTouchPosition = Vector2.zero;
        private Vector2 _secondTouchPosition = Vector2.zero;
        private float _startDelta = float.NaN;
        private float _startSize = 5f;

        public void Init(PlayerInput playerInput)
        {
            _playerInput = playerInput;
            _camera = GetComponent<Camera>();
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
            Drag();
            Scale();
        }

        private void Scale()
        {
            if (Touchscreen.current == null)
                return;

            int touchCount = Touchscreen.current.touches.Count;

            if (touchCount < 2)
                return;

            var firstTouch = Touchscreen.current.touches[0];
            var secondTouch = Touchscreen.current.touches[1];

            if (firstTouch.isInProgress) _firstTouchPosition = firstTouch.position.ReadValue();
            if (secondTouch.isInProgress) _secondTouchPosition = secondTouch.position.ReadValue();

            if (firstTouch.isInProgress && secondTouch.isInProgress)
            {
                float delta = Camera.main.ScreenToViewportPoint(_secondTouchPosition - _firstTouchPosition).sqrMagnitude;
                if (float.IsNaN(_startDelta))
                {
                    _startDelta = delta;
                    _startSize = _camera.orthographicSize;
                }

                _camera.orthographicSize = Mathf.Clamp(
                    _startSize - (delta - _startDelta) * (_camera.orthographicSize * _scaleSensitivity),
                    _minCameraScale,
                    _maxCameraScale
                );
            }
            else
                _startDelta = float.NaN;
        }

        private void Drag()
        {
            if (!_drag)
                return;

            var position = _playerInput.InputActions.Camera.Position.ReadValue<Vector2>();
            position = _startPosition - Camera.main.ScreenToViewportPoint(position - _touchPosition) * (_camera.orthographicSize * _moveSensitivity);

            if (position.x > _bounds.max.x) position.x = _bounds.max.x;
            if (position.x < _bounds.min.x) position.x = _bounds.min.x;
            if (position.y > _bounds.max.y) position.y = _bounds.max.y;
            if (position.y < _bounds.min.y) position.y = _bounds.min.y;

            transform.position = new Vector3(position.x, position.y, -10);
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