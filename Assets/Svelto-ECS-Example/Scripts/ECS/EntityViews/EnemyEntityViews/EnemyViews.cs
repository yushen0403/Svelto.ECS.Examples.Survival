using Svelto.ECS.Example.Survive.Components.Shared;
using Svelto.ECS.Example.Survive.Components.Enemies;

namespace Svelto.ECS.Example.Survive.EntityViews.Enemies
{
    public class EnemyEntityView:EntityView
    {
        public IEnemyTargetComponent      TargetComponent;
        public IEnemyAttackDataComponent  attackDamageComponent;
        public IEnemyTriggerComponent     targetTriggerComponent;
        public IEnemyMovementComponent    movementComponent;
        public IEnemyVFXComponent         vfxComponent;

        public IAnimationComponent        animationComponent;
        public ITransformComponent        transformComponent;
        public IDestroyComponent          destroyComponent;
        public IEnemySinkComponent        sinkSpeedComponent;
        public IRigidBodyComponent        rigidBodyComponent;
    }

    public class EnemyTargetEntityView : EntityView
    {
        public IPositionComponent         targetPositionComponent;
    }
}
