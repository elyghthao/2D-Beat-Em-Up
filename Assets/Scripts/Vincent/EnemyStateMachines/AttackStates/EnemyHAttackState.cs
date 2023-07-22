using UnityEngine;

/// <summary>
/// Substate of the EnemyAttackingState. When the enemy performs a heavy attack

/// WORKING ON CURRENTLY
/// </summary>
public class EnemyHAttackState : EnemyBaseState {
   // Handles timing of the attack for startup, active, and recovery frames
   private float _animationTime;
   private float _currentFrame = 1;
   private float _timePerFrame;

   public EnemyHAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      // Set canSwitch to false so we can constrain when it's ok to switch from this state
      CanSwitch = false;
   }
   
   public override void EnterState() {
      // Debug.Log("ENEMY ROOT: ENTERED HEAVY");
      _timePerFrame = (Ctx.framesPerSecond / 60f)/60f;
      Ctx.heavyAttackBounds.SetActive(true);
   }

   public override void UpdateState() {
      _animationTime += Time.deltaTime;
      _currentFrame = _animationTime / _timePerFrame;
      Debug.Log("Here");

      // Displays the current state of the attack frames.
      // Green is startup frames: No damage is given in this phase
      // Red is active frames: Damage can be given in this phase
      // Blue is recovery frames: No damage given in this phase
      if (_currentFrame <= Ctx.heavyStartupFrames.y) {
         Ctx.HeavyBoundsMat.color = Color.green;
      } else if (_currentFrame <= Ctx.heavyActiveFrames.y) {
         Ctx.HeavyBoundsMat.color = Color.red;
         Ctx.HeavyBounds.setColliderActive(true);
      } else if (_currentFrame <= Ctx.heavyRecoveryFrames.y) {
         Ctx.HeavyBoundsMat.color = Color.blue;
         Ctx.HeavyBounds.setColliderActive(false);
      } else {
         CanSwitch = true;
      }
      if (CanSwitch) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      // Debug.Log("ENEMY ROOT: EXITED HEAVY");
      Ctx.heavyAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      Ctx.Attacking = false;
      SwitchState(Factory.Idle());
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
