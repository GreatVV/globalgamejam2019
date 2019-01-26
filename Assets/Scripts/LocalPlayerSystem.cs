using Leopotam.Ecs;

namespace Client
{
    [EcsInject]
    public class LocalPlayerSystem : IEcsRunSystem, IEcsInitSystem
    {
        private PhotonServer PhotonServer;
        private EcsWorld _world;

        private EcsSystems _systems;
        private SceneDescription SceneDescription;
        private PlayerCache PlayerCache;

        public void Run()
        {
            if (PhotonServer.CurrentRoom != null && !PhotonServer.LocalPlayer.IsMasterClient)
            {
            }
        }

        public void Initialize()
        {
            _systems = new EcsSystems(_world);

            _systems.Add(new UpdateNonServerShipPositionSystem())
                .Inject(PhotonServer)
                .Inject(SceneDescription)
                .Inject(PlayerCache)
                .Initialize()
                ;
        }

        public void Destroy()
        {
            _systems.Dispose();
        }
    }


}