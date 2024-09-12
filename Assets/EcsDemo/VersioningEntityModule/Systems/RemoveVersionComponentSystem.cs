using Leopotam.EcsLite;

public class RemoveVersionComponentSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        foreach (var e in API.Query(systems).With<Version>())
        {
            if (e.ComponentsCount == 1 && e.Has<Version>())
            {
                e.Remove<Version>();
            }
        }
    }
}