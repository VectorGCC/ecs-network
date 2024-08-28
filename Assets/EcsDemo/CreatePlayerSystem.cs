using ME.BECS;
using ME.BECS.Transforms;
using ME.BECS.Views;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace EcsDemo
{
    public struct Name : IComponent
    {
        public FixedString512Bytes Value;
    }

    public struct Health : IComponent
    {
        public static Health Default => new Health()
        {
            Value = 50,
            MaxValue = 100,
            RecoverPerSecond = 2
        };

        public float Value;
        public float MaxValue;
        public float RecoverPerSecond;
    }

    public struct HealthRecoverySystem : IUpdate
    {
        public void OnUpdate(ref SystemContext context)
        {
            foreach (var e in API.Query(context.world, context.dependsOn).With<Health>())
            {
                var health = e.Read<Health>();
                if (health.Value < health.MaxValue)
                {
                    e.Get<Health>().Value = Mathf.Clamp(health.Value + health.RecoverPerSecond * context.deltaTime, 0f, health.MaxValue);
                }
            }
        }
    }

    public struct CreatePlayerSystem : IUpdate
    {
        public View PlayerPrefab;
        private int _countPlayers;

        public void OnUpdate(ref SystemContext context)
        {
            foreach (var e in API.Query(context.world, context.dependsOn).With<CreatePlayerEvent>())
            {
                e.Get<Name>().Value = e.Read<CreatePlayerEvent>().Name;
                e.Get<Health>() = Health.Default;

                var transform = e.GetOrCreateAspect<TransformAspect>();
                transform.position = new float3(0f, _countPlayers * 2 * -1, 0f);
                _countPlayers++;

                e.InstantiateView(PlayerPrefab);
                e.Remove<CreatePlayerEvent>();
            }
        }
    }
}