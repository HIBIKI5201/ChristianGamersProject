using ChristianGamers.Ingame.Item;
using System;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
            Vector3 spawnPosition = transform.TransformPoint(item.offSet);
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

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        private float _switchingTime = 0.5f;

        private void OnDrawGizmos()
        {
            if (_spawnItems == null || _spawnItems.Length == 0) return;

            // エディタ上でのデバッグ用に、一定時間ごとにスポーンアイテムを切り替える
            int index = (int)(EditorApplication.timeSinceStartup / _switchingTime) % _spawnItems.Length;

            SpawnItem spawnItem = _spawnItems[index];
            if (spawnItem.item == null) return;

            Gizmos.color = Color.green;
            foreach (MeshFilter mf in spawnItem.item.GetComponentsInChildren<MeshFilter>())
            {
                if (mf == null) continue;
                Gizmos.DrawWireMesh(
                    mf.sharedMesh,
                    transform.position
                    + mf.transform.localPosition
                    + spawnItem.offSet,
                    Quaternion.identity,
                    mf.transform.localScale);
            }
        }
#endif
    /// <summary>
    ///     スポーンアイテムの構造体
    /// </summary>
    [Serializable]
        private struct SpawnItem
        {
            public ItemBase item;
            public int spawnWeight;
            public Vector3 offSet;
        }
    }
}
