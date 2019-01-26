using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    internal class UpdateHealthSystem : EcsReactiveSystem<RoomData>
    {
        private EcsWorld _world;
        private GameState _gameState;

        private SceneDescription _sceneDescription;

        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive ()
        {
            for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var roomData = _world.GetComponent<RoomData> (entity).value;
                if (roomData.ContainsKey (RoomDataConstants.Health))
                {
                    var health = _world.EnsureComponent<Health> (_gameState.ShipEntity, out _);
                    health.value = (float) roomData[RoomDataConstants.Health];

                    _sceneDescription.UI.GameUI.SetHealth (health.value);
                }
            }
        }
    }
}
