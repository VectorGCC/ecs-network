using System;
using Mirror;
using SevenBoldPencil.EasyEvents;

public class RegisterHandler<T> : IRegisterHandler where T : struct, NetworkMessage
{
    private readonly EventsBus _eventsBus;

    public RegisterHandler(EventsBus eventsBus)
    {
        _eventsBus = eventsBus;
    }

    public IDisposable Register()
    {
        NetworkServer.RegisterHandler<T>(OnServerReceiveEvent);
        return new UnregisterHandler<T>();
    }

    private void OnServerReceiveEvent(NetworkConnection conn, T @event)
    {
        _eventsBus.NewEvent<T>() = @event;
    }
}