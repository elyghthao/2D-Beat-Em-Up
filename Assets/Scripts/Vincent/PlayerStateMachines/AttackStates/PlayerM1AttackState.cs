using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerM1AttackState : PlayerBaseState
{
   // Handles timing of the attack for startup, active, and recovery frames
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;
   // 0 == startup, 1 == active, 2 == recovery, 3 == finished
   private int _currentFrameState;
   
   public PlayerM1AttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      // Set canSwitch to false so we can constrain when it's ok to switch from this state
      CanSwitch = false;
   }

   public override void EnterState() {
      // Debug.Log("SUB: ENTERED MEDIUM 1");
      // Sets the time per frame
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.mediumFirstFollowupAttackBounds.SetActive(true);
      Ctx.MostRecentAttack = ToString();
      Ctx.StaminaRegenAllowed = false;
      Ctx.Stamina -= Ctx.MediumBounds.staminaDrain;
   }

   public override void UpdateState() {
      _animationTime += Time.deltaTime;
      _currentFrame = _animationTime / _timePerFrame;

      _currentFrameState = Ctx.FrameState(Ctx.MediumFirstFollowupBounds, _currentFrame, Ctx.medium1StartupFrames,
         Ctx.medium1ActiveFrames, Ctx.medium1RecoveryFrames);
      if (_currentFrameState == 3) {
         CanSwitch = true;
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      // Debug.Log("SUB: EXITED MEDIUM 1");
      Ctx.mediumFirstFollowupAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      if (Ctx.QueuedAttack != null) {
         SwitchState(Ctx.QueuedAttack);
         Ctx.ResetAttackQueue();
      } else {
         SwitchState(Factory.Idle());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
