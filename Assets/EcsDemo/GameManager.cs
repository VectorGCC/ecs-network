using Mirror;
using UnityEngine;

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

        World = NetworkServer.active ? new ServerWorld() : World;
        World = NetworkClient.active ? new ClientWorld() : World;
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