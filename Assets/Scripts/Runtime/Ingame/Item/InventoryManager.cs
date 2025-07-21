using ChristianGamers.Ingame.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    /// <summary>
    ///     インベントリシステム
    /// </summary>
    [Serializable]
    public class InventoryManager
    {
        public event Action<IReadOnlyList<ItemBase>> OnItemsChanged;
        public event Action<float, float> OnWeightChanged;
        public event Action<int> OnSelectItem;

        public InventoryManager(float strangth, int maxItemCount)
        {
            _strangth = strangth;
            _maxItemCount = maxItemCount;

            _inventory = new ItemBase[_maxItemCount];
        }

        /// <summary>
        ///     インベントリのリストにアイテムを追加する
        /// </summary>
        /// <param name="item"取得したアイテム></param>
        public bool AddItem(ItemBase item)
        {
            ItemBase[] items = _inventory.Where(x => x != null).ToArray();

            //アイテムの所持数が最大値以上なら追加不可
            if (_maxItemCount <= items.Length) return false;

            float sumWeight = SumInventoryWeight() + item.Weight;
            float strangth = GetFinalStrangth();

            // アイテムの重さが最大重量を超える場合は追加しない
            if (strangth < sumWeight) return false;

            //空いている場所にアイテムを追加
            int index = Array.IndexOf(_inventory, null);
            _inventory[index] = item;
            OnItemsChanged?.Invoke(_inventory);

            OnWeightChanged?.Invoke(strangth, sumWeight);

            //インベントリに一つ目の物が入ったら
            if (_inventory.Count(e => e != null) == 1) 
            {
                _selectIndex = index;
            }

            return true;
        }

        /// <summary>
        ///     インベントリのリストからアイテムを削除する
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(ItemBase item)
        {
            //アイテムをリストから削除
            int index = Array.IndexOf(_inventory, item);
            _inventory[index] = null;
            OnItemsChanged?.Invoke(_inventory);
            _selectIndex = FindNearItemIndex(_selectIndex);
            OnSelectItem?.Invoke(_selectIndex);

            //現在の合計値をイベント発行する
            OnWeightChanged?.Invoke(GetFinalStrangth(), SumInventoryWeight());
        }

        /// <summary>
        ///     アイテムの選択
        /// </summary>
        public void SelectItem(float axis)
        {
            if (_inventory.Length <= 1) return; //一つ以下なら選択できない

            int value = (int)Mathf.Sign(axis);
            _selectIndex = GetNextItemIndex(_selectIndex, (int)axis);
            OnSelectItem?.Invoke(_selectIndex);

            Debug.Log($"index : {_selectIndex}");
        }

        public void UseSelectedItem(PlayerManager player)
        {
            if (_inventory.Length <= 0)
            {
                Debug.LogWarning("No items to use.");
                return;
            }

            ItemBase selectedItem = GetSelectedItem();
            if (selectedItem is IUseble usebleItem)
            {
                if (usebleItem.Use(player))
                {
                    RemoveItem(selectedItem);
                }
            }
        }

        /// <summary>
        ///     選択されているアイテムを取得する
        /// </summary>
        /// <returns></returns>
        public ItemBase GetSelectedItem() =>
            (0 < _inventory.Length && 0 <= _selectIndex) ? _inventory[_selectIndex] : null;

        /// <summary>
        ///     全ての回収可能なアイテムを返す
        /// </summary>
        /// <returns></returns>
        public IWithdrawable[] GetWithdrawalItems()
        {
            if (_inventory.Length <= 0) return Array.Empty<IWithdrawable>();

            return _inventory
                .Select(item => item as IWithdrawable)
                .Where(item => item != null)
                .ToArray();
        }

        public void AddStrangthBuff(Func<float, float> buff)
        {
            _weightBuff.Add(buff);
            OnWeightChanged?.Invoke(GetFinalStrangth(), SumInventoryWeight());
        }
        public void RemoveStrangthBuff(Func<float, float> buff)
        {
            RemoveStrangthBuff(buff);
            OnWeightChanged?.Invoke(GetFinalStrangth(), SumInventoryWeight());
        }

        private ItemBase[] _inventory;
        private int _selectIndex = 0;
        private float _strangth;
        private int _maxItemCount;
        private List<Func<float, float>> _weightBuff = new();

        /// <summary>
        ///     最終的な力の量を返す
        /// </summary>
        /// <returns></returns>
        private float GetFinalStrangth()
        {
            float maxStrangth = _strangth;

            //バフを適用
            foreach (var buff in _weightBuff)
            {
                if (buff != null) continue;
                maxStrangth = buff.Invoke(maxStrangth);
            }
            return maxStrangth;
        }

        /// <summary>
        ///     インデックスに近いアイテムの位置を返す
        ///     同じ距離なら右側を優先する
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private int FindNearItemIndex(int origin)
        {
            //左右に探索する
            int rightIndex = GetNextItemIndex(origin, 1);
            int leftIndex = GetNextItemIndex(origin, -1);

            //探索結果が該当なしなら終了
            if (rightIndex < 0 || leftIndex < 0)
            {
                return -1;
            }

            //近い方を返す
            return (Mathf.Abs(origin - rightIndex) <= Mathf.Abs(origin - leftIndex))
                ? rightIndex : leftIndex;
        }

        /// <summary>
        ///     方向の最も近いアイテムのインデックスを返す
        ///     見つからなければ元のインデックスを返す
        ///     ただし元のインデックスもnullなら-1を返す
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private int GetNextItemIndex(int origin, int dir)
        {
            dir = (int)Mathf.Sign(dir);

            for (int i = 1; i < _inventory.Length; i++)//自分を除くので1から開始
            {
                int index = origin + i * dir;
                index = (index + _inventory.Length) % _inventory.Length;

                if (_inventory[index] != null) return index;
            }

            //オリジンがnullじゃないならoriginを返す
            return (0 <= origin && _inventory[origin] != null)
                ? origin : -1;
        }

        /// <summary>
        ///     インベントリ内の合計重量を返す
        /// </summary>
        /// <returns></returns>
        private float SumInventoryWeight() =>
            _inventory.Sum(i => i?.Weight ?? 0);
    }
}
