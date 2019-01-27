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

            var transform = _world.GetComponent<TransformRef> (_gameState.ShipEntity).value;
            var rigidBody = transform.gameObject.GetComponent<Rigidbody> ();

            float TurnSpeed = 0.3f;
            float ForwardSpeed = 30.0f;

            Vector2 mousePos = Input.mousePosition;
            Vector2 centeredMousePos = mousePos - new Vector2 (Screen.width, Screen.height) / 2.0f;
            Debug.Log (centeredMousePos);

            float pitch = centeredMousePos.y * -1.0f;
            float yaw = centeredMousePos.x;
            float roll = 0.0f;

            rigidBody.velocity = transform.forward * ForwardSpeed;

            rigidBody.AddRelativeTorque (
                pitch * TurnSpeed * Time.deltaTime,
                yaw * TurnSpeed * Time.deltaTime,
                roll);
        }
    }
}
