using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;
using UnityEngine;

namespace Client
{
    [EcsInject]
    public class ChangeRoleSystem : EcsUpdateReactiveSystem<Role>
    {
        public EcsWorld _world;
        public SceneDescription _sceneDescription;
        protected override void RunUpdateReactive ()
        {
            for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var local = _world.GetComponent<Local> (entity);
                if (local == null)
                {
                    continue;
                }

                var role = _world.GetComponent<Role> (entity).value;
                switch (role)
                {
                    case PlayerRole.Fly:
                        {
                            Cursor.visible = true;
                            Cursor.lockState = CursorLockMode.None;
                            _sceneDescription.FlyPlayerCamera?.SetActive (true);
                            _sceneDescription.ShootPlayerCamera?.SetActive (false);
                        }
                        break;
                    case PlayerRole.Shoot:
                        {
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked;
                            _sceneDescription.FlyPlayerCamera?.SetActive (false);
                            _sceneDescription.ShootPlayerCamera?.SetActive (true);
                        }
                        break;
                }
            }
        }
    }
}
