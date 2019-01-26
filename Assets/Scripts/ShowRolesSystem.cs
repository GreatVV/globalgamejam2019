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
        private readonly StringBuilder stringBuilder = new StringBuilder();

        protected override EcsReactiveType GetReactiveType()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive()
        {
            for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var roomData = _world.GetComponent<RoomData>(entity);
                if (roomData.value.ContainsKey(RoomDataConstants.PlayerRole))
                {
                    UnityEngine.Debug.Log("Update roles");
                    var roles = Extensions.DeserializeToPlayerRoles(roomData.value[RoomDataConstants.PlayerRole] as byte[]);

                    stringBuilder.Clear();
                    stringBuilder.AppendLine("Roles");

                    foreach (var role in roles)
                    {
                        stringBuilder.Append($"{role.Key} = {role.Value}");
                        if (role.Value == _photonServer.LocalPlayer.ID)
                        {
                            stringBuilder.Append("(me)");
                        }

                        stringBuilder.AppendLine();
                    }

                    SceneDescription.UI.GameUI.Roles.text = stringBuilder.ToString();
                }
            }
        }
    }
}