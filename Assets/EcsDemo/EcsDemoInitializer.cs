using System.Linq;
using ME.BECS;
using ME.BECS.Network;
using ME.BECS.Network.Markers;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

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
                var path = new NavMeshPath();
                NavMesh.CalculatePath(source.transform.position, target.transform.position, NavMesh.AllAreas, path);

                unsafe
                {
                    var list = new List<float3>(ref world.state->allocator, 1u);
                    foreach (var corner in path.corners)
                    {
                        list.Add(ref world.state->allocator, corner);
                    }

                    world.SendNetworkEvent(new MovePath()
                    {
                        Path = list
                    }, MovePath.OnReceive);
                }
            }
        }
    }
}