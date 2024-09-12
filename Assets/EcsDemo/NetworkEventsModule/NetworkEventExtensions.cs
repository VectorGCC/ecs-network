using Mirror;

public static class NetworkEventExtensions
{
    public static void SendNetworkEvent<T>(this IWorld world, T @event) where T : struct, NetworkMessage
    {
        NetworkClient.Send(@event);
    }
}