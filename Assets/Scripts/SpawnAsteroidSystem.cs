using System.Collections.Generic;
using ExitGames.Client.Photon.LoadBalancing;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    public class SpawnAsteroidSystem : IEcsRunSystem
    {
        public GameState GameState;
        public GameConfig GameConfig;

        public PhotonServer photonServer;

        public SceneDescription sceneDescription;

        private readonly List<AsteroidDesc> _asteroids = new List<AsteroidDesc> ();
        public void Run ()
        {
            var time = Time.realtimeSinceStartup;
            var lastGenerationTime = GameState.lastGenerationTime;
            var timeFromLastGeneration = lastGenerationTime - time;
            if (timeFromLastGeneration > GameConfig.GenerationInterval)
            {
                var lastId = photonServer.CurrentRoom.CustomProperties.ContainsKey (RoomDataConstants.LastGeneratedId) ? (long) photonServer.CurrentRoom.CustomProperties[RoomDataConstants.LastGeneratedId] : 0;

                GameState.lastGenerationTime = Time.realtimeSinceStartup;
                var asteroidAmount = Random.Range (GameConfig.MinAsteroidPerGeneration, GameConfig.MaxAsteroidsPerGeneration);
                _asteroids.Clear ();
                for (int i = 0; i < asteroidAmount; i++)
                {
                    lastId++;
                    Vector3 shipPosition = sceneDescription.Ship.transform.position;
                    var position = shipPosition + Random.onUnitSphere * Random.Range (GameConfig.MinGenerationRadius, GameConfig.MaxGenerationRadius);

                    var direction = Quaternion.LookRotation (shipPosition - position);
                    var speed = Random.Range (GameConfig.MinAsteroidSpeed, GameConfig.MaxAsteroidSpeed);
                    var asteroidIndex = Random.Range (0, GameConfig.AsteroidsPrefabs.Length);

                    var asteroidDesc = new AsteroidDesc ();
                    asteroidDesc.Id = lastId;
                    asteroidDesc.Position = position;
                    asteroidDesc.Rotation = direction;
                    asteroidDesc.Speed = speed;
                    asteroidDesc.Index = asteroidIndex;
                    _asteroids.Add (asteroidDesc);
                }
                photonServer.OpRaiseEvent (GameEventCode.SpawnAsteroids, _asteroids, true, RaiseEventOptions.Default);
            }
        }
    }
}
