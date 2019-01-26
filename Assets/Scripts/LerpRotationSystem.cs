using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    internal class LerpRotationSystem : IEcsRunSystem
    {
        EcsFilter<TransformRef, Rotation> _filter;
        public void Run ()
        {
            var dt = Time.deltaTime;
            foreach (var index in _filter)
            {
                var rotation = _filter.Components2[index];
                var transformRef = _filter.Components1[index];
                if (transformRef.value.rotation != rotation.value)
                {
                    transformRef.value.rotation = Quaternion.Lerp(transformRef.value.rotation, rotation.value, dt);
                }
            }
        }
    }
}
