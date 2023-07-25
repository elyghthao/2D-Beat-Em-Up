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
      Ctx.MostRecentAttack = this.ToString();
   }

   public override void UpdateState() {
      _animationTime += Time.deltaTime;
      _currentFrame = _animationTime / _timePerFrame;

      _currentFrameState = Ctx.FrameState(Ctx.LightBounds, _currentFrame, Ctx.lightStartupFrames, Ctx.lightActiveFrames,
         Ctx.lightRecoveryFrames);
      Debug.Log("CurrentFrameState for LightAttack: " + _currentFrameState);
      if (Ctx.InputSys.IsLightAttackPressed && _currentFrameState >= 2 && !Ctx.InputSys.IsActionHeld) {
         Ctx.QueuedAttack = Factory.LightFirstFollowupAttack();
         Debug.Log("LightAttack 1 Queued");
      }
      if (_currentFrameState == 3) {
         CanSwitch = true;
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      //Debug.Log("SUB: EXITED LIGHT");
      Ctx.lightAttackBounds.SetActive(false);
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
