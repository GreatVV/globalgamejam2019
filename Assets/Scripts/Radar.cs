using UnityEngine;

namespace Client
{
    public class Radar : MonoBehaviour
    {
        public void Show (bool state)
        {
            gameObject.SetActive (state);
        }
    }
}
