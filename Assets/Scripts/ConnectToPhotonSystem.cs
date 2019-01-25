using System;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.LoadBalancing;
using Leopotam.Ecs;

namespace Client
{
    [EcsInject]
    public class ConnectToPhotonSystem : IEcsInitSystem, IEcsRunSystem
    {
        public EcsWorld _world;
        public PhotonServer PhotonServer;
        public PlayerCache PlayerCache;

        public void Destroy()
        {
            PhotonServer.OnStateChangeAction -= OnStateChanged;
            PhotonServer.OnEventAction -= OnEventAction;
            PhotonServer.OnOpResponseAction -= OnOperationResponse;
            PhotonServer.Disconnect();
        }

        public void Initialize()
        {
            PhotonServer = new PhotonServer();
            PhotonServer.CallConnect();
            PhotonServer.OnStateChangeAction += OnStateChanged;
            PhotonServer.OnEventAction += OnEventAction;
            PhotonServer.OnOpResponseAction += OnOperationResponse;
        }

        private void OnOperationResponse(OperationResponse obj)
        {
            UnityEngine.Debug.Log("Response: "+obj.OperationCode + " : "+obj.ReturnCode);

            switch(obj.OperationCode)
            {
                case OperationCode.CreateGame:
                case OperationCode.JoinGame:
                case OperationCode.JoinRandomGame:
                    if (obj.ReturnCode == ErrorCode.Ok)
                    {
                        //CreatePlayer(PhotonServer.LocalPlayer.ID);
                        if (PhotonServer.CurrentRoom != null)
                        {
                            foreach (var player in PhotonServer.CurrentRoom.Players)
                            {
                                CreatePlayer(player.Key);
                            }
                        }
                    }
                    else
                    {
                        if (obj.ReturnCode == 32760)
                        {
                            PhotonServer.CreateRandomRoom();
                        }
                    }
                    break;
            }
        }

        private void OnEventAction(EventData obj)
        {
            UnityEngine.Debug.Log("Event: " + obj.Code + " "+obj.Parameters);

            switch (obj.Code)
            {
                case EventCode.Join:
                    CreatePlayer((int)obj.Parameters[ParameterCode.ActorNr]);
                    break;
                case EventCode.Leave:
                    DeletePlayer((int)obj.Parameters[ParameterCode.ActorNr]);
                    break;                    
            }
        }

        private void DeletePlayer(int sender)
        {
            if (PlayerCache.Entities.TryGetValue(sender, out var value))
            {
                _world.AddComponent<Leave>(value);
                _world.MarkComponentAsUpdated<Leave>(value);
            }
        }

        private void CreatePlayer(int sender)
        {
            if (sender < 0)
            {
                return;
            }

            if (!PlayerCache.Entities.ContainsKey(sender))
            {
                var player = _world.CreateEntity();
                _world.AddComponent<GamePlayer>(player).number = sender;
                
                PlayerCache.Entities[sender] = player;
            }
        }

        private void OnStateChanged(ClientState state)
        {
            UnityEngine.Debug.Log("State: " + state);
            switch (state)
            {
                case ClientState.Disconnected:
                    PhotonServer.CallConnect();
                    break;
                case ClientState.PeerCreated:
                    break;
                case ClientState.Authenticating:
                    break;
                case ClientState.Authenticated:
                    break;
                case ClientState.JoinedLobby:
                    //PhotonServer.ConnectToRandomRoom();
                    break;
                case ClientState.DisconnectingFromMasterserver:
                    break;
                case ClientState.ConnectingToGameserver:
                    break;
                case ClientState.ConnectedToGameserver:
                    break;
                case ClientState.Joining:
                    break;
                case ClientState.Joined:
                    break;
                case ClientState.Leaving:
                    break;
                case ClientState.DisconnectingFromGameserver:
                    break;
                case ClientState.ConnectingToMasterserver:
                    break;
                case ClientState.Disconnecting:
                    break;
                case ClientState.ConnectedToMasterserver:
                    PhotonServer.ConnectToRandomRoom();
                    break;
                case ClientState.ConnectingToNameServer:
                    break;
                case ClientState.ConnectedToNameServer:
                    
                    break;
                case ClientState.DisconnectingFromNameServer:
                    break;
            }            
        }

        public void Run()
        {
            PhotonServer.Service();
        }
    }
}