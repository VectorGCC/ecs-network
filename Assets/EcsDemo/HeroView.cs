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

    protected override unsafe void ApplyState(in EntRO ent)
    {
        gameObject.name = ent.Read<Name>().Value.ToString();
        _health.text = ent.Read<Health>().Value.ToString();
        if (ent.TryRead(out MovePath movePath))
        {
            // https://stackoverflow.com/questions/46495820/unity3d-how-to-connect-navmesh-and-navmeshagent
            if (!_navMeshAgent.isOnNavMesh)
            {
                Vector3 warpPosition = transform.position; //Set to position you want to warp to
                _navMeshAgent.transform.position = warpPosition;
                _navMeshAgent.enabled = false;
                _navMeshAgent.enabled = true;
            }

            _navMeshAgent.updatePosition = false;
            _navMeshAgent.SetDestination(movePath.Path[this.ent.World.state->allocator, 0]);
            var pos = _navMeshAgent.nextPosition;
            Debug.Log(pos);
        }
    }

    /*
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
        
    }
*/
}