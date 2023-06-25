using UnityEngine;

public class PlayerBlockState : PlayerBaseState {
   private bool _finishedAnimation = false;
   public PlayerBlockState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) {
      IsRootState = true;
      //InitializeSubState();
   }
   
   public override void EnterState() {
      Debug.Log("Entered Block State");
   }
    
   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void ExitState() {
      Debug.Log("Exiting Block State");
   }

   public override void CheckSwitchStates() {
      SwitchState(Factory.Idle());
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
