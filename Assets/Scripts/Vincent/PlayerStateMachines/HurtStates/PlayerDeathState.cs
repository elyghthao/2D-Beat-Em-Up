using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Root state, when the player is doing an attack or blocking
/// </summary>
public class PlayerDeathState : PlayerBaseState
{
   public PlayerDeathState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) {
      IsRootState = true;
   }

   public override void EnterState() {
      Debug.Log("Player Died: " + Ctx.CurrentHealth);
      Scene current_scene = SceneManager.GetActiveScene();
      SceneManager.LoadScene(current_scene.name);
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
