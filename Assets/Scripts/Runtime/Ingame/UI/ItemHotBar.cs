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
        private Image[] _slots;

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
                Image slot = _slots[i];
                if (!slot.transform.GetChild(0)
                    .TryGetComponent(out Image child))
                    continue;

                if (item != null)
                {
                    child.sprite = item.IconSprite;
                    child.color = Color.white;
                }
                else
                {
                    child.sprite = null;
                    child.color = Color.clear;
                }
            }
        }

        private void HandleSelectItem(int index)
        {
            if (_slots == null || _slots.Length <= index) return;

            Image lastSlot = _slots[_lastIndex];
            Image nextSlot = _slots[index];

            if (!lastSlot.transform.GetChild(0)
                    .TryGetComponent(out Image lastChild))
                return;
            if (!nextSlot.transform.GetChild(0)
                   .TryGetComponent(out Image nextChild))
                return;

            lastSlot.transform.DOScale(1, 0.2f);
            lastSlot.sprite = _normalSlotSprite;

            if (0 <= index) //範囲外でなければ
            {
                nextSlot.transform.DOScale(1.1f, 0.3f);
                nextChild.sprite = _selectedSlotSprite;

                _lastIndex = index;
            }
        }

        /// <summary>
        ///     スロットを初期化する
        /// </summary>
        private void ClearSlot()
        {
            foreach (Image slot in _slots)
            {
                slot.sprite = null;
                slot.color = Color.clear;
            }
        }
    }
}
