using UnityEngine;

public class PlayerHAttackState : PlayerBaseState
{
   public PlayerHAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      IsRootState = true;
      InitializeSubState();
   }
   
   public override void EnterState() {
      throw new System.NotImplementedException();
   }

   public override void UpdateState() {
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
