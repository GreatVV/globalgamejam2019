using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    internal class MakeMasterSystem : EcsReactiveSystem<MakeMaster>
    {
        private PhotonServer _photonServer;
        private GameState _gameState;

        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive ()
        {
            if (_photonServer.CurrentRoom != null && _photonServer.LocalPlayer.IsMasterClient)
            {
                var temp = Extensions.DeserializeToPlayerRoles (_photonServer.CurrentRoom.CustomProperties[RoomDataConstants.PlayerRole] as byte[]);
                _gameState.Roles.Clear ();
                foreach (var item in temp)
                {
                    _gameState.Roles.Add(item.Key, item.Value);
                }
            }
        }
    }
}
