using AOT;
using ME.BECS;
using ME.BECS.Network;
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

    public struct MovePath : IPackageData, IComponent
    {
        public List<float3> Path;
        public uint CurrentDestinationIndex;

        [NetworkMethod]
        [MonoPInvokeCallback(typeof(NetworkMethodDelegate))]
        public static void OnReceive(in InputData data, ref SystemContext context)
        {
            foreach (var e in API.Query(context.world, context.dependsOn).With<Health>())
            {
                e.Get<MovePath>() = data.GetData<MovePath>();
            }
        }

        public void Serialize(ref StreamBufferWriter writer)
        {
            writer.Write(Path);
            writer.Write(CurrentDestinationIndex);
        }

        public void Deserialize(ref StreamBufferReader reader)
        {
            reader.Read(ref Path);
            reader.Read(ref CurrentDestinationIndex);
            Debug.Log(Path.Count);
        }
    }

    public struct MovementSystem : IUpdate
    {
        public unsafe void OnUpdate(ref SystemContext context)
        {
            foreach (var e in API.Query(context.world, context.dependsOn).With<MovePath>().WithAspect<TransformAspect>())
            {
                /*
                var path = e.Read<MovePath>();
                if (path.CurrentDestinationIndex >= path.Path.Count)
                {
                    e.Remove<MovePath>();
                    continue;
                }

                var destination = path.Path[context.world.state->allocator, path.CurrentDestinationIndex];
                var transform = e.GetAspect<TransformAspect>();
                if (Vector3.Distance(transform.position, destination) < 0.1f)
                {
                    e.Get<MovePath>().CurrentDestinationIndex++;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, destination, 1f * context.deltaTime);
                }
                */
            }
        }
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
            }
        }
    }
}