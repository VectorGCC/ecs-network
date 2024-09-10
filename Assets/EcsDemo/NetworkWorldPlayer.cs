using System;
using System.Runtime.InteropServices;
using ByteStream.Interfaces;
using ByteStream.Mananged;
using Mirror;

public interface IPackageData
{
    void Deserialize(IByteStream stream);
    void AddEvent();
}

public struct PackageData<T> : IPackageData where T : unmanaged, INetworkEvent
{
    public T Data;

    public void Serialize(ManagedStream stream)
    {
        Data.Serialize(stream);
    }

    public void Deserialize(IByteStream stream)
    {
        Data.Serialize(stream);
    }

    public void AddEvent()
    {
        if (GameManager.Instance.World is ServerWorld serverWorld)
        {
            serverWorld.AddEvent(Data);
        }
    }
}

public class NetworkWorldPlayer : NetworkBehaviour
{
    [Command]
    public void CmdSendNetworkEvent(string typeName, byte[] bytes)
    {
        var type = Type.GetType(typeName);
        var packageDataType = typeof(PackageData<>).MakeGenericType(type);
        var package = Activator.CreateInstance(packageDataType) as IPackageData;

        var stream = new ManagedStream();
        stream.ResetRead(bytes);
        package.Deserialize(stream);
        package.AddEvent();
    }
}