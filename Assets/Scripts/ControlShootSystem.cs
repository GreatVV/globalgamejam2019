using System;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    internal class ControlShootSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private PlayerCache _playerCache;
        private GameState _gameState;

        private SceneDescription _sceneDescription;

        private PhotonServer _photonServer;

        private GameConfig _gameConfig;
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
                        if (role.value == PlayerRole.Shoot)
                        {
                            DoControl ();
                        }
                    }
                }
            }
        }

        private Vector3 _startPosition;

        private void DoControl ()
        {
            var shootCameraTransform = _sceneDescription.ShootPlayerCamera.transform;
            var euler = shootCameraTransform.eulerAngles;
            var x = euler.y;
            var y = euler.x;
            //Debug.Log ("Before: " + euler);
            var xSpeed = _gameConfig.xSpeed;
            var ySpeed = _gameConfig.xSpeed;

            x += Input.GetAxis ("Mouse X") * xSpeed * _gameConfig.ShootCameraDistance * 0.02f;
            y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;

            Quaternion rotation = Quaternion.Euler (y, x, 0);
            shootCameraTransform.rotation = rotation;

            var camera = _sceneDescription.GameCamera;
            var ray = camera.ViewportPointToRay (Vector2.one / 2f);
            var targetWorldPoint = ray.GetPoint (30);

            _photonServer.OpRaiseEvent (GameEventCode.ChangeShootCamera, targetWorldPoint, false, ServerSpawnAsteroidSystem.All);

            if (Input.GetMouseButtonDown (0))
            {
                _world.CreateEntityWith<WantShoot> (out _);
            }
        }
    }
}
