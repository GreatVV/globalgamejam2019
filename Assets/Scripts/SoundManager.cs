using System;
using UnityEngine;

namespace Client
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _shotSound = null;
        [SerializeField] private AudioSource _collisionSound = null;

        public void PlayShootSound()
        {
            _shotSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            _shotSound.Play();
        }

        public void PlayCollisionSound()
        {
            _collisionSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            _collisionSound.Play();
        }

        public void PlayDeathSound(Vector3 position) {
            
        }
    }
}