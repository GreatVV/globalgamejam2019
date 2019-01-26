using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    internal class AsteroidMoveSystem : IEcsRunSystem
    {
        EcsFilter<Asteroid, Position, Speed, TransformRef> _filter;
        public void Run ()
        {
            var dt = Time.deltaTime;
            foreach (var index in _filter)
            {
                var position = _filter.Components2[index];
                var speed = _filter.Components3[index];
                var transform = _filter.Components4[index];
                position.value += transform.value.forward * dt * speed.value;
            }
        }
    }
}
