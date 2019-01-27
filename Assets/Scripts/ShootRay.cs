using Leopotam.Ecs;

namespace Client
{
    [EcsOneFrame]
    public class ShootRay : IEcsAutoResetComponent
    {
        public int AsteroidTarget = -1;

        public void Reset ()
        {
            AsteroidTarget = -1;
        }
    }
}
