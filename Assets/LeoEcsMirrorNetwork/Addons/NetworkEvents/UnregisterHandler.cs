using System;
using Mirror;

public class UnregisterHandler<T> : IDisposable where T : struct, NetworkMessage
{
    public void Dispose()
    {
        NetworkServer.UnregisterHandler<T>();
    }
}