using ChristianGamers.Ingame.Player;
using ChristianGamers.Utility;
using UnityEngine;

namespace ChristianGamers.Ingame.Stage
{
    /// <summary>
    ///     アイテムの回収ポイント
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class WithdrawalPoint : MonoBehaviour
    {
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
            }
        }
    }
}
