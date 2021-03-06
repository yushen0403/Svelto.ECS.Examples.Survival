using Svelto.ECS.Example.Survive.EntityViews.Sound;
using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.Engines.Sound.Damage
{
    public class DamageSoundEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {}

        void TriggerDeathSound(int targetID)
        {
            var audioEntityView =  entityViewsDB.QueryEntityView<DamageSoundEntityView>(targetID);
            var audioComponent = audioEntityView.audioComponent;

            audioComponent.playOneShot = AudioType.death;
        }

        void TriggerDamageAudio(int sender)
        {
           var audioEntityView = entityViewsDB.QueryEntityView<DamageSoundEntityView>(sender);
           var audioComponent = audioEntityView.audioComponent;

           audioComponent.playOneShot = AudioType.damage;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            if (condition == DamageCondition.damage)
                TriggerDamageAudio(token.entityDamaged);
            else
                TriggerDeathSound(token.entityDamaged);
        }
    }
}
