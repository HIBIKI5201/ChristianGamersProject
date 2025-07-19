using UnityEngine;
using UnityEngine.InputSystem;

namespace ChristianGamers
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputBuffer : MonoBehaviour
    {
        public InputAction MoveAction => _moveAction;
        public InputAction LookAction => _lookAction;
        public InputAction CollectAction => _collectAction;

        [SerializeField]
        private string _moveActionName = "Move";
        [SerializeField]
        private string _lookActionName = "Look";
        [SerializeField]
        private string _collectActionName = "Collect";

        private PlayerInput _playerInput;

        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _collectAction;
        private void Awake()
        {
            if (!TryGetComponent(out _playerInput))
            {
                Debug.LogError("PlayerInput component is required on this GameObject.");
            }
        }

        private void OnEnable()
        {
            _moveAction = _playerInput.actions[_moveActionName];
            _lookAction = _playerInput.actions[_lookActionName];
            _collectAction = _playerInput.actions[_collectActionName];
        }
    }
}
