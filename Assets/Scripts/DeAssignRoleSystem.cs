using ExitGames.Client.Photon;
using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    public class DeAssignRoleSystem : EcsUpdateReactiveSystem<Leave>
    {
        private EcsWorld _world;
        private PhotonServer _photonServer;
        private GameState _gameState;
        private readonly Hashtable _hashtable = new Hashtable();
      
        protected override void RunUpdateReactive()
        {
             for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var player = _world.GetComponent<GamePlayer>(entity);

                foreach (var stateRole in _gameState.Roles)
                {
                    if (stateRole.Key == player.number)
                    {
                        _gameState.Roles.Remove(stateRole.Key);
                        break;
                    }
                }
            }

            _hashtable.Clear();
            _hashtable[RoomDataConstants.PlayerRole] = _gameState.Roles.Serialize();
            _photonServer.CurrentRoom.SetCustomProperties(_hashtable);
        }
    }
}