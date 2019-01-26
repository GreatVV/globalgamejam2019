using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;
using UnityEngine;

namespace Client
{
    [EcsInject]
    public class UpdateShipPositionSystem : EcsReactiveSystem<RoomData>
    {
        private EcsWorld _world;
        private PhotonServer PhotonServer;

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
                _world.CreateEntityWith<ShipPosition> (out var shipPosition);

                if (roomData.ContainsKey (RoomDataConstants.ShipPosition))
                {
                    var position = (Vector3) roomData[RoomDataConstants.ShipPosition];
                    shipPosition.position = position;
                }
                else
                {
                    shipPosition.position = _sceneDescription.Ship.transform.position;
                }

                if (roomData.ContainsKey (RoomDataConstants.ShipRotation))
                {
                    var rotation = (Quaternion) roomData[RoomDataConstants.ShipRotation];
                    shipPosition.rotation = rotation;
                }
                else
                {
                    shipPosition.rotation = _sceneDescription.Ship.transform.rotation;
                }
            }
        }
    }
}
