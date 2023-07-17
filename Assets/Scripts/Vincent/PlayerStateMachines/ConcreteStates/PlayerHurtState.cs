using UnityEngine;

/// <summary>
/// Root state, for when the player is hurt by an enemy attack
/// </summary>
public class PlayerHurtState : PlayerBaseState
{
   public PlayerHurtState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) {
      IsRootState = true;
      InitializeSubState();
   }

   public override void EnterState() {
      Debug.Log("ROOT: ENTERED HURT");
   }

   public override void UpdateState() {
      // If the stun timer has reached 0 or less, then we can transition out of being hurt. Substates will update the 
      // stun timer accordingly
      if (Ctx.IsGrounded) {
         Ctx.StunTimer -= Time.deltaTime;
      }

      if (Ctx.StunTimer <= 0) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      Debug.Log("ROOT: EXITED HURT");
      Ctx.KnockedDown = false;
   }

   public override void CheckSwitchStates() {
      // Only possible alternative currently is to be returned to the Idle State
      SwitchState(Factory.Idle());
      // Could add more states here when there are more root states implemented
   }

   public override void InitializeSubState() {
      // Only state that should be set to the substate initially is the Stunned state
      SetSubState(Factory.Stunned());
   }
}
