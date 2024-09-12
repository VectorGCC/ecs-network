using Leopotam.EcsLite;
using UnityEngine;

public class TestRecoverSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        foreach (var e in API.Query(systems).With<SyncComponent>())
        {
            ref var comp = ref e.Get<SyncComponent>();
            if(comp.Value > 55)
                continue;
            comp.Value += 1 * Time.deltaTime;
        }
    }
}