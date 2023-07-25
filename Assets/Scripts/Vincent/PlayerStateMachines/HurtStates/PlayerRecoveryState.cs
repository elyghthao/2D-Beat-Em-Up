using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Root state, when the player is doing an attack or blocking
/// </summary>
public class PlayerRecoveryState : PlayerBaseState
{
   public PlayerRecoveryState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) { }

   public override void EnterState() {
      Debug.Log("ENTERING SUBSTATE: RECOVERY");
   }

   public override void UpdateState() {
      // Waiting for superstate to handle state switch
   }

   public override void ExitState() {
      Debug.Log("EXITING SUBSTATE: RECOVERY");
   }

   public override void CheckSwitchStates() {
      throw new System.NotImplementedException();
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
