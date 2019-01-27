using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    public class Radar : MonoBehaviour, IEcsSystem
    {
        private GameState _gameState;
        private EcsWorld _world;

        public void Show (bool state)
        {
            gameObject.SetActive (state);
        }

        void Update ()
        {
            foreach (var asteroid in _gameState.Asteroids)
            {
                var entity = asteroid.Value;
                var asteroidTransform = _world.GetComponent<TransformRef> (entity).value;

            }
        }
    }
}
