using System;
using System.Collections.Generic;
using System.Diagnostics;
using ExitGames.Client.Photon;
using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    public class AssignRoleSystem : EcsReactiveSystem<GamePlayer>
    {
        private EcsWorld _world;
        private PhotonServer _photonServer;
        private GameState _gameState;
        private static readonly Hashtable _hashtable = new Hashtable ();

        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive ()
        {
            for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var player = _world.GetComponent<GamePlayer> (entity);
                if (_world.GetComponent<Leave> (entity) != null)
                {
                    continue;
                }

                var players = _photonServer.CurrentRoom.Players;

                var photonPlayer = players[player.number];

                var values = Enum.GetValues (typeof (PlayerRole)) as PlayerRole[];
                if (photonPlayer.CustomProperties.ContainsKey (RoomDataConstants.PlayerRole))
                {
                    _gameState.Roles[player.number] = (PlayerRole) photonPlayer.CustomProperties[RoomDataConstants.PlayerRole];
                }
                else
                {
                    var freeRole = _gameState.NextFreeRole ();
                    SetPlayerToRole (_photonServer, _gameState, player.number, freeRole);
                    
                }
            }           
        }

        public static void SetPlayerToRole (PhotonServer photonServer, GameState gameState, int actorNumber, PlayerRole newRole)
        {
            var photonPlayer = photonServer.CurrentRoom.Players[actorNumber];
            _hashtable.Clear ();
            _hashtable[RoomDataConstants.PlayerRole] = (int) newRole;
            photonPlayer.SetCustomProperties (_hashtable);
            gameState.Roles[actorNumber] = newRole;
            UnityEngine.Debug.Log ($"Assign player {actorNumber} to {newRole}");

             _hashtable.Clear ();
                _hashtable[RoomDataConstants.PlayerRole] = gameState.Roles.Serialize ();
                photonServer.CurrentRoom.SetCustomProperties (_hashtable);
        }
    }

    public static class Extensions
    {
        public static byte[] Serialize (this Dictionary<int, PlayerRole> value)
        {
            var array = new byte[value.Count * 2];

            var i = 0;
            foreach (var pair in value)
            {
                array[i * 2] = (byte) pair.Key;
                array[i * 2 + 1] = (byte) pair.Value;
                i++;
            }

            return array;
        }

        public static Dictionary<int, PlayerRole> DeserializeToPlayerRoles (byte[] array)
        {
            var gameState = new Dictionary<int, PlayerRole> ();
            for (int i = 0; i < array.Length / 2; i++)
            {
                var playerNumber = (int) array[i * 2];
                var playerRole = (PlayerRole) array[i * 2 + 1];
                gameState[playerNumber] = playerRole;
            }

            return gameState;
        }
    }
}
