using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

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

                var role = _world.GetComponent<Role> (entity).value;
                switch (role)
                {
                    case PlayerRole.Fly:
                        {
                            UnityEngine.Cursor.visible = true;
                            _sceneDescription.FlyPlayerCamera?.SetActive (true);
                            _sceneDescription.ShootPlayerCamera?.SetActive (false);
                        }
                        break;
                    case PlayerRole.Shoot:
                        {
                            UnityEngine.Cursor.visible = false;
                            _sceneDescription.FlyPlayerCamera?.SetActive (false);
                            _sceneDescription.ShootPlayerCamera?.SetActive (true);
                        }
                        break;
                }
            }
        }
    }
}
