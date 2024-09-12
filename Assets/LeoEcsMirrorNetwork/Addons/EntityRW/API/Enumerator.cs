using System;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

public struct Enumerator : IDisposable
{
    private readonly EcsWorld _world;
    private EcsFilter.Enumerator _enumerator;

    public Enumerator(EcsFilter filter)
    {
        _enumerator = filter.GetEnumerator();
        _world = filter.GetWorld();
    }

    public EntityRW Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new EntityRW(_enumerator.Current, _world);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        return _enumerator.MoveNext();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        _enumerator.Dispose();
    }
}