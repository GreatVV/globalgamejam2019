using Leopotam.Ecs;
using UnityEngine;

namespace Client {
    sealed class Loop : MonoBehaviour {
        EcsWorld _world;
        EcsSystems _systems;


        void OnEnable () {
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif
            var playerCache = new PlayerCache();
            var photonServer = new PhotonServer();

            _systems
                // Register your systems here, for example:
                .Add (new ConnectToPhotonSystem())
                .Add(new CreateTestPlayer())
                .Add(new RemoveTestPlayer())
                .Add(new ServerSystem())
                .Add(new LocalPlayerSystem())
                // .Add (new TestSystem2())
                .Inject(playerCache)
                .Inject(photonServer)
.Initialize ();
        }

        void Update () {
            _systems.Run ();
            // Optional: One-frame components cleanup.
            _world.RemoveOneFrameComponents();
        }

        void OnDisable () {
            _systems.Dispose ();
            _systems = null;
            _world.Dispose ();
            _world = null;
        }
    }
}