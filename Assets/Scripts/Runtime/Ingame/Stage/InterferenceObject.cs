using ChristianGamers.Ingame.Player;
using SymphonyFrameWork.System;
using UnityEngine;

namespace ChristianGamers.Ingame.Stage
{
    /// <summary>
    /// 妨害オブジェクト
    /// </summary>
    public class InterferenceObject : MonoBehaviour
    {
        [SerializeField]
        private float _knockBackPower = 5;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player))
            {
                if (player.IsInvincible)
                {
                    Destroy(gameObject);
                    //壊れるAnimationの生成など
                }
                else
                {
                     Vector3 closestPoint = other.ClosestPoint(transform.position); // 近い位置を取得
                    Vector3 dir = (closestPoint - transform.position).normalized; // 方向を計算
                    player.KnockBack(dir, _knockBackPower);
                }
            }
        }
    }
}
