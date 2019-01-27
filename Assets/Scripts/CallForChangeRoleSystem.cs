using System;
using ExitGames.Client.Photon.LoadBalancing;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    internal class CallForChangeRoleSystem : IEcsRunSystem
    {
        private PhotonServer _photonServer;
        private GameState _gameState;
        public void Run ()
        {
            if (Input.GetKeyDown (KeyCode.Keypad1) || Input.GetKeyDown (KeyCode.Alpha1))
            {
                Try (PlayerRole.Fly);
            }
            else
            {
                if (Input.GetKeyDown (KeyCode.Keypad2)|| Input.GetKeyDown (KeyCode.Alpha2))
                {
                    Try (PlayerRole.Shoot);
                }
            }
        }

        private readonly RaiseEventOptions toServerOnly = new RaiseEventOptions ()
        {
            Receivers = ReceiverGroup.MasterClient
        };

        private void Try (PlayerRole newRole)
        {
            if (_photonServer.CurrentRoom != null && _photonServer.LocalPlayer != null)
            {
                var id = _photonServer.LocalPlayer.ID;
                if (_gameState.Roles.TryGetValue (id, out var role))
                {
                    if (role != newRole)
                    {
                        var playerWithRole = _gameState.GetPlayerWithRole (newRole);
                        if (playerWithRole == -1)
                        {
                            _photonServer.OpRaiseEvent (GameEventCode.ChangeRole, (byte) newRole, true, toServerOnly);
                        }
                    }
                }
            }
        }
    }
}
