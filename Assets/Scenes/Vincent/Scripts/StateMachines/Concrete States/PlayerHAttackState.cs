using UnityEngine;

public class PlayerHAttackState : PlayerBaseState {
   private bool _finishedAnimation = false;
   private float _animationTime = 0;
   private float _currentFrame = 1;
   private float _timePerFrame;
   
   public PlayerHAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      IsRootState = true;
      //InitializeSubState();
   }
   
   public override void EnterState() {
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.heavyAttackBounds.SetActive(true);
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
      if (_currentFrame <= Ctx.heavyStartupFrames.y) {
         Ctx.HeavyBoundsMat.color = Color.green;
      } else if (_currentFrame <= Ctx.heavyActiveFrames.y) {
         Ctx.HeavyBoundsMat.color = Color.red;
      } else if (_currentFrame <= Ctx.heavyRecoveryFrames.y) {
         Ctx.HeavyBoundsMat.color = Color.blue;
      } else {
         _finishedAnimation = true;
      }
   }

   public override void ExitState() {
      Ctx.heavyAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsActionPressed) {
         if (Ctx.IsLightAttackPressed) {
            SwitchState(Factory.LightAttack());
         } else if (Ctx.IsMediumAttackPressed) {
            SwitchState(Factory.MediumAttack());
         } else if (Ctx.IsHeavyAttackPressed) {
            SwitchState(Factory.Idle());
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
