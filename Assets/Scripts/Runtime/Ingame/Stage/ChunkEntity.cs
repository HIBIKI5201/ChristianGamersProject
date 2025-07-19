using UnityEngine;

namespace ChristianGamers.Ingame.Stage
{
    /// <summary>
    ///     チャンクのエンティティを表すクラス
    /// </summary>
    public class ChunkEntity : MonoBehaviour
    {
        public void Initialize()
        {
            Debug.Log($"chunk{gameObject.name} has been initialized.");
        }
    }
}
