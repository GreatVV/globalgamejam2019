using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;
using UnityEngine;

namespace Client
{
    [EcsInject]
    internal class CreateTestPlayer : EcsReactiveSystem<GamePlayer>
    {
        EcsWorld _world;
        PlayerCache PlayerCache;

        protected override EcsReactiveType GetReactiveType()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive()
        {
            for (var i = 0; i < ReactedEntitiesCount; i++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                var entity = ReactedEntities[i];
                _world.AddComponent<TransformRef>(entity).value = go.transform;
                go.transform.position = new Vector3(2 * _world.GetComponent<GamePlayer>(ReactedEntities[i]).number, 0, 0);
            }
        }
    }
}