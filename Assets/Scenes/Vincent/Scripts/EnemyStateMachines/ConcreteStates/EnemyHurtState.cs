using System.Threading;
using UnityEngine;

public class EnemyHurtState : EnemyBaseState {

   private float _hurtTimer = 0.5f;
   private float _curTimer = 0;
   
   public EnemyHurtState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      IsRootState = true;
   }

   public override void EnterState() {
      Debug.Log("ENEMY ROOT: ENTERED HURT");
      Ctx.BaseMaterial.color = Color.red;
      Ctx.Rigidbody.AddForce(5f, 200, 0);
      Ctx.KnockdownMeter -= DetermineKnockdownPressure();
   }

   public override void UpdateState() {
      _curTimer += Time.deltaTime;
      if (_curTimer >= _hurtTimer) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      Debug.Log("ENEMY ROOT: EXITED HURT");
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsAttacked) {
         if (Ctx.KnockdownMeter <= 0) {
            SwitchState(Factory.KnockedDown());
         }
      } else {
         SwitchState(Factory.Idle());
      }
   }

   public int DetermineKnockdownPressure() {
      int pressure = 0;
      if (Ctx.RecievedAttack[0]) {
         pressure = 40;
      } else if (Ctx.RecievedAttack[1]) {
         pressure = 60;
      } else if (Ctx.RecievedAttack[2]) {
         pressure = 100;
      } else if (Ctx.RecievedAttack[3]) {
         pressure = 70;
      } else if (Ctx.RecievedAttack[4]) {
         pressure = 80;
      } else if (Ctx.RecievedAttack[5]) {
         pressure = 150;
      }
      return pressure;
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
