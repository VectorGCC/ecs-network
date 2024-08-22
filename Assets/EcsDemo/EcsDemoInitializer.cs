using ME.BECS.Network;
using ME.BECS.Network.Markers;
using UnityEngine;

namespace EcsDemo
{
    public class EcsDemoInitializer : NetworkWorldInitializer
    {
        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse button down");
                world.SendNetworkEvent(new MouseButtonDownEvent()
                {
                    Value = 100
                }, InputSystem.OnMouseButtonDown);
            }
        }
    }
}