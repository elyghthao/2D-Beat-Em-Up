using System.Threading;
using UnityEngine;

public class EnemyHurtState : EnemyBaseState {
   public EnemyHurtState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      IsRootState = true;
      InitializeSubState();
   }

   public override void EnterState() {
      Debug.Log("ENEMY ROOT: ENTERED HURT");
   }

   public override void UpdateState() {
      Ctx.StunTimer -= Time.deltaTime;
      if (Ctx.StunTimer <= 0) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      Debug.Log("ENEMY ROOT: EXITED HURT");
      Ctx.KnockedDown = false;
   }

   public override void CheckSwitchStates() {
      SwitchState(Factory.Idle());
   }

   public override void InitializeSubState() {
      SetSubState(Factory.Stunned());
   }
}
