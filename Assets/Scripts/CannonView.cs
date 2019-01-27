using UnityEngine;

namespace Client
{
    public class CannonView : MonoBehaviour
    {
        public Transform RayStart;
        public Transform Root;
        public WeaponRay Ray;

        void OnValidate ()
        {
            if (!Root)
            {
                Root = transform;
            }
        }
    }
}
