using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    public class SyncShipPosition : EcsReactiveSystem<ShipPosition>
    {
        public EcsWorld World;
        public SceneDescription SceneDescription;
        public GameState _gameState;

        private EcsFilter<Local, Role> _localPlayer;

        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive ()
        {
            if (_localPlayer.EntitiesCount > 0 && _localPlayer.Components2[0].value == PlayerRole.Fly)
            {
                return;
            }

            for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var shipPosition = World.GetComponent<ShipPosition> (entity);
                World.EnsureComponent<Position> (_gameState.ShipEntity, out _).value = shipPosition.position;
                World.EnsureComponent<Rotation> (_gameState.ShipEntity, out _).value = shipPosition.rotation;

            }
        }
    }
}
