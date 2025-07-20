using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    public class CollectItem : ItemBase, IWithdrawable
    {
        public int WithdrawScore => _withdrawScore;

        [SerializeField]
        private int _withdrawScore = 10;
    }
}
