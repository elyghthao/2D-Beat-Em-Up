using UnityEngine;

/// <summary>
/// Default enemy root state, for when nothing is happening to the enemy, and the enemy is doing nothing.
/// </summary>
public class EnemyIdleState : EnemyBaseState {
   public EnemyIdleState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      IsRootState = true;
   }

   public override void EnterState() {
      // Debug.Log("ENEMY ROOT: ENTERED IDLE");
      Ctx.BaseMaterial.color = Color.green;
   }

   public override void UpdateState() {
      // Checks to see if we should start regenerating our knockdown meter. Should probably exponentially grow instead 
      // of linearly
      if (Ctx.KnockdownMeter < Ctx.knockdownMax) {
         // Debug.Log("Regenerating: " + Ctx.KnockdownMeter);
         Ctx.KnockdownMeter += Time.deltaTime * 50;
      } else if (Ctx.KnockdownMeter > Ctx.knockdownMax){
         // Debug.Log("Degenerating: " + Ctx.KnockdownMeter);
         Ctx.KnockdownMeter = Ctx.knockdownMax;
      }
      CheckSwitchStates();
   }

   public override void FixedUpdateState() {
      
   }

   public override void ExitState() {
      // Debug.Log("ENEMY ROOT: EXITED IDLE");
   }
   
   public override void CheckSwitchStates() {
      // Only other root state implemented right now is the hurt state
      if (Ctx.IsAttacked) {
         SwitchState(Factory.Hurt());
         return;
      }
      
      // If player and if the player is within the activation distance, move towards the player
      if (Ctx.CurrentPlayerMachine != null) {
         float dist = Vector3.Distance(Ctx.gameObject.transform.position, Ctx.CurrentPlayerMachine.gameObject.transform.position);
         if (dist <= Ctx.activationDistance) {
            SwitchState(Factory.Move());
         }
      }
   }

   public override void InitializeSubState() {
      // Idle has no sub state
      throw new System.NotImplementedException();
   }
}
