using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public abstract class EntityViewBase : NetworkBehaviour
{
    protected virtual T Read<T>() where T : struct
    {
        // Override from code generation.
        return default;
    }
}

public abstract partial class EntityView : EntityViewBase
{
    private EntityRW _entity;
    private uint _lastVersion;

    protected virtual void ApplyState()
    {
    }

    [Server]
    public void Init(EntityRW entity)
    {
        _entity = entity;
    }
}