using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    public class SpawnWeaponRaysSystem : EcsReactiveSystem<ShootRay>
    {
        private EcsWorld _world;
        private SceneDescription _sceneDescription;
        private GameConfig _gameConfig;

        private GameState _gameState;

        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive ()
        {
            var entity = ReactedEntities[0];
            var shootRay = _world.GetComponent<ShootRay> (entity);
            var cannons = _sceneDescription.Ship.Cannons;
            if (shootRay.AsteroidTarget == -1)
            {
                foreach (var cannonView in cannons)
                {
                    var endPosition = cannonView.Root.forward * 1000;
                    cannonView.Root.LookAt (endPosition);
                    cannonView.Ray.Set (cannonView.RayStart.position, endPosition);
                }
            }
            else
            {
                if (_gameState.Asteroids.TryGetValue (shootRay.AsteroidTarget, out var asteroidEntity))
                {
                    var view = _world.GetComponent<TransformRef> (asteroidEntity);
                    var endPosition = view.value.position;
                    foreach (var cannonView in cannons)
                    {
                        cannonView.Root.LookAt (endPosition);
                        cannonView.Ray.Set (cannonView.RayStart.position, endPosition);
                    }
                }

            }
        }
    }
}
