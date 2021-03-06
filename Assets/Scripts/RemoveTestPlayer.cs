﻿using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;
using UnityEngine;

namespace Client
{
    [EcsInject]
    internal class RemoveTestPlayer : EcsUpdateReactiveSystem<Leave>
    {
        public PlayerCache PlayerCache;

        protected override void RunUpdateReactive()
        {
            for (var i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var transformRef = _world.GetComponent<TransformRef>(entity);
                Object.Destroy(transformRef.value.gameObject);
                _world.RemoveEntity(entity);
            }
        }
    }
}