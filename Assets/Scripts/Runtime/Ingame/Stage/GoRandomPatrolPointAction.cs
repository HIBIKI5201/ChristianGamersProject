using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoRandomPatrolPoint", story: "[self] go to random [PatrolPoints]", category: "Action", id: "11eace440f96c160269fdc3133aa697d")]
public partial class GoRandomPatrolPointAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<List<GameObject>> PatrolPoints;

    private NavMeshAgent _navMeshAgent;

    protected override Status OnStart()
    {
        if (!Self.Value.TryGetComponent<NavMeshAgent>(out _navMeshAgent))
        {
            Debug.LogError("GoRandomPatrolPointAction: Self does not have a NavMeshAgent component.");
            return Status.Failure;
        }

        //ランダムなパトロールポイントを選択
        int index = UnityEngine.Random.Range(0, PatrolPoints.Value.Count);
        Vector3 point = PatrolPoints.Value[index].transform.position;

        if (!NavMesh.SamplePosition(point, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            Debug.LogError("GoRandomPatrolPointAction: No valid NavMesh position found near the patrol point.");
            return Status.Failure;
        }

        _navMeshAgent.SetDestination(hit.position); //移動開始

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            //目的地に到達した場合、ステータスを成功に設定
            return Status.Success;
        }

        return Status.Running; //まだ移動中
    }

    protected override void OnEnd()
    {
    }
}

