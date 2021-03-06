﻿using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.LoadBalancing;
using Leopotam.Ecs;
using Photon.Pun;
using UnityEngine;

namespace Client
{
    [EcsInject]
    public class ConnectToPhotonSystem : IEcsInitSystem, IEcsRunSystem
    {
        public EcsWorld _world;
        public PhotonServer PhotonServer;
        public PlayerCache PlayerCache;

        private GameState _gameState;

        private SceneDescription _sceneDescription;

        private GameConfig _gameConfig;

        public void Destroy ()
        {
            PhotonServer.OnStateChangeAction -= OnStateChanged;
            PhotonServer.OnEventAction -= OnEventAction;
            PhotonServer.OnOpResponseAction -= OnOperationResponse;
            PhotonServer.Disconnect ();
        }

        public void Initialize ()
        {
            CustomTypes.Register ();
            PhotonServer.OnStateChangeAction += OnStateChanged;
            PhotonServer.OnEventAction += OnEventAction;
            PhotonServer.OnOpResponseAction += OnOperationResponse;
        }

        private void OnOperationResponse (OperationResponse obj)
        {
            UnityEngine.Debug.Log ("Response: " + obj.OperationCode + " : " + obj.ReturnCode + " : " + obj.Parameters.ToStringFull ());

            switch (obj.OperationCode)
            {
                case OperationCode.CreateGame:
                case OperationCode.JoinGame:
                case OperationCode.JoinRandomGame:
                    if (obj.ReturnCode == ErrorCode.Ok)
                    {
                        //CreatePlayer(PhotonServer.LocalPlayer.ID);
                        if (PhotonServer.CurrentRoom != null)
                        {
                            var data = PhotonServer.CurrentRoom.CustomProperties;
                            UnityEngine.Debug.Log ($"Set Properties on connect: {data.ToStringFull()}");
                            _world.CreateEntityWith<RoomData> (out var roomData);
                            roomData.value = data;

                            foreach (var player in PhotonServer.CurrentRoom.Players)
                            {
                                CreatePlayer (player.Key);
                            }

                            if (PhotonServer.LocalPlayer.IsMasterClient && !data.ContainsKey (RoomDataConstants.Health))
                            {
                                var hashtable = new Hashtable ();
                                hashtable[RoomDataConstants.Health] = _gameConfig.DefaultHealth;
                                PhotonServer.CurrentRoom.SetCustomProperties (hashtable);
                            }

                            if (data.ContainsKey (RoomDataConstants.Asteroids))
                            {
                                var newAsteroids = data[RoomDataConstants.Asteroids] as AsteroidDesc[];
                                _world.CreateEntityWith<SpawnAsteroids> (out var spawnAsteroids);
                            }

                            _sceneDescription.UI.GameUI.gameObject.SetActive (true);
                        }
                    }
                    else
                    {
                        if (obj.ReturnCode == 32760)
                        {
                            PhotonServer.CreateRandomRoom ();
                        }
                    }
                    break;
                case OperationCode.SetProperties:
                    {
                        var data = PhotonServer.CurrentRoom.CustomProperties;
                        UnityEngine.Debug.Log ($"Set Properties: {data.ToStringFull()}");
                        _world.CreateEntityWith<RoomData> (out var roomData);
                        roomData.value = data;
                    }
                    break;
            }
        }

