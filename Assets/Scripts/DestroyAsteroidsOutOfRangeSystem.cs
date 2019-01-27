using System;
using System.Collections.Generic;
using Leopotam.Ecs;

namespace Client
{
    [EcsInject]
    internal class DestroyAsteroidsOutOfRangeSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter<Asteroid, Position> _asteroids;
        private GameState _gameState;

        private GameConfig _gameConfig;
        private List<int> _asteroidIds = new List<int> ();

        private PhotonServer _photonServer;
        public void Run ()
        {
            var playerTransform = _world.GetComponent<TransformRef> (_gameState.ShipEntity);
            foreach (var index in _asteroids)
            {
                var position = _asteroids.Components2[index].value;
                var distance = (position - playerTransform.value.position).sqrMagnitude;
                if (distance > _gameConfig.DeathDistance * _gameConfig.DeathDistance)
                {
                    _asteroidIds.Add (_asteroids.Components1[index].Id);
                }
            }

            if (_asteroidIds.Count > 0)
            {
                _photonServer.OpRaiseEvent (GameEventCode.KillAsteroids, _asteroidIds.ToArray (), true, ServerSpawnAsteroidSystem.All);
                _asteroidIds.Clear ();
            }
        }
    }
}
