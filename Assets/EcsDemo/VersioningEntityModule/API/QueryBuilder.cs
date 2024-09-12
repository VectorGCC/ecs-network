using Leopotam.EcsLite;

public struct QueryBuilder
{
    private readonly EcsWorld _world;
    private EcsWorld.Mask _filerMask;

    public QueryBuilder(EcsWorld world)
    {
        _world = world;
        _filerMask = null;
    }

    public QueryBuilder With<T>() where T : struct
    {
        _filerMask = _world.Filter<T>();
        return this;
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(_filerMask.End());
    }
}