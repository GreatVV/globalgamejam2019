using UnityEngine;
using UnityEngine.Playables;

namespace Client
{
    public class WeaponRay : MonoBehaviour
    {
        public float LifeTime = 0.4f;
        public PlayableDirector ShootAnimation;

        public LineRenderer LineRenderer;

        private float _timeLeft;

        public void Set (Vector3 start, Vector3 end)
        {
            LineRenderer.SetPosition (0, start);
            LineRenderer.SetPosition (1, end);
            _timeLeft = LifeTime;
            LineRenderer.enabled = true;
            ShootAnimation.Play ();
        }

        void Update ()
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft < 0)
            {
                LineRenderer.enabled = false;
            }
        }
    }
}
