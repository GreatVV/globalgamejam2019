using System;
using ExitGames.Client.Photon;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    internal class ControlShipSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private PlayerCache _playerCache;
        private GameState _gameState;

        private PhotonServer _photonServer;
        public void Run ()
        {
            if (_photonServer.CurrentRoom != null)
            {
                var localPlayer = _photonServer.LocalPlayer;
                if (_playerCache.Entities.TryGetValue (localPlayer.ID, out var playerEntity))
                {
                    var role = _world.GetComponent<Role> (playerEntity);
                    if (role != null)
                    {
                        if (role.value == PlayerRole.Fly)
                        {

                            DoControl ();

                        }
                    }
                }
            }
        }

        private readonly Hashtable _hashtable = new Hashtable ();

        private void DoControl ()
        {
            _hashtable.Clear ();
            var vertical = Input.GetAxis ("Vertical");
            if (vertical != 0)
            {
                var dt = UnityEngine.Time.deltaTime;
                var speed = _world.GetComponent<Speed> (_gameState.ShipEntity).value;
                var transform = _world.GetComponent<TransformRef> (_gameState.ShipEntity).value;

                var newPosition = transform.position + transform.forward * dt * speed * vertical;
                _hashtable[RoomDataConstants.ShipPosition] = newPosition;

                transform.position = newPosition;
            }

            var horizontal = Input.GetAxis ("Horizontal");
            if (horizontal != 0)
            {
                var dt = UnityEngine.Time.deltaTime;
                var rotationSpeed = _world.GetComponent<RotationSpeed> (_gameState.ShipEntity).value;
                var transform = _world.GetComponent<TransformRef> (_gameState.ShipEntity).value;

                var newRotation = transform.rotation * Quaternion.AngleAxis (dt * rotationSpeed * horizontal, Vector3.up);

                _hashtable[RoomDataConstants.ShipRotation] = newRotation;
                transform.rotation = newRotation;
            }

            if (_hashtable.Count > 0)
            {
                _photonServer.CurrentRoom.SetCustomProperties (_hashtable);
            }
        }
    }
}
