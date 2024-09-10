using ByteStream.Interfaces;
using Mirror;
using UnityEngine;

public struct TestEvent : INetworkEvent
{
    public int Value;

    public void Serialize(IByteStream stream)
    {
        stream.Serialize(ref Value);
    }
}

public class StartWorldScenario : MonoBehaviour
{
    public bool _sended;

    void Update()
    {
        if (_sended)
            return;
        if (NetworkClient.localPlayer == null)
            return;

        var test = new TestEvent()
        {
            Value = 100
        };
        GameManager.Instance.World.SendNetworkEvent(test);
    }
}