using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    internal class DestroyTransformSystem : EcsReactiveSystem<Destroy>
    {
        private EcsWorld _world;

        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive ()
        {
            for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var transformRef = _world.GetComponent<TransformRef> (entity);
                if (transformRef != null)
                {
                    UnityEngine.Object.Destroy (transformRef.value.gameObject);
                }
                _world.RemoveEntity (entity);
            }
        }
    }
}
