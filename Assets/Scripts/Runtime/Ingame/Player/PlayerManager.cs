using ChristianGamers.Ingame.Item;
using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChristianGamers.Ingame.Player
{
    /// <summary>
    ///     プレイヤーの管理を行うクラス。
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerManager : MonoBehaviour
    {
        public bool IsInvincible => _isInvincible;

        public float ThrowPower => _throwPower;
        public Transform MuzzlePivot => _muzzlePivot;

        public void SetInvincible(bool active) => _isInvincible = active;

        [Header("移動系設定")]
        [SerializeField, Tooltip("移動速度")]
        private float _moveSpeed = 10;
        [SerializeField, Tooltip("Y回転速度")]
        private Vector2 _rotationSpeed = new Vector2(10, 10);

        [Header("アイテム収集系設定")]
        [SerializeField, Min(0), Tooltip("アイテムを収集する範囲")]
        private float _collectRange = 2.0f;

        [SerializeField, Range(0, 360), Tooltip("アイテムを収集するための角度の閾値（度数法）")]
        private float _angleThreshold = 45.0f;

        [SerializeField, Tooltip("アイテム投げのマズルの位置を指定するためのピボット")]
        private Transform _muzzlePivot;
        [SerializeField, Tooltip("投げる力の大きさ")]
        private float _throwPower = 10.0f;

        private Rigidbody _rigidbody;

        private PlayerController _playerController;
        private PlayerItemCollecter _playerItemCollecter;
        private InventoryManager _inventoryManager;

        private Vector3 _moveDir;
        private Vector2 _lookDir;

        private bool _isInvincible;

        private void Awake()
        {
            if (!TryGetComponent(out _rigidbody))
            {
                Debug.LogError("Rigidbody component is required on this GameObject.");
            }

            if (_rigidbody != null)
            {
                _playerController = new(transform, _rigidbody);
            }

            _playerItemCollecter = new(transform);
        }

        private void Start()
        {
            InputBuffer inputBuffer = ServiceLocator.GetInstance<InputBuffer>();
            RegisterInputActionHandle(inputBuffer);
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

        private void HandleCollect(InputAction.CallbackContext context)
        {
            // アイテム収集の処理をここに実装
            // 例えば、周囲のアイテムを検出し、収集するロジックを追加する
            Debug.Log("Collect action triggered.");

            ItemBase item = _playerItemCollecter.GetItem(_collectRange, _angleThreshold);

            if (item == null) return;

            item.HadGet(_inventoryManager);

        }

        private void RegisterInputActionHandle(InputBuffer inputBuffer)
        {
            if (inputBuffer == null)
            {
                Debug.LogError("InputBuffer is null.");
                return;
            }

            inputBuffer.MoveAction.performed += HandleMove;
            inputBuffer.MoveAction.canceled += HandleMove;

            inputBuffer.LookAction.performed += HandleLook;
            inputBuffer.LookAction.canceled += HandleLook;

            inputBuffer.CollectAction.started += HandleCollect;
        }
    }
}
