using AOT;
using ME.BECS;
using ME.BECS.Network;
using UnityEngine;

namespace EcsDemo
{
    public struct InputSystem : ISystem
    {
        [NetworkMethod]
        [MonoPInvokeCallback(typeof(NetworkMethodDelegate))]
        public static void OnMouseButtonDown(in InputData data, ref SystemContext context)
        {
            Debug.Log("Mouse button down event received with value: " + data.GetData<MouseButtonDownEvent>().Value);
            context.world.NewEnt().Set<DebugLog>(new DebugLog()
            {
                Message = "Mouse button msg" + data.GetData<MouseButtonDownEvent>().Value,
                Value = 5
            });
        }
    }
}