using UnityEngine;
using UnityEngine.InputSystem;

namespace ChristianGamers
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputBuffer : MonoBehaviour
    {
        public InputAction MoveAction => _moveAction;
        public InputAction LookAction => _lookAction;

        [SerializeField]
        private string _moveActionName = "Move";
        [SerializeField]
        private string _lookActionName = "Look";

        private PlayerInput _playerInput;

        private InputAction _moveAction;
        private InputAction _lookAction;
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
        }
    }
}
