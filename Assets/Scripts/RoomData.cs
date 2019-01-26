using ExitGames.Client.Photon;
using Leopotam.Ecs;

namespace Client
{
    [EcsOneFrame]
    public class RoomData : IEcsAutoResetComponent
    {
        public Hashtable value;
        public void Reset()
        {
            value = null;
        }
    }
}