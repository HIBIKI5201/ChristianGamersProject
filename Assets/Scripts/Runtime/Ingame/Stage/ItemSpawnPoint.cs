using ChristianGamers.Ingame.Item;
using System;
using System.Linq;
using UnityEngine;

namespace ChristianGamers
{
    public class ItemSpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private SpawnItem[] _spawnItems;

        private void Awake()
        {
            if (_spawnItems == null || _spawnItems.Length == 0)
            {
                Debug.LogWarning("Spawn items are not configured. Please check the configuration.");
                return;
            }

            //ランダムなスポーンアイテムを取得
            SpawnItem item = GetRandomSpawnItem();
            if (item.item == null) return;

            //Pivotの量だけオフセットを掛ける
            Vector3 spawnPosition = transform.TransformPoint(-item.item.SpawnPivot.localPosition);
            // アイテムをスポーンポイントの位置に生成
            Instantiate(item.item, spawnPosition, Quaternion.identity);

            Destroy(gameObject); // スポーンポイント自体は不要なので削除
        }

        /// <summary>
        ///     重さ確率でランダムなスポーンアイテムを取得する
        /// </summary>
        /// <returns></returns>
        private SpawnItem GetRandomSpawnItem()
        {
            int sumWeight = _spawnItems.Sum(e => e.spawnWeight);
            int randomWeight = UnityEngine.Random.Range(0, sumWeight);

            int currentWeight = 0;
            for (int i = 0; i < _spawnItems.Length; i++)
            {
                if (randomWeight < _spawnItems[i].spawnWeight + currentWeight)
                {
                    return _spawnItems[i];
                }

                // 現在のアイテムの重みを加算
                currentWeight += _spawnItems[i].spawnWeight;
            }

            return default;
        }

        /// <summary>
        ///     スポーンアイテムの構造体
        /// </summary>
        [Serializable]
        private struct SpawnItem
        {
            public ItemBase item;
            public int spawnWeight;
        }
    }
}
