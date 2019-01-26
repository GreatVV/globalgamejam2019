using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;
using UnityEngine;

namespace Client
{
    [EcsInject]
    public class UpdateNonServerShipPositionSystem : EcsReactiveSystem<RoomData>
    {
        private EcsWorld _world;
        private PhotonServer PhotonServer;
        protected override EcsReactiveType GetReactiveType()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive()
        {
            for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var roomData = _world.GetComponent<RoomData>(entity).value;

                
                     var position = (Vector3)roomData[RoomDataConstants.ShipPosition];
                     _world.CreateEntityWith<ShipPosition>(out var shipPosition);
                     shipPosition.value = position;
                }
            }
        }
    }

    public static class RoomDataConstants
    {
        public const  int ShipPosition = 1;
    }
}