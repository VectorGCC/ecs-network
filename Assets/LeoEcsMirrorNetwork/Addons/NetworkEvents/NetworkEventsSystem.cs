using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Mirror;
using SevenBoldPencil.EasyEvents;

public class NetworkEventsSystem : IEcsPreInitSystem, IEcsDestroySystem, IEcsRunSystem
{
    private readonly EventsBus _eventsBus;
    private readonly List<IRegisterHandler> _registerHandlers = new List<IRegisterHandler>();
    private readonly List<IDisposable> _unregisterHandlers = new List<IDisposable>();
    private readonly List<Action> _destructionActions = new List<Action>(16);

    public NetworkEventsSystem(EventsBus eventsBus)
    {
        _eventsBus = eventsBus;
    }

    public NetworkEventsSystem Register<T>() where T : struct, NetworkMessage
    {
        _registerHandlers.Add(new RegisterHandler<T>(_eventsBus));
        _destructionActions.Add(() => _eventsBus.DestroyEvents<T>());
        return this;
    }

    public void PreInit(IEcsSystems systems)
    {
        foreach (var register in _registerHandlers)
        {
            var unregisterHandler = register.Register();
            _unregisterHandlers.Add(unregisterHandler);
        }
    }

    public void Destroy(IEcsSystems systems)
    {
        foreach (var unregisterHandler in _unregisterHandlers)
        {
            unregisterHandler.Dispose();
        }
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var action in _destructionActions)
        {
            action();
        }
    }
}