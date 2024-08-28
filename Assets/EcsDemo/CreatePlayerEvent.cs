using AOT;
using ME.BECS;
using ME.BECS.Network;
using Unity.Collections;

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
        public static void OnNetworkReceive(in InputData data, ref SystemContext context)
        {
            context.world.NewEnt().Get<CreatePlayerEvent>() = data.GetData<CreatePlayerEvent>();
        }
    }
}