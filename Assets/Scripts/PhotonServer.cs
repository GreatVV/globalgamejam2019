using System;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.LoadBalancing;

namespace Client
{
    public class PhotonServer : LoadBalancingClient
    {
        public bool CallConnect ()
        {
            this.AppId = "a5c87f84-ff33-42c1-9b8f-4dfeba3751e7"; // set your app id here
            this.AppVersion = "1.0"; // set your app version here

            // "eu" is the European region's token
            if (!this.ConnectToRegionMaster ("eu")) // can return false for errors
            {
                this.DebugReturn (DebugLevel.ERROR, "Can't connect to: " + this.CurrentServerAddress);
                return false;
            }
            return true;
        }

        internal void ConnectToRandomRoom ()
        {
            //OpJoinOrCreateRoom ("smoothies", new RoomOptions(), TypedLobby.Default);
            OpJoinRandomRoom(null, 4);
        }

        public void CreateRandomRoom ()
        {
            UnityEngine.Debug.Log ("Create Random room");
            if (!OpJoinOrCreateRoom (Guid.NewGuid ().ToString (), new RoomOptions ()
                {
                    MaxPlayers = 4,

                }, TypedLobby.Default))
            {
                UnityEngine.Debug.Log ("Can't connect: ");
            }
        }
    }
}
