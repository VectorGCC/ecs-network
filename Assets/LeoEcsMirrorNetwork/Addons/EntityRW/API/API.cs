using Leopotam.EcsLite;

public static class API
{
    public static QueryBuilder Query(IEcsSystems systems)
    {
        return new QueryBuilder(systems.GetWorld());
    }
}