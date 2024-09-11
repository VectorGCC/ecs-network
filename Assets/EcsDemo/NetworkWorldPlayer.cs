using Mirror;

public partial class NetworkWorldPlayer : NetworkBehaviour
{
    [Server]
    private void OnServerReceiveEvent<T>(T @event) where T : struct
    {
        if (GameManager.Instance.World is ServerWorld serverWorld)
        {
            serverWorld.AddEvent(@event);
        }
    }
}