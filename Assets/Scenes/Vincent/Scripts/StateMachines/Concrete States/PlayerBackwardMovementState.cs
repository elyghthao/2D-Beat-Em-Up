using UnityEngine;

public class PlayerBackwardMovementState : PlayerBaseState
{
   public PlayerBackwardMovementState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
      : base(currentContext, playerStateFactory) {}

   public override void EnterState() {
      throw new System.NotImplementedException();
   }

   public override void UpdateState() {
      throw new System.NotImplementedException();
   }

   public override void OnCollisionEnter() {
      throw new System.NotImplementedException();
   }

   public override void ExitState() {
      throw new System.NotImplementedException();
   }

   public override void CheckSwitchStates() {
      throw new System.NotImplementedException();
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
