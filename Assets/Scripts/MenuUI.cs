using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    public class MenuUI : MonoBehaviour, IEcsSystem
    {
        private PhotonServer _photonServer;
        public void OnStartClick ()
        {
            _photonServer.CallConnect ();
        }
    }
}
