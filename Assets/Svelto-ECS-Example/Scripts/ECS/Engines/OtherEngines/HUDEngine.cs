using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.EntityViews.HUD;
using Svelto.Tasks.Enumerators;
using System.Collections;
using Svelto.ECS.Example.Survive.Others;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svelto.ECS.Example.Survive.Engines.HUD
{
    public class HUDEngine : SingleEntityViewEngine<HUDEntityView>, IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public HUDEngine(ITime time)
        {
            _time = time;
        }

        public void Ready()
        {}

        protected override void Add(HUDEntityView EntityView)
        {
            _guiEntityView = EntityView;
            Tick().Run();
        }

        protected override void Remove(HUDEntityView EntityView)
        {
            _guiEntityView = null;
        }

        IEnumerator Tick()
        {
            while (true)
            {
                var damageComponent = _guiEntityView.damageImageComponent;

                damageComponent.imageColor = Color.Lerp(damageComponent.imageColor, Color.clear, damageComponent.speed * _time.deltaTime);

                yield return null;
                
                if (_guiEntityView == null) yield break;
            }
        }

        void OnDamageEvent(DamageInfo damaged)
        {
            UpdateSlider(damaged);
        }

        void OnDeadEvent()
        {
            _guiEntityView.healthSliderComponent.value = 0;

            RestartLevelAfterFewSeconds().Run();
        }

        void UpdateSlider(DamageInfo damaged)
        {
            var damageComponent = _guiEntityView.damageImageComponent;

            damageComponent.imageColor = damageComponent.flashColor;

            var hudDamageEntityView =
                entityViewsDB.QueryEntityView<HUDDamageEntityView>(damaged.entityDamaged);
         
            _guiEntityView.healthSliderComponent.value = hudDamageEntityView.healthComponent.currentHealth;
        }

        IEnumerator RestartLevelAfterFewSeconds()
        {
            _waitForSeconds.Reset(5);
            yield return _waitForSeconds;

            _guiEntityView.HUDAnimator.trigger = "GameOver";

            _waitForSeconds.Reset(2);
            yield return _waitForSeconds;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Step(ref DamageInfo token, int condition)
        {
            if (condition == DamageCondition.damage)
            {
                OnDamageEvent(token);
            }
            else
            if (condition == DamageCondition.dead)
                OnDeadEvent();
        }

        HUDEntityView                      _guiEntityView;
        readonly WaitForSecondsEnumerator  _waitForSeconds = new WaitForSecondsEnumerator(5);
        readonly ITime                     _time;
    }
}

