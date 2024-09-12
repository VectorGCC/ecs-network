using Leopotam.EcsLite;

public struct EntityRW
{
    public int Entity;
    public EcsWorld World;
    public uint Version => _versionPool.Get(Entity).Value;
    public int ComponentsCount => World.GetComponentsCount(Entity);

    private readonly EcsPool<Version> _versionPool;

    public EntityRW(int entity, EcsWorld world)
    {
        Entity = entity;
        World = world;
        _versionPool = world.GetPool<Version>();
    }

    public ref T Get<T>() where T : struct
    {
        if (_versionPool.Has(Entity))
            _versionPool.Get(Entity).Value++;
        else
            _versionPool.Add(Entity) = new Version {Value = 1};
        if (World.GetPool<T>().Has(Entity))
            return ref World.GetPool<T>().Get(Entity);
        else
            return ref World.GetPool<T>().Add(Entity);
    }

    public T Read<T>() where T : struct
    {
        if (World.GetPool<T>().Has(Entity))
            return World.GetPool<T>().Get(Entity);
        else
            return default;
    }

    public bool Has<T>() where T : struct
    {
        return World.GetPool<T>().Has(Entity);
    }

    public void Remove<T>() where T : struct
    {
        World.GetPool<T>().Del(Entity);
    }
}