using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    public class UpdateCannonDirectionSystem : EcsReactiveSystem<ChangeShootTarget>
    {
        public SceneDescription SceneDescription;
        private EcsWorld _world;

        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive ()
        {
            var entity = ReactedEntities[0];
            var target = _world.GetComponent<ChangeShootTarget> (entity);
            var shipView = SceneDescription.Ship;
            foreach (var cannon in shipView.Cannons)
            {
                cannon.Root.LookAt (target.value);
            }
        }
    }
}
