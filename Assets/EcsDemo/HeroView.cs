using EcsDemo;
using ME.BECS;
using ME.BECS.Views;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class HeroView : EntityView
{
    [SerializeField] private TMP_Text _health;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    protected override void ApplyState(in EntRO ent)
    {
        gameObject.name = ent.Read<Name>().Value.ToString();
        _health.text = ent.Read<Health>().Value.ToString();
        if (ent.TryRead(out MovePath movePath))
        {
            _navMeshAgent.SetDestination(movePath.Destination);
        }
    }

    protected override void OnUpdate(in EntRO ent, float dt)
    {
        if (_navMeshAgent.path == null)
            return;

        Debug.Log("Path status: " + _navMeshAgent.path.status);
        /*
        for (var i = 0; i < _navMeshAgent.path.corners.Length - 1; i++)
        {
            Debug.DrawLine(_navMeshAgent.path.corners[i], _navMeshAgent.path.corners[i + 1], Color.red);
        }
        */
    }
}