using UnityEngine;

public class PlayerMAttackState : PlayerBaseState {
   private bool _finishedAnimation = false;
   private float _animationTime = 0;
   private float _currentFrame = 1;
   private float _timePerFrame;
   
   public PlayerMAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      IsRootState = true;
      //InitializeSubState();
   }

   public override void EnterState() {
      _timePerFrame = Ctx.framesPerSecond / 60f;
      Ctx.mediumAttackBounds.SetActive(true);
      Debug.Log("Entered Medium Attack State");
   }

   public override void UpdateState() {
      if (_finishedAnimation) {
         CheckSwitchStates();
      }
      _animationTime += Time.deltaTime;
      _currentFrame = _animationTime / _timePerFrame;

      // Displays the current state of the attack frames.
      // Green is startup frames: No damage is given in this phase
      // Red is active frames: Damage can be given in this phase
      // Blue is recovery frames: No damage given in this phase
      if (_currentFrame <= Ctx.mediumStartupFrames.y) {
         Ctx.MediumBoundsMat.color = Color.green;
      } else if (_currentFrame <= Ctx.mediumActiveFrames.y) {
         Ctx.MediumBoundsMat.color = Color.red;
      } else if (_currentFrame <= Ctx.mediumRecoveryFrames.y) {
         Ctx.MediumBoundsMat.color = Color.blue;
      } else {
         _finishedAnimation = true;
      }
   }

   public override void ExitState() {
      Ctx.mediumAttackBounds.SetActive(false);
      Debug.Log("Exiting Medium Attack State");
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsActionPressed) {
         if (Ctx.IsLightAttackPressed) {
            SwitchState(Factory.LightAttack());
         } else if (Ctx.IsMediumAttackPressed) {
            SwitchState(Factory.Idle());
         } else if (Ctx.IsHeavyAttackPressed) {
            SwitchState(Factory.HeavyAttack());
         } else if (Ctx.IsBlockPressed) {
            SwitchState(Factory.Block());
         }
      } else if (Ctx.IsMovementPressed) {
         if (Ctx.CurrentMovementInput.x < 0) {
            SwitchState(Factory.Backward());
         } else if (Ctx.CurrentMovementInput.x > 0 || Ctx.CurrentMovementInput.y != 0) {
            SwitchState(Factory.Forward());
         }
      } else {
         SwitchState(Factory.Idle());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
