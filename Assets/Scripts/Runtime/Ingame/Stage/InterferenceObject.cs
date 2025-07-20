using ChristianGamers.Ingame.Player;
using ChristianGamers.Utility;
using Action = System.Action;
using Unity.Behavior;
using UnityEngine;
using System;
using UnityEngine.AI;
using Unity.VisualScripting;

namespace ChristianGamers.Ingame.Stage
{
    /// <summary>
    /// 妨害オブジェクト
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class InterferenceObject : MonoBehaviour
    {
        public event Action OnRespawn;
        public event Action OnDead;

        [SerializeField, Tooltip("ノックバックの吹っ飛び力")]
        private float _knockBackPower = 5;
        [SerializeField, Tooltip("プレイヤーをスタンさせる時間")]
        private float _stunTime = 1f; // ノックバック中のスタン時間

        [SerializeField, Tooltip("リスポーンするまでの時間")]
        private float _respawnTime;

        [SerializeField, Tooltip("Agentの")]
        private string _BBPatrolPoints = "Patrol Points";

        private void OnTriggerEnter(Collider other)
        {
            PlayerManager player = TransformUtility.FindTypeByParents<PlayerManager>(other.transform);

            if (player != null)
            {
                if (player.IsInvincible)
                {
                    if (!TryGetComponent(out BehaviorGraphAgent agent)) return;
                    if (!TryGetComponent(out Rigidbody rb)) return;
                    if (!TryGetComponent(out NavMeshAgent nvAgent)) return;

                    //プレイヤーに当たらないように変更
                    rb.excludeLayers = ~LayerMask.NameToLayer(LayersEnum.Player.ToString());
                    agent.End();
                    nvAgent.enabled = false;

                    Dead();

                    RespawnWithDelay(_respawnTime, () =>
                    {
                        if (agent.BlackboardReference
                        .GetVariable<GameObject[]>(_BBPatrolPoints, out var bbVariable))
                        {
                            //ランダムな徘徊ポイントを取得
                            GameObject[] points = bbVariable.Value;
                            int index = UnityEngine.Random.Range(0, points.Length);
                            Transform point = points[index].transform;

                            transform.position = point.position; //ポイントにワープ
                        }

                        rb.excludeLayers = 0; //レイヤーを戻す
                        nvAgent.enabled = true;
                        agent.Start();
                    });
                }
                else
                {
                    Vector3 dir = (other.transform.position - transform.position).normalized; // 方向を計算
                    player.KnockBack(dir * _knockBackPower, _stunTime);
                }
            }
        }

        /// <summary>
        ///     ディレイしてからリスポーンする
        /// </summary>
        /// <param name="delayTime"></param>
        /// <param name="respawnAction"></param>
        private async void RespawnWithDelay(float delayTime, Action respawnAction)
        {
            try
            {
                await Awaitable.WaitForSecondsAsync(delayTime, destroyCancellationToken);
            }
            catch (OperationCanceledException) { }

            respawnAction?.Invoke();
            Respawn();
        }

        private void Dead()
        {
            OnDead?.Invoke();
        }

        private void Respawn()
        {
            OnRespawn?.Invoke();
        }
    }
}
