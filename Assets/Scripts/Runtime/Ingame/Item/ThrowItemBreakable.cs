using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    [RequireComponent(typeof(Rigidbody))]
    public class ThrowItemBreakable : MonoBehaviour
    {
        private const float DESTROY_DELAY = 5;

        [SerializeField]
        private float _reactionPower = 1;

        public void Breaked(Vector3 dir)
        {
            if (!TryGetComponent(out Rigidbody rb)) return;

            dir.Normalize();

            rb.excludeLayers = ~0; //全てのコライダーから当たらなくする
            rb.AddForce(dir * _reactionPower, ForceMode.Impulse);

            Destroy(gameObject, DESTROY_DELAY);
        }
    }
}
