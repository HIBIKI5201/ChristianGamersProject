using UnityEngine;

namespace ChristianGamers
{
    /// <summary>
    ///      回収可能なアイテムのインターフェース
    /// </summary>
    public interface IWithdrawable
    {
        public int WithdrawScore { get; }
    }
}
