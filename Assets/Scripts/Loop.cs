using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client
{
    sealed class Loop : MonoBehaviour
    {
        EcsWorld _world;
        EcsSystems _systems;

        public SceneDescription SceneDescription;

        public GameConfig GameConfig;

        private EcsSystems _fixedUpdateSystems;
        private EcsSystems _lateUpdateSystems;

        public SoundManager SoundManager;

        void OnEnable ()
        {
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
            _fixedUpdateSystems = new EcsSystems (_world);
            _lateUpdateSystems = new EcsSystems (_world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_fixedUpdateSystems);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_lateUpdateSystems);
#endif
            var playerCache = new PlayerCache ();
            var photonServer = new PhotonServer ();
            var gameState = new GameState ();

            var ship = SceneDescription.CreateShip (_world);
            gameState.ShipEntity = ship;

            SceneDescription.UI.MenuUI.gameObject.SetActive (true);

            _systems
                // Register your systems here, for example:
                .Add (SceneDescription.UI.MenuUI)
                .Add (SceneDescription.Radar)
                .Add (new ConnectToPhotonSystem ())
                .Add (new ServerSystem ())
                .Add (new LocalPlayerSystem ())
                .Add (new UpdateShipPositionSystem ())
                .Add (new ShowRolesSystem ())
                .Add (new MakeMasterSystem ())
                .Add (new CallForChangeRoleSystem ())
                .Add (new ControlShootSystem ())
                .Add (new UpdateHealthSystem ())
                .Add (new SpawnAsteroidsSystem ())
                .Add (new ChangeRoleSystem ())
                .Add (new UpdateCannonDirectionSystem ())
                .Add (new SpawnWeaponRaysSystem ())
                .Add (new KillAsteroidSystem ())
                .Add (new AsteroidMoveSystem ())
                .Add (new LerpPositionSystem ())
                .Add (new LerpRotationSystem ())
                .Add (new ShootSystem ())
                .Add (new DestroyTransformSystem ())
                .Inject (GameConfig)
                .Inject (SceneDescription)
                .Inject (playerCache)
                .Inject (photonServer)
                .Inject (gameState)
                .Inject( SoundManager)
                .Initialize ();

            _fixedUpdateSystems
                .Add (new ControlShipSystem ())
                .Inject (GameConfig)
                .Inject (SceneDescription)
                .Inject (playerCache)
                .Inject (photonServer)
                .Inject (gameState)
                .Initialize ();

            _lateUpdateSystems
                .Add (new SyncShipPosition ())
                .Add (new SyncRigidbodyPositionToShipSystem ())
                .Inject (GameConfig)
                .Inject (SceneDescription)
                .Inject (playerCache)
                .Inject (photonServer)
                .Inject (gameState)
                .Initialize ();
        }

        void Update ()
        {
            _systems.Run ();
        }

        private void FixedUpdate ()
        {
            _fixedUpdateSystems.Run ();
        }

        private void LateUpdate ()
        {
            _lateUpdateSystems.Run ();
            _world.RemoveOneFrameComponents ();
        }

        void OnDisable ()
        {
            _systems.Dispose ();
            _systems = null;

            _fixedUpdateSystems.Dispose ();
            _fixedUpdateSystems = null;

            _lateUpdateSystems.Dispose ();
            _lateUpdateSystems = null;

            _world.Dispose ();
            _world = null;
        }
    }
}
