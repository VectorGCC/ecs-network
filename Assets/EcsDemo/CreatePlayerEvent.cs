using AOT;
using ME.BECS;
using ME.BECS.Network;
using Unity.Collections;
using UnityEngine;

namespace EcsDemo
{
    public struct CreatePlayerEvent : IPackageData, IComponent
    {
        public FixedString512Bytes Name;

        public void Serialize(ref StreamBufferWriter writer)
        {
            writer.Write(Name);
        }

        public void Deserialize(ref StreamBufferReader reader)
        {
            reader.Read(ref Name);
        }

        [NetworkMethod]
        [MonoPInvokeCallback(typeof(NetworkMethodDelegate))]
        public static void OnReceive(in InputData data, ref SystemContext context)
        {
            Debug.Log("Receive create player event");
            context.world.NewEnt().SetOneShot(data.GetData<CreatePlayerEvent>());
        }
    }
}