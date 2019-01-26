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
        private readonly Hashtable _hashtable = new Hashtable ();

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
                var photonPlayer = _photonServer.CurrentRoom.Players[player.number];

                var values = Enum.GetValues (typeof (PlayerRole)) as PlayerRole[];
                for (int j = 1; j < values.Length; j++)
                {
                    if (!_gameState.Roles.ContainsKey (values[j]))
                    {
                        _hashtable.Clear ();
                        _hashtable[RoomDataConstants.PlayerRole] = photonPlayer.ID;
                        _gameState.Roles[(PlayerRole) j] = player.number;
                        photonPlayer.SetCustomProperties (_hashtable);

                        UnityEngine.Debug.Log ($"Assign player {player.number} to {(PlayerRole)j}");
                        break;
                    }
                }
            }

            _hashtable.Clear ();
            _hashtable[RoomDataConstants.PlayerRole] = _gameState.Roles.Serialize ();
            _photonServer.CurrentRoom.SetCustomProperties (_hashtable);
        }
    }

    public static class Extensions
    {
        public static byte[] Serialize (this Dictionary<PlayerRole, int> value)
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

        public static Dictionary<PlayerRole, int> DeserializeToPlayerRoles (byte[] array)
        {
            var gameState = new Dictionary<PlayerRole, int> ();
            for (int i = 0; i < array.Length / 2; i++)
            {
                var playerRole = (PlayerRole) array[i * 2];
                var playerNumber = array[i * 2 + 1];
                gameState[playerRole] = playerNumber;
            }

            return gameState;
        }
    }
}
