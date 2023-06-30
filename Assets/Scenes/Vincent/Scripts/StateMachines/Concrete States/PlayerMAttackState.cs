using UnityEngine;

public class PlayerMAttackState : PlayerBaseState {
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;

   public PlayerMAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      CanSwitch = false;
   }

   public override void EnterState() {
      Debug.Log("SUB: ENTERED MEDIUM");
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.mediumAttackBounds.SetActive(true);
   }

   public override void UpdateState() {
      if (CanSwitch) {
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
         CanSwitch = true;
      }
   }

   public override void ExitState() {
      Debug.Log("SUB: EXITED MEDIUM");
      Ctx.mediumAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsLightAttackPressed) {
         SwitchState(Factory.LightAttack());
      } else if (Ctx.IsHeavyAttackPressed) {
         SwitchState(Factory.HeavyAttack());
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
