using System;
using System.Collections;
using UnityEngine;
using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemyAnimationEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {}

        void EntityDamaged(DamageInfo damageInfo)
        {
            var entity = entityViewsDB.QueryEntityView<EnemyEntityView>(damageInfo.entityDamaged);

            entity.vfxComponent.position = damageInfo.damagePoint;
            entity.vfxComponent.play.value = true;
        }

        void TriggerTargetDeathAnimation(int targetID)
        {
            var entity = entityViewsDB.QueryEntityViews<EnemyEntityView>();

            for (int i = 0; i < entity.Count; i++)
                entity[i].animationComponent.trigger = "PlayerDead";
        }

        void TriggerDeathAnimation(int targetID)
        {
            var entity = entityViewsDB.QueryEntityView<EnemyEntityView>(targetID);
            entity.animationComponent.trigger = "Dead";

            Sink(entity, entity.sinkSpeedComponent.sinkAnimSpeed).Run();
        }

        IEnumerator Sink(EnemyEntityView entity, float sinkSpeed)
        {
            DateTime afterTwoSec = DateTime.UtcNow.AddSeconds(2);

            while (DateTime.UtcNow < afterTwoSec)
            {
                entity.transformComponent.position += -Vector3.up * sinkSpeed * Time.deltaTime;

                yield return null;
            }

            entity.destroyComponent.destroyed.value = true;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            if (token.entityType == EntityDamagedType.PlayerTarget)
            {
                if (condition == DamageCondition.dead)
                    TriggerDeathAnimation(token.entityDamaged);
                else
                    EntityDamaged(token);
            }
            else
            {
                if (condition == DamageCondition.dead)
                    TriggerTargetDeathAnimation(token.entityDamaged);    
            }
        }
    }
}
