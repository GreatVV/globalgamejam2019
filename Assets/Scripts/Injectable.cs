using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class Injectable : MonoBehaviour
    {
        protected EcsWorld World;

        public void Inject (EcsWorld world)
        {
            World = world;
        }
    }

}
