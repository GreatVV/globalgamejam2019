using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    public class MenuUI : MonoBehaviour, IEcsSystem
    {
        private PhotonServer _photonServer;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
        }
        public void OnStartClick ()
        {
            _photonServer.CallConnect ();
        }
    }
}
