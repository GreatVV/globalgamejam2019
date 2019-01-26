using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    internal class LerpPositionSystem : IEcsRunSystem
    {
        EcsFilter<TransformRef, Position> _filter;
        private float teleportDistance = 3f;

        public void Run ()
        {
            var dt = Time.deltaTime;
            foreach (var index in _filter)
            {
                var position = _filter.Components2[index];
                var transformRef = _filter.Components1[index];
                var distance = Vector3.Distance (transformRef.value.position, position.value);
                if (transformRef.value.position != position.value)
                {
                    // if (distance < teleportDistance)
                    // {
                    //     transformRef.value.position = Vector3.MoveTowards(transformRef.value.position, position.value, dt * distance / 5);
                    // }
                    // else
                    // {
                        transformRef.value.position = position.value;
                    // }
                }
            }
        }
    }
}
