using ME.BECS;
using ME.BECS.Network;

public struct MouseButtonDownEvent : IPackageData
{
    public int Value;

    public void Serialize(ref StreamBufferWriter writer)
    {
        writer.Write(this);
    }

    public void Deserialize(ref StreamBufferReader reader)
    {
        reader.Read(ref this);
    }
}