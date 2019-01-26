using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.LoadBalancing;
using Leopotam.Ecs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client
{
    [EcsInject]
    public class ServerSpawnAsteroidSystem : IEcsRunSystem, IEcsInitSystem
    {
        public GameState GameState;
        public GameConfig GameConfig;

        public PhotonServer photonServer;

        public SceneDescription sceneDescription;

        private readonly List<AsteroidDesc> _asteroids = new List<AsteroidDesc> ();

        public static readonly RaiseEventOptions All = new RaiseEventOptions ()
        {
            Receivers = ReceiverGroup.All
        };

        private readonly Hashtable _hashtable = new Hashtable ();
        public void Run ()
        {
            var time = Time.realtimeSinceStartup;
            var lastGenerationTime = GameState.lastGenerationTime;
            var timeFromLastGeneration = time - lastGenerationTime;
            if (timeFromLastGeneration > GameConfig.GenerationInterval)
            {
                var lastId = photonServer.CurrentRoom.CustomProperties.ContainsKey (RoomDataConstants.LastGeneratedId) ? (int) photonServer.CurrentRoom.CustomProperties[RoomDataConstants.LastGeneratedId] : 0;

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
                Debug.Log ($"Want generate: {_asteroids.Count}");
                _hashtable.Clear ();
                _hashtable[RoomDataConstants.LastGeneratedId] = lastId;
                photonServer.CurrentRoom.SetCustomProperties (_hashtable);
                photonServer.OpRaiseEvent (GameEventCode.SpawnAsteroids, _asteroids.ToArray(), true, All);
            }
        }

        public void Initialize ()
        {
            PhotonPeer.RegisterType (typeof (AsteroidDesc), (byte)
                'd', SerializeAsteroidDesc, DeserializeAsteroidDesc);
        }

        public static readonly byte[] memAsteroidDesc = new byte[4 * 10];
        private static short SerializeAsteroidDesc (StreamBuffer outStream, object customobject)
        {
            AsteroidDesc o = (AsteroidDesc) customobject;

            lock (memAsteroidDesc)
            {
                byte[] bytes = memAsteroidDesc;
                int index = 0;
                Protocol.Serialize (o.Id, bytes, ref index); //4
                Protocol.Serialize (o.Index, bytes, ref index); //4
                Protocol.Serialize (o.Position.x, bytes, ref index); //4
                Protocol.Serialize (o.Position.y, bytes, ref index); //4
                Protocol.Serialize (o.Position.z, bytes, ref index); //4
                Protocol.Serialize (o.Rotation.x, bytes, ref index); //4
                Protocol.Serialize (o.Rotation.y, bytes, ref index); //4
                Protocol.Serialize (o.Rotation.z, bytes, ref index); //4
                Protocol.Serialize (o.Rotation.w, bytes, ref index); //4
                Protocol.Serialize (o.Speed, bytes, ref index); //4
                outStream.Write (bytes, 0, 4 * 10);
            }

            return 4 * 10;
        }

        private static object DeserializeAsteroidDesc (StreamBuffer inStream, short length)
        {
            AsteroidDesc o = new AsteroidDesc ();

            lock (memAsteroidDesc)
            {
                inStream.Read (memAsteroidDesc, 0, 4 * 10);
                int index = 0;
                Protocol.Deserialize (out o.Id, memAsteroidDesc, ref index); //4
                Protocol.Deserialize (out o.Index, memAsteroidDesc, ref index); //4
                var position = new Vector3 ();
                Protocol.Deserialize (out position.x, memAsteroidDesc, ref index); //4
                Protocol.Deserialize (out position.y, memAsteroidDesc, ref index); //4
                Protocol.Deserialize (out position.z, memAsteroidDesc, ref index); //4
                o.Position = position;

                var rotation = new Quaternion ();

                Protocol.Deserialize (out rotation.x, memAsteroidDesc, ref index); //4
                Protocol.Deserialize (out rotation.y, memAsteroidDesc, ref index); //4
                Protocol.Deserialize (out rotation.z, memAsteroidDesc, ref index); //4
                Protocol.Deserialize (out rotation.w, memAsteroidDesc, ref index); //4

                o.Rotation = rotation;

                Protocol.Deserialize (out o.Speed, memAsteroidDesc, ref index); //4
            }

            return o;
        }
        public void Destroy ()
        {

        }
    }
}
