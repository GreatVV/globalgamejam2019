using UnityEngine;

namespace Client
{
    internal struct AsteroidDesc
    {
        public long Id;
        internal Vector3 Position;
        internal Quaternion Rotation;
        internal float Speed;
        internal int Index;
    }
}