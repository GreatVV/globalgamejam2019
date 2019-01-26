using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    public class ShowRolesSystem : EcsReactiveSystem<RoomData>
    {
        private EcsWorld _world;
        private SceneDescription SceneDescription;
        private PhotonServer _photonServer;
        private readonly StringBuilder stringBuilder = new StringBuilder ();

        private PlayerCache _playerCache;

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
                var roomData = _world.GetComponent<RoomData> (entity);
                if (roomData.value.ContainsKey (RoomDataConstants.PlayerRole))
                {
                    var roles = Extensions.DeserializeToPlayerRoles (roomData.value[RoomDataConstants.PlayerRole] as byte[]);
                    _gameState.UpdateRoles (roles);

                    foreach (var playerRole in roles)
                    {
                        var actorNumber = playerRole.Key;
                        var role = playerRole.Value;
                        if (_playerCache.Entities.TryGetValue (actorNumber, out var playerEntity))
                        {
                            var roleComponent = _world.EnsureComponent<Role> (playerEntity, out var isNew);
                            if (!isNew)
                            {
                                if (roleComponent.value != role)
                                {
                                    _world.MarkComponentAsUpdated<Role> (playerEntity);
                                }
                            } else {
                                _world.MarkComponentAsUpdated<Role> (playerEntity);
                            }
                            roleComponent.value = role;
                        }
                    }

                    stringBuilder.Clear ();
                    stringBuilder.AppendLine ("Roles");

                    var values = Enum.GetValues (typeof (PlayerRole)) as PlayerRole[];
                    for (int j = 1; j < values.Length; j++)
                    {
                        var role = values[j];
                        var playerWithRole = _gameState.GetPlayerWithRole (role);

                        stringBuilder.Append ($"{role} = {( playerWithRole == -1 ? "-" : playerWithRole.ToString() )}");
                        if (playerWithRole == _photonServer.LocalPlayer.ID)
                        {

                            stringBuilder.Append ("(me)");
                        }

                        stringBuilder.AppendLine ();
                    }

                    SceneDescription.UI.GameUI.Roles.text = stringBuilder.ToString ();
                }
            }
        }
    }
}
