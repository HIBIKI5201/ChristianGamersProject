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
        ///     範囲内の最も取りやすいアイテムを取得する
        /// </summary>
        /// <param name="range">回収半径</param>
        /// <param name="angleThreshold">回収範囲（度数）</param>
        /// <param name="offset">オフセット</param>
        /// <returns></returns>
        public ItemBase SearchItem(float range, float angleThreshold, Vector3 offset = new())
        {
            Vector3 position = _self.position + offset;

            //範囲内のオブジェクトを取得
            Collider[] hits = Physics.OverlapSphere(position, range);

            ItemBase result = null;
            float minAngle = float.MinValue;
            foreach (Collider hit in hits)
            {
                //アイテムかどうかを確認
                Transform hitTransform = hit.transform;
                ItemBase item = null;
                while (!hitTransform.TryGetComponent(out item))
                {
                    if (hitTransform.parent == null)
                        break; // 親がいない場合は諦める

                    hitTransform = hitTransform.parent; // 親をたどる
                }

                if (item == null) continue; // アイテムではない

                // 角度を計算
                Vector3 directionToItem = hit.transform.position - position;
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

        #region Debug
        public static void DrawGizmos(Transform self, float collectRange, float angleThreshold, Vector3 offset = new())
        {
            Vector3 position = self.position + offset;
            Gizmos.color = new Color(1f, 1f, 0f, 0.3f);

            int segments = 24; // 円周の分割数
            float radius = Mathf.Tan(Mathf.Deg2Rad * angleThreshold) * collectRange; // コーン底面の半径

            Vector3 origin = position;
            Vector3 forward = self.forward;
            Quaternion rotation = Quaternion.LookRotation(forward);

            // コーン底面の円周点を計算して線で描く
            Vector3 prevPoint = Vector3.zero;
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * (360f / segments);
                Vector3 localPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 1) * collectRange;
                localPos = localPos.normalized * radius;
                localPos.z = collectRange;

                Vector3 worldPos = origin + rotation * localPos;

                if (i > 0)
                    Gizmos.DrawLine(prevPoint, worldPos); // 底面円の線

                Gizmos.DrawLine(origin, worldPos); // 頂点から円周への線
                prevPoint = worldPos;
            }
        }
        #endregion
    }
}
