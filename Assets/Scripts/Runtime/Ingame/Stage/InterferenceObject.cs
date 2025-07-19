using ChristianGamers.Ingame.Player;
using ChristianGamers.Utility;
using SymphonyFrameWork.System;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;

namespace ChristianGamers.Ingame.Stage
{
    /// <summary>
    /// 妨害オブジェクト
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class InterferenceObject : MonoBehaviour
    {
        [SerializeField]
        private float _knockBackPower = 5;
        [SerializeField]
        private float _stunTime = 1f; // ノックバック中のスタン時間

        private void OnTriggerEnter(Collider other)
        {
            PlayerManager player = TransformUtility.FindTypeByParents<PlayerManager>(other.transform);

            if (player != null)
            {
                if (player.IsInvincible)
                {
                    Destroy(gameObject);
                    //壊れるAnimationの生成など
                }
                else
                {
                    Vector3 dir = (other.transform.position - transform.position).normalized; // 方向を計算
                    player.KnockBack(dir * _knockBackPower, _stunTime);
                }
            }
        }
    }
}
