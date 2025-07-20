using ChristianGamers.Ingame.Item;
using ChristianGamers.Ingame.Player;
using DG.Tweening;
using SymphonyFrameWork.System;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class ItemHotBar : MonoBehaviour
    {
        [SerializeField]
        private Image[] _slots;

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

                if (item != null)
                {
                    slot.sprite = item.IconSprite;
                    slot.color = Color.white;
                }
                else
                {
                    slot.sprite = null;
                    slot.color = Color.clear;
                }
            }
        }

        private void HandleSelectItem(int index)
        {
            if (_slots == null || _slots.Length <= index) return;
            
            _slots[_lastIndex].transform.DOScale(1, 0.2f);

            if (0 <= index) //範囲外でなければ
            {
                _slots[index].transform.DOScale(1.1f, 0.3f);
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
