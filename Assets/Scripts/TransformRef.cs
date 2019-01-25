using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    internal class TransformRef : IEcsAutoResetComponent
    {
        public Transform value;

        public void Reset()
        {
            value = null;
        }
    }
}