using ChristianGamers.Ingame.Player;
using ChristianGamers.Utility;
using System;
using UnityEngine;

namespace ChristianGamers.Ingame.Stage
{
    /// <summary>
    ///     アイテムの回収ポイント
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class WithdrawalPoint : MonoBehaviour
    {
        public event Action OnPlayerWithdrawal; // プレイヤーが回収ポイントに入ったときのイベント
        private void Awake()
        {
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = true; // Rigidbodyをキネマティックに設定
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerManager player = TransformUtility.FindTypeByParents<PlayerManager>(other.transform);

            if (player != null)
            {
                Debug.Log("Player has entered the withdrawal point.");
                player.Withdrawal();
                OnPlayerWithdrawal?.Invoke();
            }
        }
    }
}
