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
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player))
            {
                if (player.IsInvincible)
                {
                    Destroy(gameObject);
                    //壊れるAnimationの生成など
                }
            }
        }
    }
}
