using System;
using Mirror;
using UnityEngine;

public struct SyncComponent
{
    public float Value;
}

[RequireComponent(typeof(NetworkIdentity))]
public abstract class EntityView : NetworkBehaviour
{
    [SyncVar(hook = nameof(Hook))] SyncComponent _syncComponent;

    private EntityRW _entity;
    private uint _lastVersion;

    protected virtual void ApplyState()
    {
    }

    public void Hook(SyncComponent s, SyncComponent l)
    {
        ApplyState();
    }

    [Server]
    public void Init(EntityRW entity)
    {
        _entity = entity;
    }

    private void Update()
    {
        if (!NetworkServer.active)
            return;

        if (_entity.Version != _lastVersion)
        {
            _lastVersion = _entity.Version;
            _syncComponent = _entity.Read<SyncComponent>();
        }
    }

    public SyncComponent Read()
    {
        return _syncComponent;
    }
}