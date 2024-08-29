using ME.BECS.Network;
using ME.BECS.Network.Markers;
using UnityEngine;

namespace EcsDemo
{
    public class EcsDemoInitializer : NetworkWorldInitializer
    {
        public GameObject source;
        public GameObject target;


        protected override void LateUpdate()
        {
            base.LateUpdate();

            Debug.Log($"Local: {this.networkModule.LocalPlayerId}");

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse button down");
                world.SendNetworkEvent(new CreatePlayerEvent()
                {
                    Name = "Bob"
                }, CreatePlayerEvent.OnReceive);
            }

            if (Input.GetMouseButtonDown(1))
            {
                world.SendNetworkEvent(new MovePath()
                {
                    Destination = target.transform.position
                }, MovePath.OnReceive);
            }
        }
    }
}