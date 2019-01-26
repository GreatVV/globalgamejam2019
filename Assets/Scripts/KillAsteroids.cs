using Leopotam.Ecs;

namespace Client
{
    [EcsOneFrame]
    public class KillAsteroids : IEcsAutoResetComponent
    {
        public int[] value;

        public void Reset ()
        {
            value = null;
        }
    }
}
