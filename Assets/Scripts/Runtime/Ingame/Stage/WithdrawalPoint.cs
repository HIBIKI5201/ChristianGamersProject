using ChristianGamers.Ingame.Player;
using ChristianGamers.Utility;
using UnityEngine;

namespace ChristianGamers.Ingame.Stage
{
    /// <summary>
    ///     アイテムの回収ポイント
    /// </summary>
    public class WithdrawalPoint : MonoBehaviour
    {
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
