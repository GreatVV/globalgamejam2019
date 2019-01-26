using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsOneFrame]
    public class ShipPosition
    {
        public Vector3 position;
        public Quaternion rotation;
    }
}