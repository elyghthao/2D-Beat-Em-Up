using UnityEngine;

/// <summary>
/// Substate of the PlayerAttackState. When the player does a medium attack
/// </summary>
public class PlayerMAttackState : PlayerBaseState {
   // Handles timing of the attack for startup, active, and recovery frames
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;
   // 0 == startup, 1 == active, 2 == recovery, 3 == finished
   private int _currentFrameState;

   public PlayerMAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      // Set canSwitch to false so we can constrain when it's ok to switch from this state
      CanSwitch = false;
   }

   public override void EnterState() {
      // Debug.Log("SUB: ENTERED MEDIUM");
      // Sets the time per frame
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.mediumAttackBounds.SetActive(true);
      Ctx.FollowupTimer = Ctx.attackFollowupThreshold;
      Ctx.MostRecentAttack = this.ToString();
   }

   public override void UpdateState() {
      _animationTime += Time.deltaTime;
      _currentFrame = _animationTime / _timePerFrame;

      _currentFrameState = Ctx.FrameState(Ctx.MediumBounds, _currentFrame, Ctx.mediumStartupFrames,
         Ctx.mediumActiveFrames, Ctx.mediumRecoveryFrames);
      if (Ctx.InputSys.IsMediumAttackPressed && _currentFrameState >= 2 && !Ctx.InputSys.IsActionHeld) {
         Ctx.QueuedAttack = Factory.MediumFirstFollowupAttack();
      }
      if (_currentFrameState == 3) {
         CanSwitch = true;
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      // Debug.Log("SUB: EXITED MEDIUM");
      Ctx.mediumAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      if (Ctx.QueuedAttack != null) {
         SwitchState(Ctx.QueuedAttack);
         Ctx.QueuedAttack = null;
         return;
      }
      SwitchState(Factory.Idle());
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
