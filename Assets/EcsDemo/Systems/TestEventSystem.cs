using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

[AutoCollectSystem]
public class TestEventSystem : IEcsRunSystem
{
    private EcsCustomInject<EventsBus> _eventBus;

    public void Run(IEcsSystems systems)
    {
        if (_eventBus.Value.HasEvents<TestEvent>())
        {
            var filter = _eventBus.Value.GetEventBodies<TestEvent>(out var pool);
            foreach (var e in filter)
            {
                ref var testEvent = ref pool.Get(e);
                Debug.Log($"TestEventSystem: {testEvent.Value}");
            }
        }
    }
}