using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerL2AttackState : PlayerBaseState
{
   // Handles timing of the attack for startup, active, and recovery frames
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;
   // 0 == startup, 1 == active, 2 == recovery, 3 == finished
   private int _currentFrameState;
   
   public PlayerL2AttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      CanSwitch = false;
   }

   public override void EnterState() {
      Debug.Log("SUB: ENTERED LIGHT 2");
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.lightSecondFollowupAttackBounds.SetActive(true);
      Ctx.MostRecentAttack = this.ToString();
   }

   public override void UpdateState() {
      _animationTime += Time.deltaTime;
      _currentFrame = _animationTime / _timePerFrame;

      _currentFrameState = Ctx.FrameState(Ctx.LightSecondFollowupBounds, _currentFrame, Ctx.light2StartupFrames,
         Ctx.light2ActiveFrames, Ctx.light2RecoveryFrames);
      Debug.Log("CurrentFrameState for LightAttack 2: " + _currentFrameState);
      if (_currentFrameState == 3) {
         CanSwitch = true;
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      Debug.Log("SUB: EXITED LIGHT 2");
      Ctx.lightSecondFollowupAttackBounds.SetActive(false);
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
