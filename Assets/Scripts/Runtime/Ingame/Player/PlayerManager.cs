using ChristianGamers.Ingame.Item;
using ChristianGamers.System.Score;
using SymphonyFrameWork.System;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public event Action<int> OnSelectItem
        {
            add => _inventoryManager.OnSelectItem += value;
            remove => _inventoryManager.OnSelectItem -= value;
        }


        public event Action<float, float> OnWeightChanged
        {
            add => _inventoryManager.OnWeightChanged += value;
            remove => _inventoryManager.OnWeightChanged -= value;
        }

        public bool IsInvincible => _isInvincible;

        public float ThrowPower => _playerData.ThrowPower;
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

        [SerializeField]
        private PlayerData _playerData;

        [SerializeField, Tooltip("アイテム投げのマズルの位置を指定するためのピボット")]
        private Transform _muzzlePivot;

        [SerializeField, Tooltip("足音のオーディオソース")]
        private AudioSource _footStepAudio;

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

            if ( _footStepAudio == null )
            {
                Debug.LogWarning("foot audio is null");
            }

            Animator animator = GetComponentInChildren<Animator>();
            if (animator)
                _animController = new(animator);

            if (_rigidbody != null)
            {
                _playerController = new(transform, _rigidbody, _animController, _footStepAudio);
            }

            _playerItemCollecter = new(transform);
            _inventoryManager = new(_playerData.Strangth, _playerData.MaxItemCount);

            _isMoveActionActive = true;

            _inventoryManager.OnItemsChanged += HandleChangeInventory;
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
            _playerController.RotateYaw(_lookDir, _playerData.RotationSpeed.x);
        }

        private void FixedUpdate()
        {
            if (_isMoveActionActive)
                _playerController.Move(_moveDir, transform.forward, _playerData.MoveSpeed);
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

            ItemBase item = _playerItemCollecter.SearchItem(
                _playerData.CollectRange,
                _playerData.AngleThreshold,
                _playerData.CollectOffset);

            if (item == null) return;

            item.HadGet(_inventoryManager);
        }

        private void HandleUse(InputAction.CallbackContext context)
        {
            _inventoryManager.UseSelectedItem(this);
        }

        private void HandleSelect(InputAction.CallbackContext context)
        {
            _inventoryManager.SelectItem(context.ReadValue<float>());
        }

        private void HandleChangeInventory(IReadOnlyList<ItemBase> list)
        {
            float sum = list.Sum(e => e?.Weight ?? 0);

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

            _moveDir = Vector3.zero;
            _lookDir = Vector2.zero;
        }

        #region
        private void OnDrawGizmos()
        {
            if (_playerData == null) return;

            PlayerItemCollecter.DrawGizmos(
                transform,
                _playerData.CollectRange,
                _playerData.AngleThreshold, 
                _playerData.CollectOffset);
        }
        #endregion
    }
}
