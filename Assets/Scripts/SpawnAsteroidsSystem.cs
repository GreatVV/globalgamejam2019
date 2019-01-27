using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    internal class SpawnAsteroidsSystem : EcsReactiveSystem<SpawnAsteroids>
    {
        private EcsWorld _world;
        private GameConfig _gameConfig;

        private GameState _gameState;
        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive ()
        {
            for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var spawnAsteroids = _world.GetComponent<SpawnAsteroids> (entity);
                if (spawnAsteroids != null)
                {
                    foreach (var asteroidDesc in spawnAsteroids.value)
                    {
                        var asteroidEntity = _world.CreateEntity ();
                        Asteroid asteroid = _world.AddComponent<Asteroid> (asteroidEntity);
                        asteroid.Id = asteroidDesc.Id;
                        _world.AddComponent<Position> (asteroidEntity).value = asteroidDesc.Position;
                        _world.AddComponent<Rotation> (asteroidEntity).value = asteroidDesc.Rotation;
                        _world.AddComponent<Speed> (asteroidEntity).value = asteroidDesc.Speed;
                        var index = asteroidDesc.Index;
                        asteroid.Index = index;
                        var instance = UnityEngine.Object.Instantiate (_gameConfig.AsteroidsPrefabs[index], asteroidDesc.Position, asteroidDesc.Rotation);
                        _world.AddComponent<TransformRef> (asteroidEntity).value = instance.transform;
                        instance.Entity = asteroidEntity;
                        _gameState.Asteroids[asteroidDesc.Id] = asteroidEntity;
                    }
                }
            }
        }
    }
}
