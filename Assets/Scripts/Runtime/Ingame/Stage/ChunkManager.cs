using UnityEngine;

namespace ChristianGamers.Ingame.Stage
{
    /// <summary>
    ///     チャンクの管理を行うクラス
    /// </summary>
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField, Tooltip("チャンクの位置")]
        private Transform[] _chunkPositions;

        [SerializeField, Tooltip("チャンクのサイズ")]
        private float _chunkSize = 10f;

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
    }
}
