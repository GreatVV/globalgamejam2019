using Leopotam.Ecs;

namespace Client
{
    [EcsInject]
    public class ServerSystem : IEcsRunSystem, IEcsInitSystem
    {
        public PhotonServer PhotonServer;

        public void Run()
        {
            if (PhotonServer.CurrentRoom != null && PhotonServer.LocalPlayer.IsMasterClient)
            {
                //todo server simulation
            }
        }

        public void Initialize()
        {
        }

        public void Destroy()
        {
        }
    }
}