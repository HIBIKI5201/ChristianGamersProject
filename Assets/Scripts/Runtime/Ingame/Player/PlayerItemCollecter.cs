using ChristianGamers.Ingame.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChristianGamers.Ingame.Player
{
    /// <summary>
    ///     プレイヤーがアイテムを収集するためのクラス。
    /// </summary>]
    [Serializable]
    public class PlayerItemCollecter
    {
        /// <summary>
        ///     範囲内のアイテムを取得する
        /// </summary>
        /// <param name="selfPos"></param>
        /// <param name="range">回収半径</param>
        /// <param name="angleThreshold">回収範囲（度数）</param>
        /// <returns></returns>
        public ItemBase[] GetItems(Transform selfPos, float range, float angleThreshold)
        {
            //範囲内のオブジェクトを取得
            Collider[] hits = Physics.OverlapSphere(selfPos.position, range);

            List<ItemBase> result = new List<ItemBase>();
            foreach (Collider hit in hits)
            {
                //アイテム以外は無視
                if (!hit.TryGetComponent(out ItemBase item)) continue;

                // 角度を計算
                Vector3 directionToItem = hit.transform.position - selfPos.position;
                float angle = Vector3.Angle(selfPos.forward, directionToItem);

                // 角度が閾値以下なら収集可能
                if (angle <= angleThreshold)
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }
    }
}
