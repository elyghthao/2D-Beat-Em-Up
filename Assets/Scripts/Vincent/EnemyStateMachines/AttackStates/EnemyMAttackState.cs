using UnityEngine;

/// <summary>
/// Substate of the EnemyAttackingState. When the enemy does a medium attack

/// NOT IMPLEMENTED YET
/// </summary>
public class EnemyMAttackState : EnemyBaseState {
   // Handles timing of the attack for startup, active, and recovery frames
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;

   public EnemyMAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      // Set canSwitch to false so we can constrain when it's ok to switch from this state
      CanSwitch = false;
   }

   public override void EnterState() {
      // Debug.Log("SUB: ENTERED MEDIUM");
      // Sets the time per frame
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.mediumAttackBounds.SetActive(true);
   }

   public override void UpdateState() {
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
         Ctx.MediumBounds.setColliderActive(true);
      } else if (_currentFrame <= Ctx.mediumRecoveryFrames.y) {
         Ctx.MediumBoundsMat.color = Color.blue;
         Ctx.MediumBounds.setColliderActive(false);
      } else {
         CanSwitch = true;
      }
      if (CanSwitch) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      // Debug.Log("SUB: EXITED MEDIUM");
      Ctx.mediumAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      Ctx.Attacking = false;
      SwitchState(Factory.Idle());
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
