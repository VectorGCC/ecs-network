using Mirror;
using SevenBoldPencil.EasyEvents.EcsDemo;
using UnityEngine;

[NetworkEvent]
public struct TestEvent
{
    public int Value;
}

public class StartWorldScenario : MonoBehaviour
{
    public bool _sended;
    public EntityView _entityView;

    void Update()
    {
        if (_sended)
            return;
        
        if (NetworkClient.localPlayer == null)
          //  return;
        
        _sended = true;

        /*
        GameManager.Instance.World.SendNetworkEvent(new TestEvent()
        {
            Value = 100
        });
        */

        if (GameManager.Instance.World is ServerWorld serverWorld)
        {
            var ent = serverWorld.CreateEntity();
            ent.Get<SyncComponent>() = new SyncComponent()
            {
                Value = 50
            };
            ent.InstantiateView(_entityView);
        }
    }
}