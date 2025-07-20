using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChristianGamers.Ingame.Player
{
    /// <summary>
    ///     プレイヤーの移動と回転を制御するクラス。
    /// </summary>
    [Serializable]
    public class PlayerController
    {
        public PlayerController(Transform self, Rigidbody rigidbody, PlayerAnimationController animator)
        {
            _self = self;
            _rigidbody = rigidbody;
            _animator = animator;
        }

        /// <summary>
        ///     入力の方向に応じてプレイヤーを移動させる。
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="forward"></param>
        /// <param name="speed"></param>
        public void Move(Vector3 dir, Vector3 forward, float speed)
        {
            // 前から右を計算
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            // dir をローカル系からワールド系に変換
            Vector3 moveDir = (right * dir.x + forward.normalized * dir.z).normalized;

            //バフによる影響を計算
            foreach(Func<float, float> buff in _speedBuffs)
            {
                speed = buff(speed);
            }

            _rigidbody.linearVelocity =
                new Vector3(moveDir.x * speed, _rigidbody.linearVelocity.y, moveDir.z * speed);

            _animator?.SetMoveDirParam(new Vector2(dir.x, dir.z));
        }

        /// <summary>
        ///     入力の方向に応じてプレイヤーをY軸回転させる。
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="rotationSpeed"></param>
        public void RotateYaw(Vector2 dir, float rotationSpeed)
        {
            float turnAmount = dir.x * rotationSpeed * Time.deltaTime;
            _self.Rotate(0f, turnAmount, 0f);
        }

        public void RegisterSpeedBuff(Func<float, float> func) => _speedBuffs.Add(func);
        public void UnregisterSpeedBuff(Func<float, float> func) => _speedBuffs.Remove(func);

        private Transform _self;
        private Rigidbody _rigidbody;
        private PlayerAnimationController _animator;

        private List<Func<float, float>> _speedBuffs = new();
    }
}
