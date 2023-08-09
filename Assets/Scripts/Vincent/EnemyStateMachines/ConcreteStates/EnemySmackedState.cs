using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Substate of EnemyHurtState. For when the enemy is smacked by any attack that hasn't immediately knocked them down.
/// </summary>
public class EnemySmackedState : EnemyBaseState
{
   public EnemySmackedState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   }

   public override void EnterState() {
      Debug.Log("ENEMY SUB: ENTERED SMACKED");
      if (!Ctx.KnockedDown) {
         Ctx.BaseMaterial.color = new Color(255, 68, 0, 255);
      }
      List<string> recievedAttackNames = Ctx.ApplyAttackStats();
      // Sets the stun timer to 0.5f, which is the default for any non-knockdown attack
      if (Ctx.StunTimer < 0.5f) {
         Ctx.StunTimer = 0.5f;
      }

      if (!recievedAttackNames.Contains("SlamAttack")) {
         GameObject smackedInstance = Ctx.InstantiatePrefab(GameManager.SmackedPrefabInstance);
         smackedInstance.transform.position = Ctx.transform.position;
         if (Ctx.KnockedDown && !Ctx.IsGrounded) {
            smackedInstance.transform.position += new Vector3(0, 1, -0.05f);
         } else {
            smackedInstance.transform.position += new Vector3(0, 3, 0.05f);
         }
      }
   }

   public override void UpdateState() {
      if (!Ctx.IsAttacked) {
         CheckSwitchStates();
      }
   }

   public override void FixedUpdateState() { }

   public override void ExitState() {
     Debug.Log("ENEMY SUB: EXITED SMACKED");
   }

   public override void CheckSwitchStates() {
      if (Ctx.KnockdownMeter <= 0) {
         SwitchState(Factory.KnockedDown());
         return;
      }
      SwitchState(Factory.Stunned());
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
