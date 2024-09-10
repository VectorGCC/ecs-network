using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;

public class ServerWorld : IWorld
{
    private EcsWorld _world;
    private EcsSystems _systems;
    private EventsBus _eventBus;

    public void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _eventBus = new EventsBus();

        _systems
            .Add(new TestEventSystem())
            // Автоматическое удаление ивентов.
            .Add(_eventBus.GetDestroyEventsSystem()
                //.IncSingleton<PlayerReloadGunEvent>()
            )
#if UNITY_EDITOR
            // Регистрируем отладочные системы по контролю за состоянием каждого отдельного мира:
            // .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem("events"))
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
            // Регистрируем отладочные системы по контролю за текущей группой систем. 
            .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem())
#endif
            .Inject(_eventBus)
            .Init()
            ;
    }

    public void OnDestroy()
    {
        _systems?.Destroy();
        _world?.Destroy();
        _eventBus?.Destroy();
    }

    public void Update()
    {
        _systems?.Run();
    }

    public void SendNetworkEvent<T>(T @event) where T : unmanaged, INetworkEvent
    {
        AddEvent(@event);
    }

    public void AddEvent<T>(T @event) where T : unmanaged, IEventReplicant
    {
        _eventBus.NewEvent<T>() = @event;
    }
}