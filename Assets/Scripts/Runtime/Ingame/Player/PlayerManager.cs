using SymphonyFrameWork.System;
using Unity.AppUI.Core;
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

        private Vector3 _moveDir;
        private Vector2 _lookDir;

        private void Awake()
        {
            if (!TryGetComponent(out _rigidbody))
            {
                Debug.LogError("Rigidbody component is required on this GameObject.");
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
            RotateYaw(_lookDir, _rotationSpeed.x);
        }

        private void FixedUpdate()
        {
            Move(_moveDir, transform.forward, _moveSpeed);
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

        private void HandleLook(InputAction.CallbackContext context)
        {
            _lookDir = context.ReadValue<Vector2>().normalized;
        }

        /// <summary>
        ///     入力の方向に応じてプレイヤーを移動させる。
        /// </summary>
        private void Move(Vector3 dir, Vector3 forward, float speed)
        {
            // forward（前）と right（右）を計算
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            // dir をローカル座標系（right/forward）に変換
            Vector3 moveDir = (right * dir.x + forward.normalized * dir.z).normalized;

            _rigidbody.AddForce(moveDir * speed, ForceMode.Force);
        }

        private void RotateYaw(Vector2 dir, float rotationSpeed)
        {
            float turnAmount = dir.x * rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, turnAmount, 0f);
        }
    }
}
