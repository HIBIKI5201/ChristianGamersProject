using ChristianGamers.Ingame.Item;
using ChristianGamers.System.Score;
using SymphonyFrameWork.System;
using System;
using System.Collections.Generic;
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
        public event Action<IReadOnlyList<ItemBase>> OnItemsChanged
        {
            add => _inventoryManager.OnItemsChanged += value;
            remove => _inventoryManager.OnItemsChanged -= value;
        }

        public event Action<float, float> OnWeightChanged
        {
            add => _inventoryManager.OnWeightChanged += value;
            remove => _inventoryManager.OnWeightChanged -= value;
        }

        public bool IsInvincible => _isInvincible;

        public float ThrowPower => _throwPower;
        public Transform MuzzlePivot => _muzzlePivot;

        /// <summary>
        ///     無敵状態を設定
        /// </summary>
        /// <param name="active"></param>
        public void SetInvincible(bool active) => _isInvincible = active;

        /// <summary>
        ///     ノックバックする
        /// </summary>
        public async void KnockBack(Vector3 power, float StunTime)
        {
            if (_rigidbody == null)
            {
                Debug.LogError("Rigidbody is not assigned.");
                return;
            }

            Debug.Log("NockBack");

            _isMoveActionActive = false;

            _rigidbody.linearVelocity = Vector3.zero; // 既存の速度をリセット
            _rigidbody.AddForce(power, ForceMode.Impulse);
            _animController.SetMoveDirParam(Vector2.zero);

            try
            {
                await Awaitable.WaitForSecondsAsync(StunTime, destroyCancellationToken);
            }
            catch (OperationCanceledException) { }
            finally
            {
                _isMoveActionActive = true;
            }
        }

        public void RegisterSpeedBuff(Func<float, float> func) => _playerController.RegisterSpeedBuff(func);
        public void UnregisterSpeedBuff(Func<float, float> func) => _playerController.UnregisterSpeedBuff(func);

        public void RegisterStrangthBuff(Func<float, float> func) => _inventoryManager.AddStrangthBuff(func);
        public void UnregisterStrangthBuff(Func<float, float> func) => _inventoryManager.RemoveStrangthBuff(func);

        public void Withdrawal()
        {
            ScoreManager scoreManager = ServiceLocator.GetInstance<ScoreManager>();

            if (scoreManager == null)
            {
                Debug.LogWarning("ScoreManager is not found in the ServiceLocator.");
                return;
            }

            // 取得したアイテムのスコアを計算し、スコアマネージャーに追加する
            IWithdrawable[] withdrawables = _inventoryManager.GetWithdrawalItems();

            // 取得したアイテムをインベントリから削除する
            foreach (IWithdrawable withdrawable in withdrawables)
            {
                scoreManager.AddScore(withdrawable.WithdrawScore);
                if (withdrawable is ItemBase item)
                {
                    _inventoryManager.RemoveItem(item);
                }
            }
        }

        public void SetActiveInputHandle(bool active)
        {
            if (_inputBuffer == null)
            {
                Debug.LogWarning("input buffer is null");
                return;
            }

            if (active)
            {
                RegisterInputActionHandle(_inputBuffer);
            }
            else
            {
                UnregisterInputActionHandle(_inputBuffer);
            }
        }

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
        [SerializeField, Tooltip("アイテム収集範囲のオフセット")]
        private Vector3 _collectOffset = new Vector3(0, 0.5f, 0);

        [SerializeField, Tooltip("インベントリの最大重量")]
        private float _strangth = 10.0f;
        [SerializeField, Tooltip("インベントリの最大所持数")]
        private int _maxItemCount = 7;

        [SerializeField, Tooltip("アイテム投げのマズルの位置を指定するためのピボット")]
        private Transform _muzzlePivot;
        [SerializeField, Tooltip("投げる力の大きさ")]
        private float _throwPower = 10.0f;

        private Rigidbody _rigidbody;
        private InputBuffer _inputBuffer;

        private PlayerController _playerController;
        private PlayerItemCollecter _playerItemCollecter;
        private InventoryManager _inventoryManager;
        private PlayerAnimationController _animController;

        private Vector3 _moveDir;
        private Vector2 _lookDir;

        private bool _isInvincible;
        private bool _isMoveActionActive;

        private void Awake()
        {
            if (!TryGetComponent(out _rigidbody))
            {
                Debug.LogError("Rigidbody component is required on this GameObject.");
            }

            Animator animator = GetComponentInChildren<Animator>();
            if (animator)
                _animController = new(animator);

            if (_rigidbody != null)
            {
                _playerController = new(transform, _rigidbody, _animController);
            }

            _playerItemCollecter = new(transform);
            _inventoryManager = new(_strangth, _maxItemCount);

            _isMoveActionActive = true;
        }

        private void Start()
        {
            _inputBuffer = ServiceLocator.GetInstance<InputBuffer>();
            if (_inputBuffer == null)
            {
                Debug.LogError("InputBuffer is not found in the ServiceLocator.");
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            _playerController.RotateYaw(_lookDir, _rotationSpeed.x);
        }

        private void FixedUpdate()
        {
            if (_isMoveActionActive)
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

            ItemBase item = _playerItemCollecter.SearchItem(_collectRange, _angleThreshold, _collectOffset);

            if (item == null) return;

            item.HadGet(_inventoryManager);
        }

        private void HandleUse(InputAction.CallbackContext context)
        {
            ItemBase item = _inventoryManager.GetSelectedItem();

            if (item is IUseble usable) //Usableを継承していたら実行
            {
                _inventoryManager.UseSelectedItem(this);
            }
        }

        private void HandleSelect(InputAction.CallbackContext context)
        {
            _inventoryManager.SelectItem(context.ReadValue<float>());
        }

        /// <summary>
        ///     入力アクションのハンドルを登録する。
        /// </summary>
        /// <param name="inputBuffer"></param>
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

            inputBuffer.UseAction.started += HandleUse;

            inputBuffer.SelectAction.performed += HandleSelect;
        }

        private void UnregisterInputActionHandle(InputBuffer inputBuffer)
        {
            if (inputBuffer == null)
            {
                Debug.LogError("InputBuffer is null.");
                return;
            }

            inputBuffer.MoveAction.performed -= HandleMove;
            inputBuffer.MoveAction.canceled -= HandleMove;

            inputBuffer.LookAction.performed -= HandleLook;
            inputBuffer.LookAction.canceled -= HandleLook;

            inputBuffer.CollectAction.started -= HandleCollect;

            inputBuffer.UseAction.started -= HandleUse;

            inputBuffer.SelectAction.performed -= HandleSelect;
        }

        #region
        private void OnDrawGizmos()
        {
            PlayerItemCollecter.DrawGizmos(transform, _collectRange, _angleThreshold, _collectOffset);
        }
        #endregion
    }
}
