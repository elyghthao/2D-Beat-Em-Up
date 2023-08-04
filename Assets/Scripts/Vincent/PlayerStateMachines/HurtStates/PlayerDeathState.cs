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
      // Scene current_scene = SceneManager.GetActiveScene();
      // SceneManager.LoadScene(current_scene.name);
      Ctx.StartCoroutine(Ctx.DeathTimeDelay(2));
   }

   public override void UpdateState() {
   }

   public override void FixedUpdateState() {
      
   }

   public override void ExitState() {
   }

   public override void CheckSwitchStates() {
   }

   public override void InitializeSubState() {
   }
}
