using ByteStream.Interfaces;
using ByteStream.Mananged;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Mirror;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

public interface IWorld
{
    void Start();
    void OnDestroy();
    void Update();
}

public interface INetworkEvent
{
    void Serialize(IByteStream stream);
}

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

public class ClientWorld : IWorld
{
    private NetworkWorldPlayer _networkPlayer;

    public void Start()
    {
    }

    public void OnDestroy()
    {
    }

    public void Update()
    {
    }
}

public class GameManager : MonoBehaviour
{
    public IWorld World { get; private set; }
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        World = NetworkServer.active ? (IWorld) new ServerWorld() : World;
        World = NetworkClient.active ? (IWorld) new ClientWorld() : World;
    }

    private void Start()
    {
        World?.Start();
    }

    private void OnDestroy()
    {
        World?.OnDestroy();
    }

    private void Update()
    {
        World?.Update();
    }
}