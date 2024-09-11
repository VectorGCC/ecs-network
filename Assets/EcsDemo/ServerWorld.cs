using System;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Mirror;
using SevenBoldPencil.EasyEvents;
using Object = UnityEngine.Object;
using Time = UnityEngine.Time;

[EcsSystem]
public class TestRecoverSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        foreach (var e in API.Query(systems).With<SyncComponent>())
        {
            ref var comp = ref e.Get<SyncComponent>();
            comp.Value += 1 * Time.deltaTime;
        }
    }
}

public static class API
{
    public static QueryBuilder Query(IEcsSystems systems)
    {
        return new QueryBuilder(systems.GetWorld());
    }
}

public struct QueryBuilder
{
    private readonly EcsWorld _world;
    private EcsWorld.Mask _filerMask;

    public QueryBuilder(EcsWorld world)
    {
        _world = world;
        _filerMask = null;
    }

    public QueryBuilder With<T>() where T : struct
    {
        _filerMask = _world.Filter<T>();
        return this;
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(_filerMask.End());
    }
}

public struct Enumerator : IDisposable
{
    private readonly EcsWorld _world;
    private EcsFilter.Enumerator _enumerator;

    public Enumerator(EcsFilter filter)
    {
        _enumerator = filter.GetEnumerator();
        _world = filter.GetWorld();
    }

    public EntityRW Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new EntityRW(_enumerator.Current, _world);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        return _enumerator.MoveNext();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        _enumerator.Dispose();
    }
}

[EcsSystem]
public class RemoveVersionComponentSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        foreach (var e in API.Query(systems).With<Version>())
        {
            if (e.ComponentsCount == 1 && e.Has<Version>())
            {
                e.Remove<Version>();
            }
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

        // Если сломалось, то закомментируй эти строку:
        // Tools -> UnityCodeGen -> Generate
        // Раскоментируй строки.
        //EcsSystemsCollection.Add(_systems);
        //NetworkEventDestroy.Add(_systems, _eventBus);

        _systems
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

public struct Version
{
    public uint Value;
}

public struct EntityRW
{
    public int Entity;
    public EcsWorld World;
    public uint Version => _versionPool.Get(Entity).Value;
    public int ComponentsCount => World.GetComponentsCount(Entity);

    private readonly EcsPool<Version> _versionPool;

    public EntityRW(int entity, EcsWorld world)
    {
        Entity = entity;
        World = world;
        _versionPool = world.GetPool<Version>();
    }

    public ref T Get<T>() where T : struct
    {
        _versionPool.Get(Entity).Value++;
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

    public bool Has<T>() where T : struct
    {
        return World.GetPool<T>().Has(Entity);
    }

    public void Remove<T>() where T : struct
    {
        World.GetPool<T>().Del(Entity);
    }
}