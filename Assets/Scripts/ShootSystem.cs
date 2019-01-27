using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;
using UnityEngine;

namespace Client
{
    [EcsInject]
    public class ShootSystem : EcsReactiveSystem<WantShoot>
    {
        private SceneDescription _sceneDescription;
        public EcsWorld _world;

        private PhotonServer _photonServer;
        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive ()
        {
            var camera = _sceneDescription.GameCamera;
            var ray = camera.ViewportPointToRay (Vector2.one / 2f);
            if (Physics.Raycast (ray, out var hitInfo))
            {
                var asteroidView = hitInfo.transform.GetComponent<AsteroidView> ();
                if (asteroidView)
                {
                    var asteroid = _world.GetComponent<Asteroid> (asteroidView.Entity);
                    _photonServer.OpRaiseEvent (GameEventCode.ShootAsteroid, asteroid.Id, true, ServerSpawnAsteroidSystem.All);
                }
            } else {
                _photonServer.OpRaiseEvent (GameEventCode.ShootAsteroid, -1, true, ServerSpawnAsteroidSystem.All);
            }
        }
    }
}
