using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SyncAgentVelocityAndAnimParam", story: "[Agent] agent velocity sync [Animator] Parameter", category: "Action", id: "a520b279f29fec16b6052c4aaedff88e")]
public partial class SyncAgentVelocityAndAnimParamAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;
    [SerializeReference] public BlackboardVariable<Animator> Animator;

    protected override Status OnStart()
    {
        if (Animator == null || Agent == null)
        {
            Debug.LogError("Animator or Self is not set.");
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 localVelocity = Agent.Value.transform.InverseTransformDirection(Agent.Value.velocity);
        UpdateVelocity(new (localVelocity.x, localVelocity.z));

        return Status.Running;
    }

    protected override void OnEnd()
    {
        UpdateVelocity(Vector2.zero);
    }

    private void UpdateVelocity(Vector2 dir)
    {
        dir.Normalize();

        Animator.Value.SetFloat("MoveX", dir.x);
        Animator.Value.SetFloat("MoveY", dir.y);
    }
}

