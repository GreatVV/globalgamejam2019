using ExitGames.Client.Photon;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    internal class DumpAsteroidsToPropertiesSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter<Asteroid> _asteroids;

        private PhotonServer _photonServer;

        private GameConfig _gameConfig;

        private float LastDump;

        private Hashtable _hashtable = new Hashtable ();

        public void Run ()
        {
            var time = Time.realtimeSinceStartup;
            var diff = time - LastDump;
            if (diff > _gameConfig.AsteroidDumpInterval)
            {
                LastDump = time;
                var asteroidsAmount = _asteroids.EntitiesCount;
                var asteroids = new AsteroidDesc[asteroidsAmount];
                for (var i = 0; i < asteroidsAmount; i++)
                {
                    var asteroidDesc = new AsteroidDesc ();
                    var asteroidEntity = _asteroids.Entities[i];
                    asteroidDesc.Id = _asteroids.Components1[i].Id;
                    asteroidDesc.Index = _asteroids.Components1[i].Index;
                    asteroidDesc.Position = _world.GetComponent<Position> (asteroidEntity).value;
                    asteroidDesc.Rotation = _world.GetComponent<Rotation> (asteroidEntity).value;
                    asteroidDesc.Speed = _world.GetComponent<Speed> (asteroidEntity).value;
                    asteroids[i] = asteroidDesc;
                }
                _hashtable.Clear ();
                _hashtable[RoomDataConstants.Asteroids] = asteroids;
                _photonServer.CurrentRoom.SetCustomProperties (_hashtable);
            }
        }
    }
}
