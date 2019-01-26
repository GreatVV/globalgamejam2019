using System;
using Leopotam.Ecs;

namespace Client
{
    [EcsInject]
    public class ServerSystem : IEcsRunSystem, IEcsInitSystem
    {
        private PhotonServer PhotonServer;
        private EcsWorld _world;

        private EcsSystems _systems;
        private SceneDescription SceneDescription;
        private PlayerCache PlayerCache;
        private GameState GameState;
        private GameConfig GameConfig;

        public void Run ()
        {
            if (PhotonServer.CurrentRoom != null && PhotonServer.LocalPlayer.IsMasterClient)
            {
                _systems.Run ();
            }
        }

        public void Initialize ()
        {
            _systems = new EcsSystems (_world);

            _systems
                .Add (new AssignRoleSystem ())
                .Add (new DeAssignRoleSystem ())
                .Add( new SpawnAsteroidSystem())
               .Inject(GameConfig)
                .Inject (SceneDescription)
                .Inject (PlayerCache)
                .Inject (PhotonServer)
                .Inject (GameState)
                .Initialize ();
        }

        public void Destroy ()
        {
            _systems.Dispose ();
        }
    }
}
