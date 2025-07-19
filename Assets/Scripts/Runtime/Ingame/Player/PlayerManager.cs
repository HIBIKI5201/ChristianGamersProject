using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChristianGamers.Ingame.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField, Tooltip("移動速度")]
        private float _moveSpeed = 10;
        [SerializeField, Tooltip("Y回転速度")]
        private Vector2 _rotationSpeed = new Vector2(10, 10);

        private Rigidbody _rigidbody;

        private PlayerController _playerController;

        private Vector3 _moveDir;
        private Vector2 _lookDir;

        private void Awake()
        {
            if (!TryGetComponent(out _rigidbody))
            {
                Debug.LogError("Rigidbody component is required on this GameObject.");
            }

            if (_rigidbody != null)
            {
                _playerController = new PlayerController(transform, _rigidbody);
            }
        }

        private void Start()
        {
            InputBuffer inputBuffer = ServiceLocator.GetInstance<InputBuffer>();

            if (inputBuffer != null)
            {
                inputBuffer.MoveAction.performed += HandleMove;
                inputBuffer.MoveAction.canceled += HandleMove;

                inputBuffer.LookAction.performed += HandleLook;
                inputBuffer.LookAction.canceled += HandleLook;
            }
            else
            {
                Debug.LogWarning("InputBuffer not found in ServiceLocator.");
            }
        }

        private void Update()
        {
            _playerController.RotateYaw(_lookDir, _rotationSpeed.x);
        }

        private void FixedUpdate()
        {
            _playerController.Move(_moveDir, transform.forward, _moveSpeed);
        }

        /// <summary>
        ///     入力アクションのコールバックを処理し、移動方向を更新する。
        /// </summary>
        /// <param name="context"></param>
        private void HandleMove(InputAction.CallbackContext context)
        {
            Vector2 dir = context.ReadValue<Vector2>().normalized;
            _moveDir = new Vector3(dir.x, 0, dir.y);
        }

        /// <summary>
        ///     入力アクションのコールバックを処理し、視点の方向を更新する。
        /// </summary>
        /// <param name="context"></param>
        private void HandleLook(InputAction.CallbackContext context)
        {
            _lookDir = context.ReadValue<Vector2>().normalized;
        }
    }
}
