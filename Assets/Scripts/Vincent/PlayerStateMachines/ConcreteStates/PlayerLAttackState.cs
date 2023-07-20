using UnityEngine;

/// <summary>
/// Substate for the PlayerAttackState. When the player does a light attack
/// </summary>
public class PlayerLAttackState : PlayerBaseState {
   // Handles timing of the attack for startup, active, and recovery frames
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;
   
   public PlayerLAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      // Set canSwitch to false so we can constrain when it's ok to switch from this state
      CanSwitch = false;
   }
   
   public override void EnterState() {
      //Debug.Log("SUB: ENTERED LIGHT");
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.lightAttackBounds.SetActive(true);
   }

   public override void UpdateState() {
      _animationTime += Time.deltaTime;
      _currentFrame = _animationTime / _timePerFrame;

      // Displays the current state of the attack frames.
      // Green is startup frames: No damage is given in this phase
      // Red is active frames: Damage can be given in this phase
      // Blue is recovery frames: No damage given in this phase
      if (_currentFrame <= Ctx.lightStartupFrames.y) {
         Ctx.LightBounds.setMatColor(Color.green);
      } else if (_currentFrame <= Ctx.lightActiveFrames.y) {
         Ctx.LightBounds.setMatColor(Color.red);
         Ctx.LightBounds.setColliderActive(true);
      } else if (_currentFrame <= Ctx.lightRecoveryFrames.y) {
         Ctx.LightBounds.setMatColor(Color.blue);
         Ctx.LightBounds.setColliderActive(false);
      } else {
         CanSwitch = true;
      }
      if (CanSwitch) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      //Debug.Log("SUB: EXITED LIGHT");
      Ctx.lightAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsMediumAttackPressed) {
         SwitchState(Factory.MediumAttack());
      } else if (Ctx.IsPowerupPressed) {
         if (Ctx.PowerupSystem.IsEquipped(PowerupSystem.Powerup.Slam)) {
            SetSubState(Factory.HeavyAttack());
         } else if (Ctx.PowerupSystem.IsEquipped(PowerupSystem.Powerup.Dash)) {
            SetSubState(Factory.DashAttack());
         }
      } else if (Ctx.IsBlockPressed) {
         SwitchState(Factory.Block());
      } else {
         SwitchState(Factory.Idle()); // TEMP FIX for action not ending because the action is being held down
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
