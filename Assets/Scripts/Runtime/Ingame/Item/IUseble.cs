using ChristianGamers.Ingame.Player;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    /// <summary>
    /// アイテムが使われる
    /// </summary>
    interface IUseble
    {
        /// <summary>
        /// 使用時の動き
        /// </summary>
        public bool Use(PlayerManager player);
    }
}
