using System.Collections.Generic;
using Leopotam.Ecs;

namespace Client
{
    [EcsOneFrame]
    internal class SpawnAsteroids : IEcsAutoResetComponent
    {
        public AsteroidDesc[] value;

        public void Reset ()
        {
            value = null;
        }
    }
}
