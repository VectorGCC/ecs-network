using ME.BECS;
using Unity.Collections;

namespace EcsDemo
{
    public struct DebugLog : IComponent
    {
        public FixedString4096Bytes Message;
        public int Value;
    }
}