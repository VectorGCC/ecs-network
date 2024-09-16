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
            .AddAutoCollectSystems()
            .Add(new TestRecoverSystem())
            .Add(new VersioningSystem())
            .Add(new NetworkEventsSystem(_eventBus)
                .Register<TestEvent>())
#if UNITY_EDITOR
            // ������������ ���������� ������� �� �������� �� ���������� ������� ���������� ����:
            // .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem("events"))
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
            // ������������ ���������� ������� �� �������� �� ������� ������� ������. 
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

    public EntityRW CreateEntity()
    {
        var entity = _world.NewEntity();
        return new EntityRW(entity, _world);
    }
}