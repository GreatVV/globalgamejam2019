using UnityEngine;

namespace Client
{
    [CreateAssetMenu]
    public class GameConfig : ScriptableObject
    {
        public float DefaultHealth = 100;
        public float GenerationInterval = 10;
        public int MaxAsteroidsPerGeneration = 1;
        public int MinAsteroidPerGeneration = 5;
        public float MaxGenerationRadius = 30;
        public float MinGenerationRadius = 100;
        public float MaxAsteroidSpeed = 10;
        public float MinAsteroidSpeed = 30;
        public GameObject[] AsteroidsPrefabs;
        public float AsteroidDumpInterval = 15;
        public float DeathDistance = 200;
    }
}
