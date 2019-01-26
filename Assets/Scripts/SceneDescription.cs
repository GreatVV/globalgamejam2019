using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class SceneDescription : MonoBehaviour
    {
        [Header ("Shoot player config")]
        public GameObject ShootPlayerCamera;
        public Radar Radar;

        [Header ("Fly player config")]
        public GameObject FlyPlayerCamera;

        [Header ("Ship")]
        public ShipView Ship;
        public float Speed = 10;
        public float RotationSpeed = 5;

        public UI UI;
        public Camera GameCamera;

        public int CreateShip (EcsWorld world)
        {
            var ship = world.CreateEntity ();
            world.AddComponent<Ship> (ship);
            world.AddComponent<TransformRef> (ship).value = Ship.transform;
            world.AddComponent<Speed> (ship).value = Speed;
            world.AddComponent<RotationSpeed> (ship).value = RotationSpeed;
            world.AddComponent<RigidbodyRef> (ship).value = Ship.Rigidbody;
            Ship.Inject (world);
            return ship;
        }
    }
}
