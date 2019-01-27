using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    public class Radar : MonoBehaviour, IEcsSystem
    {
        private GameState _gameState;
        private EcsWorld _world;

        private SceneDescription _sceneDescription;

        private List<RadarMarker> _pointObjects = new List<RadarMarker> ();
        private List<Vector3> _positions = new List<Vector3> ();

        public RadarMarker RadarMarker;

        public void Show (bool state)
        {
            gameObject.SetActive (state);
        }

        void Update ()
        {
            _positions.Clear ();
            var rect = new Rect (0, 0, 1, 1);
            var camera = _sceneDescription.GameCamera;
            if (!camera)
            {
                return;
            }

            foreach (var asteroid in _gameState.Asteroids)
            {
                var entity = asteroid.Value;
                var asteroidTransform = _world.GetComponent<TransformRef> (entity).value;
                var viewPortPosition = camera.WorldToViewportPoint (asteroidTransform.position);
                if (!rect.Contains (viewPortPosition))
                {
                    _positions.Add (rect.IntersectionWithRayFromCenter (viewPortPosition));
                }
            }

            for (var i = 0; i < _positions.Count; i++)
            {
                var position = _positions[i];
                var go = Instantiate (RadarMarker);
                if (i < _pointObjects.Count)
                {
                    go = _pointObjects[i];
                }
                else
                {
                    go = Instantiate (RadarMarker);
                    _pointObjects.Add (go);
                }
                go.transform.position = camera.ViewportPointToRay (position).GetPoint (10);
                go.transform.rotation = camera.transform.rotation;
            }

            for (var i = _positions.Count; i < _pointObjects.Count; i++)
            {
                _pointObjects[i]?.gameObject.SetActive (false);
            }

        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable ()
        {
            for (var i = 0; i < _pointObjects.Count; i++)
            {
                _pointObjects[i]?.gameObject.SetActive (false);
            }
        }
    }

    public static class UnityEngineExtensions
    {

        public static Vector2 Abs (this Vector2 vector)
        {
            for (int i = 0; i < 2; ++i) vector[i] = Mathf.Abs (vector[i]);
            return vector;
        }

        public static Vector2 DividedBy (this Vector2 vector, Vector2 divisor)
        {
            for (int i = 0; i < 2; ++i) vector[i] /= divisor[i];
            return vector;
        }

        public static Vector2 Max (this Rect rect)
        {
            return new Vector2 (rect.xMax, rect.yMax);
        }

        public static Vector2 IntersectionWithRayFromCenter (this Rect rect, Vector2 pointOnRay)
        {
            Vector2 pointOnRay_local = pointOnRay - rect.center;
            Vector2 edgeToRayRatios = (rect.Max () - rect.center).DividedBy (pointOnRay_local.Abs ());
            return (edgeToRayRatios.x < edgeToRayRatios.y) ?
                new Vector2 (pointOnRay_local.x > 0 ? rect.xMax : rect.xMin,
                    pointOnRay_local.y * edgeToRayRatios.x + rect.center.y) :
                new Vector2 (pointOnRay_local.x * edgeToRayRatios.y + rect.center.x,
                    pointOnRay_local.y > 0 ? rect.yMax : rect.yMin);
        }

    }
}
