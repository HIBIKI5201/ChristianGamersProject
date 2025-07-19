using ChristianGamers.Ingame.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChristianGamers.Ingame.Player
{
    /// <summary>
    ///     プレイヤーがアイテムを収集するためのクラス。
    /// </summary>]
    public class PlayerItemCollecter
    {
        public PlayerItemCollecter(Transform self)
        {
            _self = self;
        }

        /// <summary>
        ///     範囲内のアイテムを取得する
        /// </summary>
        /// <param name="selfPos"></param>
        /// <param name="range">回収半径</param>
        /// <param name="angleThreshold">回収範囲（度数）</param>
        /// <returns></returns>
        public ItemBase GetItem(float range, float angleThreshold)
        {
            //範囲内のオブジェクトを取得
            Collider[] hits = Physics.OverlapSphere(_self.position, range);

            ItemBase result = null;
            float minAngle = float.MinValue;
            foreach (Collider hit in hits)
            {
                //アイテム以外は無視
                if (!hit.TryGetComponent(out ItemBase item)) continue;

                // 角度を計算
                Vector3 directionToItem = hit.transform.position - _self.position;
                float angle = Vector3.Angle(_self.forward, directionToItem);

                // 角度が閾値以下かつアングルがより少ないものを収集対象にする
                if (angle <= angleThreshold && minAngle < angle)
                {
                    result = item;
                    minAngle = angle;
                }
            }

            return result;
        }

        private Transform _self;
    }
}
