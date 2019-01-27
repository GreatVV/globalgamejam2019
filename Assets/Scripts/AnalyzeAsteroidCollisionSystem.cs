using System.Collections.Generic;
using ExitGames.Client.Photon;
using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    internal class AnalyzeAsteroidCollisionSystem : EcsReactiveSystem<AsteroidCollision>
    {
        private GameConfig _gameConfig;
        private EcsWorld _world;

        private PhotonServer _photonServer;
        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        private Hashtable _hashtable = new Hashtable ();

        private readonly List<int> _asteroidsId = new List<int> ();

        protected override void RunReactive ()
        {
            _asteroidsId.Clear ();
            _hashtable.Clear ();
            var health = (float) _photonServer.CurrentRoom.CustomProperties[RoomDataConstants.Health];

            for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                if (!_world.IsEntityExists (entity))
                {
                    continue;
                }
                var asteroidCollision = _world.GetComponent<AsteroidCollision> (entity);
                health -= _gameConfig.DamageFromAsteroid;

                var asteroid = _world.GetComponent<Asteroid> (asteroidCollision.AsteroidEntity);
                _asteroidsId.Add (asteroid.Id);
            }

            _photonServer.OpRaiseEvent (GameEventCode.DamageByAsteroids, _asteroidsId.ToArray (), true, ServerSpawnAsteroidSystem.All);

            _hashtable[RoomDataConstants.Health] = health;
            _photonServer.CurrentRoom.SetCustomProperties (_hashtable);
        }
    }
}
