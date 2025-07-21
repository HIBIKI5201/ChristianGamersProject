using ChristianGamers.Ingame.Item;
using ChristianGamers.Ingame.Player;
using DG.Tweening;
using SymphonyFrameWork.System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class ItemHotBar : MonoBehaviour
    {
        [SerializeField]
        private ItemHotBarSlot[] _slots;

        [SerializeField]
        private Sprite _normalSlotSprite;
        [SerializeField]
        private Sprite _selectedSlotSprite;

        private int _lastIndex;

        private void Start()
        {
            PlayerManager player = ServiceLocator.GetInstance<PlayerManager>();
            player.OnItemsChanged += HandleItemChange;
            player.OnSelectItem += HandleSelectItem;

            ClearSlot();
        }

        private void HandleItemChange(IReadOnlyList<ItemBase> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ItemBase item = list[i];
                Image slot = _slots[i].Self;
                Image child = _slots[i].Child;
                Text price = _slots[i].Price;

                if (item != null)
                {
                    child.sprite = item.IconSprite;
                    child.color = Color.white;

                    //値段があるならテキスト表示
                    if (item is IWithdrawable withdrawable)
                    {
                        price.text = withdrawable.WithdrawScore.ToString("0");
                    }
                    else
                    {
                        price.text = string.Empty;
                    }
                }
                else
                {
                    child.sprite = null;
                    child.color = Color.clear;
                    price.text = string.Empty;
                }
            }
        }

        private void HandleSelectItem(int index)
        {
            if (_slots == null || _slots.Length <= index) return;

            Image lastSlot = _slots[_lastIndex].Self;
            Image lastChild = _slots[_lastIndex].Child;


            lastChild.transform.DOScale(1, 0.2f);
            lastSlot.sprite = _normalSlotSprite;

            if (0 <= index) //範囲外でなければ
            {
                Image nextSlot = _slots[index].Self;
                Image nextChild = _slots[index].Child;  

                    nextChild.transform.DOScale(1.1f, 0.3f);
                nextSlot.sprite = _selectedSlotSprite;

                _lastIndex = index;
            }
        }

        /// <summary>
        ///     スロットを初期化する
        /// </summary>
        private void ClearSlot()
        {
            foreach (ItemHotBarSlot slot in _slots)
            {
                slot.Self.sprite = _normalSlotSprite;
                slot.Child.sprite = null;
                slot.Child.color = Color.clear;
                slot.Price.text = string.Empty;
            }
        }
    }
}
