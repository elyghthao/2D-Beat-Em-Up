using UnityEngine;

public class PlayerHAttackState : PlayerBaseState {
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;

   public PlayerHAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      CanSwitch = false;
   }
   
   public override void EnterState() {
      Debug.Log("SUB: ENTERED HEAVY");
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.heavyAttackBounds.SetActive(true);
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
      if (_currentFrame <= Ctx.heavyStartupFrames.y) {
         Ctx.HeavyBoundsMat.color = Color.green;
      } else if (_currentFrame <= Ctx.heavyActiveFrames.y) {
         Ctx.HeavyBoundsMat.color = Color.red;
      } else if (_currentFrame <= Ctx.heavyRecoveryFrames.y) {
         Ctx.HeavyBoundsMat.color = Color.blue;
      } else {
         CanSwitch = true;
      }
   }

   public override void ExitState() {
      Debug.Log("SUB: EXITED HEAVY");
      Ctx.heavyAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsLightAttackPressed) {
         SwitchState(Factory.LightAttack());
      } else if (Ctx.IsMediumAttackPressed) {
         SwitchState(Factory.MediumAttack());
      } else if (Ctx.IsBlockPressed) {
         SwitchState(Factory.Block());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
