using Mirror;
using UnityEngine;

public class StartWorldScenario : MonoBehaviour
{
    public bool _sended;
    public EntityView _entityView;

    void Update()
    {
        if (_sended)
            return;

        if (NetworkClient.active)
        {
            NetworkClient.Send(new TestEvent()
            {
                Value = 100
            });
            _sended = true;
        }


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