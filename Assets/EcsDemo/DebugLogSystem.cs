using ME.BECS;
using Unity.Burst;
using UnityEngine;

namespace EcsDemo
{
    [BurstCompile(CompileSynchronously = true)]
    public struct DebugLogSystem : IUpdate
    {
        public void OnUpdate(ref SystemContext context)
        {
            Debug.Log("DebugLogSystem.OnUpdate");
            var job = API.Query(context.world, context.dependsOn).With<DebugLog>().ForEach((in CommandBufferJob buffer) =>
            {
                var log = buffer.Get<DebugLog>();
                log.Value++;
                //Debug.Log($"[{buffer.ent.id}] {log.Message.ToString()}");
            });
            /*
            foreach (var e in logs)
            {
                var log = e.Get<DebugLog>();
                Debug.Log($"[{e.id}] {log.Message.ToString()}");
            }
            */

            context.SetDependency(job);
        }
    }
}