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
        public void Use();
    }
}
