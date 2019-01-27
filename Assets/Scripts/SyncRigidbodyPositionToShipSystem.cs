using ExitGames.Client.Photon;
using Leopotam.Ecs;

namespace Client
{
    [EcsInject]
    public class SyncRigidbodyPositionToShipSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private PlayerCache _playerCache;
        private GameState _gameState;

        private PhotonServer _photonServer;
        private readonly Hashtable _hashtable = new Hashtable ();

        private EcsFilter<Local, Role> _player;
        public void Run ()
        {
            if (_photonServer.CurrentRoom != null)
            {
                if (_player.EntitiesCount > 0)
                {
                    var playerEntity = _player.Entities[0];
                    var role = _world.GetComponent<Role> (playerEntity);
                    if (role != null)
                    {
                        if (role.value == PlayerRole.Fly)
                        {
                            _hashtable.Clear ();
                            var transform = _world.GetComponent<TransformRef> (_gameState.ShipEntity).value;
                            _hashtable[RoomDataConstants.ShipPosition] = transform.position;
                            _hashtable[RoomDataConstants.ShipRotation] = transform.rotation;

                            _photonServer.CurrentRoom.SetCustomProperties (_hashtable);
                        }
                    }
                }
            }
        }
    }
}
