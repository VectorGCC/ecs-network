using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Mirror;
using SevenBoldPencil.EasyEvents;
using Object = UnityEngine.Object;
using Time = UnityEngine.Time;

public class TestRecoverSystem : IEcsRunSystem
{
    public EcsFilterInject<Inc<SyncComponent>> _filter;
    public void Run(IEcsSystems systems)
    {
        foreach (var e in _filter.Value)
        {
            ref var comp = ref _filter.Pools.Inc1.Get(e);
            comp.Value += 1 * Time.deltaTime;
        }
    }
}

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
            .Add(new TestRecoverSystem())
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

    public void AddEvent<T>(T @event) where T : struct
    {
        _eventBus.NewEvent<T>() = @event;
    }

    public EntityRW CreateEntity()
    {
        var entity = _world.NewEntity();
        return new EntityRW(entity, _world);
    }
}

public struct EntityRW
{
    public int Entity;
    public EcsWorld World;
    public uint Version => World.GetPool<Version>().Get(Entity).Value;

    public EntityRW(int entity, EcsWorld world)
    {
        Entity = entity;
        World = world;
    }

    public ref T Get<T>() where T : struct
    {
        if (World.GetPool<T>().Has(Entity))
            return ref World.GetPool<T>().Get(Entity);
        else
            return ref World.GetPool<T>().Add(Entity);
    }

    public T Read<T>() where T : struct
    {
        if (World.GetPool<T>().Has(Entity))
            return World.GetPool<T>().Get(Entity);
        else
            return default;
    }

    // TODO: To extension.
    public void InstantiateView(EntityView prefabView)
    {
        var view = Object.Instantiate(prefabView);
        view.Init(this);

        NetworkServer.Spawn(view.gameObject);
    }
}