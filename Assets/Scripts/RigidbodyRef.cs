using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    internal class RigidbodyRef : IEcsAutoResetComponent
    {
        public Rigidbody value;

        public void Reset ()
        {
            value = null;
        }
    }
    
    
}
