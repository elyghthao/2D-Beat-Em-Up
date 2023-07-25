using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Root state, when the player is doing an attack or blocking
/// </summary>
public class PlayerRecoveryState : PlayerBaseState
{
   public PlayerRecoveryState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) {
      IsRootState = true;
   }

   public override void EnterState() {
      
   }

   public override void UpdateState() {
   }

   public override void ExitState() {
   }

   public override void CheckSwitchStates() {
   }

   public override void InitializeSubState() {
   }
}
