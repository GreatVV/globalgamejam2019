using Leopotam.Ecs;

namespace Client
{
    [EcsOneFrame]
    public class KillAsteroids : IEcsAutoResetComponent
    {
        public int[] value;
        public bool WithEffect;

        public void Reset ()
        {
            value = null;
            WithEffect = false;
        }
    }
}
