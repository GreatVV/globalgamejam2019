using System;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class ShipView : Injectable
    {
        public Rigidbody Rigidbody;

        public CannonView[] Cannons;

        void OnValidate ()
        {
            if (!Rigidbody)
            {
                Rigidbody = GetComponent<Rigidbody> ();
            }
        }

        private void OnTriggerEnter (Collider other)
        {
            var asteroid = other.gameObject.GetComponent<AsteroidView> ();
            if (asteroid)
            {
                World.CreateEntityWith<AsteroidCollision> (out var collision);
                collision.AsteroidEntity = asteroid.Entity;
            }
        }
    }

}