        private void OnEventAction (EventData obj)
        {
            UnityEngine.Debug.Log ("Event: " + obj.Code + " " + obj.Parameters.ToStringFull ());

            switch (obj.Code)
            {
                case EventCode.Join:
                    CreatePlayer ((int) obj.Parameters[ParameterCode.ActorNr]);
                    break;
                case EventCode.Leave:
                    DeletePlayer ((int) obj.Parameters[ParameterCode.ActorNr]);
                    if (obj.Parameters.ContainsKey (ParameterCode.MasterClientId))
                    {
                        var masterClient = (int) obj.Parameters[ParameterCode.MasterClientId];
                        if (masterClient == PhotonServer.LocalPlayer.ID)
                        {
                            var gamePlayer = _world.GetComponent<GamePlayer> (PlayerCache.Entities[masterClient]);
                            if (!gamePlayer.isMaster)
                            {
                                _world.CreateEntityWith<MakeMaster> (out _);
                                gamePlayer.isMaster = true;
                            }
                        }
                    }
                    break;
                case EventCode.PropertiesChanged:
                    if (obj.Parameters.ContainsKey (ParameterCode.TargetActorNr) && (int) obj.Parameters[ParameterCode.TargetActorNr] == 0 && obj.Parameters.ContainsKey (ParameterCode.Properties))
                    {
                        //todo update global state
                        var data = obj.Parameters[ParameterCode.Properties] as Hashtable;
                        _world.CreateEntityWith<RoomData> (out var roomData);
                        roomData.value = data;
                    }
                    break;
                case GameEventCode.ChangeRole:
                    {
                        var actorNumber = (int) obj.Parameters[ParameterCode.ActorNr];
                        var newRole = (PlayerRole) obj.Parameters[ParameterCode.Data];
                        var otherPlayer = _gameState.GetPlayerWithRole (newRole);
                        if (otherPlayer == -1)
                        {
                            AssignRoleSystem.SetPlayerToRole (PhotonServer, _gameState, actorNumber, newRole);
                        }

                    }
                    break;
                case GameEventCode.SpawnAsteroids:
                    {
                        var newAsteroids = obj.Parameters[ParameterCode.Data] as AsteroidDesc[];
                        _world.CreateEntityWith<SpawnAsteroids> (out var spawnAsteroids);
                        spawnAsteroids.value = newAsteroids;
                    }
                    break;
                case GameEventCode.KillAsteroids:
                    {
                        var asteroidIds = obj.Parameters[ParameterCode.Data] as int[];
                        _world.CreateEntityWith<KillAsteroids> (out var killAsteroids);
                        killAsteroids.value = asteroidIds;
                    }
                    break;
                case GameEventCode.DamageByAsteroids:
                    {
                        var asteroidIds = obj.Parameters[ParameterCode.Data] as int[];
                        _world.CreateEntityWith<KillAsteroids> (out var killAsteroids);
                        killAsteroids.value = asteroidIds;
                        killAsteroids.WithEffect = true;
                    }
                    break;
                case GameEventCode.ShootAsteroid:
                    {
                        var asteroidId = (int) obj.Parameters[ParameterCode.Data];
                        if (asteroidId != -1)
                        {
                            _world.CreateEntityWith<KillAsteroids> (out var killAsteroids);
                            killAsteroids.value = new [] { asteroidId };
                            killAsteroids.WithEffect = true;
                        }

                        _world.CreateEntityWith<ShootRay> (out var shootRay);
                        shootRay.AsteroidTarget = asteroidId;
                    }
                    break;
                case GameEventCode.ChangeShootCamera:
                    {
                        var target = (Vector3) obj.Parameters[ParameterCode.Data];
                        _world.CreateEntityWith<ChangeShootTarget> (out var shootTarget);
                        shootTarget.value = target;
                    }
                    break;

            }
        }

        private void DeletePlayer (int actorNumber)
        {
            if (PlayerCache.Entities.TryGetValue (actorNumber, out var value))
            {
                _world.AddComponent<Leave> (value);
                _world.MarkComponentAsUpdated<Leave> (value);
            }
        }

        private void CreatePlayer (int actorNumber)
        {
            if (actorNumber < 0)
            {
                return;
            }

            if (!PlayerCache.Entities.ContainsKey (actorNumber))
            {
                var player = _world.CreateEntity ();
                _world.AddComponent<GamePlayer> (player).number = actorNumber;

                PlayerCache.Entities[actorNumber] = player;

                if (PhotonServer.LocalPlayer.ID == actorNumber)
                {
                    _world.AddComponent<Local> (player);
                }
            }

        }

        private void OnStateChanged (ClientState state)
        {
            UnityEngine.Debug.Log ("State: " + state);
            switch (state)
            {
                case ClientState.Disconnected:
                    PhotonServer.CallConnect ();
                    break;
                case ClientState.PeerCreated:
                    break;
                case ClientState.Authenticating:
                    _sceneDescription.UI.MenuUI.gameObject.SetActive (false);
                    break;
                case ClientState.Authenticated:
                    break;
                case ClientState.JoinedLobby:
                    //PhotonServer.ConnectToRandomRoom();
                    break;
                case ClientState.DisconnectingFromMasterserver:
                    break;
                case ClientState.ConnectingToGameserver:
                    break;
                case ClientState.ConnectedToGameserver:
                    break;
                case ClientState.Joining:
                    break;
                case ClientState.Joined:
                    break;
                case ClientState.Leaving:
                    break;
                case ClientState.DisconnectingFromGameserver:
                    break;
                case ClientState.ConnectingToMasterserver:
                    break;
                case ClientState.Disconnecting:
                    break;
                case ClientState.ConnectedToMasterserver:
                    PhotonServer.ConnectToRandomRoom ();
                    break;
                case ClientState.ConnectingToNameServer:
                    break;
                case ClientState.ConnectedToNameServer:

                    break;
                case ClientState.DisconnectingFromNameServer:
                    break;
            }
        }

        public void Run ()
        {
            PhotonServer.Service ();
        }
    }
}
