using UnityEngine;

/// <summary>
/// Substate of PlayerMoveState. When the player is moving towards the left side of the screen
/// </summary>
public class PlayerBackwardMovementState : PlayerBaseState
{
   public PlayerBackwardMovementState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) { }

   public override void EnterState() {
      // Debug.Log("SUB: ENTERED BACKWARD");
      // Flips the character to be facing left of the camera

      // !!! EDITED BY BRANDON ============================================================= !!!
      GameObject mainCamera = GameObject.Find("Main Camera");

      if (mainCamera.GetComponent<Mirror_Mode>().flipHorizontal == false)
      {
         if (!Ctx.CharacterFlipped)
         {
            Ctx.FlipCharacter();
         }
      }
      else
      {
         if (Ctx.CharacterFlipped)
         {
            Ctx.FlipCharacter();
         }
      }
      // !!! =============================================================================== !!!

      /*
      if (!Ctx.CharacterFlipped) {
         Ctx.FlipCharacter();
      }*/
      Ctx.BaseMaterial.color = Color.blue;
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void FixedUpdateState() {
   }

   public override void ExitState() {
      // Debug.Log("SUB: EXITED BACKWARD");
      Ctx.BaseMaterial.color = Color.white;
      Ctx.Rigidbody.velocity = new Vector3(0, 0, 0);
   }

   public override void CheckSwitchStates() {
      if (Ctx.CurrentMovementInput.x > 0) {
         SwitchState(Factory.Forward());
      }
   }

   public override void InitializeSubState() { throw new System.NotImplementedException(); }
}
