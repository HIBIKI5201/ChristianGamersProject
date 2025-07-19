using System;
using UnityEngine;

namespace ChristianGamers.Ingame.Player
{
    [Serializable]
    public class PlayerController
    {
        private Transform _self;
        private Rigidbody _rigidbody;

        public PlayerController(Transform self, Rigidbody rigidbody)
        {
            _self = self;
            _rigidbody = rigidbody;
        }

        /// <summary>
        ///     入力の方向に応じてプレイヤーを移動させる。
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="forward"></param>
        /// <param name="speed"></param>
        public void Move(Vector3 dir, Vector3 forward, float speed)
        {
            // forward（前）と right（右）を計算
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            // dir をローカル座標系（right/forward）に変換
            Vector3 moveDir = (right * dir.x + forward.normalized * dir.z).normalized;

            _rigidbody.AddForce(moveDir * speed, ForceMode.Force);
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
    }
}
