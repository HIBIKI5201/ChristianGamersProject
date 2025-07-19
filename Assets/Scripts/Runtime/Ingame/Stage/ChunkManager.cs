using ChristianGamers.Utility;
using System.Linq;
using UnityEngine;

namespace ChristianGamers.Ingame.Stage
{
    /// <summary>
    ///     チャンクの管理を行うクラス
    /// </summary>
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField, Tooltip("チャンクのエンティティ")]
        private ChunkEntity[] _chunkEntities;

        [Header("チャンク位置の設定")]
        [SerializeField, Tooltip("チャンクの位置")]
        private Transform[] _chunkPositions;

        [SerializeField, Tooltip("チャンクのサイズ")]
        private float _chunkSize = 10f;

        private void Awake()
        {
            ShuffleHelper.FisherYatesShuffle(_chunkPositions);
            GenerateChunkEntity(_chunkEntities);
        }

        private void GenerateChunkEntity(ChunkEntity[] chunkEntities)
        {
            // チャンクのエンティティを生成
            for (int i = 0; i < _chunkPositions.Length; i++)
            {
                int index = i % chunkEntities.Length; // チャンクのエンティティをループさせる

                if (chunkEntities[index] == null) continue;

                //Positionの位置にチャンクのエンティティを生成
                var chunkEntity = Instantiate(chunkEntities[index],
                    _chunkPositions[i].position,
                    Quaternion.identity);
                chunkEntity.Initialize();
            }
        }

        #region Debug
        private void OnValidate()
        {
            _chunkPositions = _chunkPositions.Where(_chunkPositions => _chunkPositions != null).ToArray();
        }

        private void OnDrawGizmos()
        {
            // チャンクの境界を可視化するためのGizmosを描画
            Gizmos.color = Color.green;
            foreach (var chunk in _chunkPositions)
            {
                if (chunk == null) continue;

                Gizmos.DrawWireCube(chunk.position, new Vector3(_chunkSize, 0, _chunkSize));
            }
        }
        #endregion
    }
}
