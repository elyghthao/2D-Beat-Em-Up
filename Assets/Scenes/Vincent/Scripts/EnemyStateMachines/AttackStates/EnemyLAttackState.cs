using UnityEngine;

/// <summary>
/// Substate of the EnemyAttackingState. When the enemy does a light attack

/// NOT IMPLEMENTED YET
/// </summary>
public class EnemyLAttackState : EnemyBaseState {
   // Handles timing of the attack for startup, active, and recovery frames
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;
   
   public EnemyLAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      // Set canSwitch to false so we can constrain when it's ok to switch from this state
      CanSwitch = false;
   }
   
   public override void EnterState() {
      //Debug.Log("SUB: ENTERED LIGHT");
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.lightAttackBounds.SetActive(true);
   }

   public override void UpdateState() {
      _animationTime += Time.deltaTime;
      _currentFrame = _animationTime / _timePerFrame;

      // Displays the current state of the attack frames.
      // Green is startup frames: No damage is given in this phase
      // Red is active frames: Damage can be given in this phase
      // Blue is recovery frames: No damage given in this phase
      if (_currentFrame <= Ctx.lightStartupFrames.y) {
         Ctx.LightBoundsMat.color = Color.green;
      } else if (_currentFrame <= Ctx.lightActiveFrames.y) {
         Ctx.LightBoundsMat.color = Color.red;
      } else if (_currentFrame <= Ctx.lightRecoveryFrames.y) {
         Ctx.LightBoundsMat.color = Color.blue;
      } else {
         CanSwitch = true;
      }
      if (CanSwitch) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      //Debug.Log("SUB: EXITED LIGHT");
      Ctx.lightAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      Ctx.Attacking = false;
      SwitchState(Factory.Idle());
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
