using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class SceneDescription : MonoBehaviour
    {
        public Transform Ship;

        public void Init(EcsWorld world)
        {
            var ship = world.CreateEntity();
            world.AddComponent<Ship>(ship);
            world.AddComponent<TransformRef>(ship).value = Ship;
        }
    }
}