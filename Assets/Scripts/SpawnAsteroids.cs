using System.Collections.Generic;
using Leopotam.Ecs;

namespace Client
{
    [EcsOneFrame]
    internal class SpawnAsteroids : IEcsAutoResetComponent
    {
        public List<AsteroidDesc> value;

        public void Reset ()
        {
            value = null;
        }
    }
}
