using UnityEngine;

/// <summary>
/// Substate for the PlayerAttackState. When the player does a light attack
/// </summary>
public class PlayerLAttackState : PlayerBaseState {
   // Handles timing of the attack for startup, active, and recovery frames
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;
   // 0 == startup, 1 == active, 2 == recovery, 3 == finished
   private int _currentFrameState;
   
   public PlayerLAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      // Set canSwitch to false so we can constrain when it's ok to switch from this state
      CanSwitch = false;
   }
   
   public override void EnterState() {
      //Debug.Log("SUB: ENTERED LIGHT");
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.lightAttackBounds.SetActive(true);
      Ctx.FollowupTimer = Ctx.attackFollowupThreshold;
      Ctx.MostRecentAttack = ToString();
      Ctx.StaminaRegenAllowed = false;
      Ctx.Stamina -= Ctx.LightBounds.staminaDrain;
   }

   public override void UpdateState() {
      _animationTime += Time.deltaTime;
      _currentFrame = _animationTime / _timePerFrame;

      _currentFrameState = Ctx.FrameState(Ctx.LightBounds, _currentFrame, Ctx.lightStartupFrames, Ctx.lightActiveFrames,
         Ctx.lightRecoveryFrames);
      //Debug.Log("CurrentFrameState for LightAttack: " + _currentFrameState);
      if (Ctx.InputSys.IsLightAttackPressed && _currentFrameState >= 2 && !Ctx.InputSys.IsActionHeld
          && Ctx.Stamina >= Ctx.LightFirstFollowupBounds.staminaDrain) {
         Ctx.QueuedAttack = Factory.LightFirstFollowupAttack();
         // Debug.Log("LightAttack 1 Queued");
      }
      if (_currentFrameState == 0 || _currentFrameState == 2) {
         if (Ctx.IsAttacked) {
            CheckSwitchStates();
         }
      } else if (_currentFrameState == 3) {
         CanSwitch = true;
         CheckSwitchStates();
      }
   }

   public override void FixedUpdateState() {
      
   }

   public override void ExitState() {
      //Debug.Log("SUB: EXITED LIGHT");
      Ctx.lightAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsAttacked) {
         SwitchState(Factory.Hurt(), true);
         Ctx.QueuedAttack = null;
         return;
      }
      if (Ctx.QueuedAttack != null) {
         SwitchState(Ctx.QueuedAttack);
         Ctx.ResetAttackQueue();
         return;
      }
      SwitchState(Factory.Idle(), true); // TEMP FIX for action not ending because the action is being held down
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
