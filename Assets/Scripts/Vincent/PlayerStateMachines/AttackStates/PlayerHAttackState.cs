using UnityEngine;

/// <summary>
/// Substate of the PlayerAttackState. When the player performs a heavy attack
/// </summary>
public class PlayerHAttackState : PlayerBaseState {
   // Handles timing of the attack for startup, active, and recovery frames
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;
   private bool _didEffect;

   public PlayerHAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      // Set canSwitch to false so we can constrain when it's ok to switch from this state
      CanSwitch = false;
      _didEffect = false;
   }
   
   public override void EnterState() {
      // Debug.Log("SUB: ENTERED HEAVY");
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.heavyAttackBounds.SetActive(true);
      Ctx.MostRecentAttack = this.ToString();
      Ctx.StaminaRegenAllowed = false;
      Ctx.Stamina -= Ctx.HeavyBounds.staminaDrain;
   }

   public override void UpdateState() {
      _animationTime += Time.deltaTime;
      _currentFrame = _animationTime / _timePerFrame;

      // Displays the current state of the attack frames.
      // Green is startup frames: No damage is given in this phase
      // Red is active frames: Damage can be given in this phase
      // Blue is recovery frames: No damage given in this phase
      if (_currentFrame <= Ctx.heavyStartupFrames) {
         Ctx.HeavyBounds.SetMatColor(Color.green);
      } else if (_currentFrame <= Ctx.heavyActiveFrames) {
         Ctx.HeavyBounds.SetMatColor(Color.red);
         Ctx.HeavyBounds.SetColliderActive(true);

         // Slam Effect
         if (!_didEffect) {
            Ctx.SpriteEffects.doEffect("Slam", Ctx.CharacterFlipped);
            _didEffect = true;
         }
         Ctx.HeavyBounds.SetMatColor(Color.red);
         Ctx.HeavyBounds.SetColliderActive(true);
      } else if (_currentFrame <= Ctx.heavyRecoveryFrames) {
         Ctx.HeavyBounds.SetMatColor(Color.blue);
         Ctx.HeavyBounds.SetColliderActive(false);
      } else {
         CanSwitch = true;
      }
      if (CanSwitch) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      // Debug.Log("SUB: EXITED HEAVY");
      Ctx.heavyAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsLightAttackPressed) {
         SwitchState(Factory.LightAttack());
      } else if (Ctx.IsMediumAttackPressed) {
         SwitchState(Factory.MediumAttack());
      } else {
         // Debug.Log("HEAVY ATTACK - SWITCHING TO IDLE");
         SwitchState(Factory.Idle(), true); // TEMP FIX for action not ending because the action is being held down
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
