/// <summary>
/// Factory class that creates new states.
/// </summary>
public class EnemyStateFactory {
   private EnemyStateMachine _context;
   
   /// <summary>
   /// Default constructor for the factory class
   /// </summary>
   /// <param name="currentContext">Context file that is provided to all new states</param>
   public EnemyStateFactory(EnemyStateMachine currentContext) {
      _context = currentContext;
   }

   /// <summary>
   /// Creates a new EnemyIdleState
   /// </summary>
   /// <returns>new EnemyIdleState</returns>
   public EnemyBaseState Idle() {
      return new EnemyIdleState(_context, this);
   }

   /// <summary>
   /// Creates a new EnemyHurtState
   /// </summary>
   /// <returns>new EnemyHurtState</returns>
   public EnemyBaseState Hurt() {
      return new EnemyHurtState(_context, this);
   }
   
   /// <summary>
   /// Creates a new EnemyKnockedDownState
   /// </summary>
   /// <returns>new EnemyKnockedDownState</returns>
   public EnemyBaseState KnockedDown() {
      return new EnemyKnockedDownState(_context, this);
   }

   /// <summary>
   /// Creates a new EnemyStunnedState
   /// </summary>
   /// <returns>new EnemyStunnedState</returns>
   public EnemyBaseState Stunned() {
      return new EnemyStunnedState(_context, this);
   }

   /// <summary>
   /// Creates a new EnemySmackedState
   /// </summary>
   /// <returns>new EnemySmackedState</returns>
   public EnemyBaseState Smacked() {
      return new EnemySmackedState(_context, this);
   }

   /// <summary>
   /// Creates a new EnemyMovingState
   /// By Abdul
   /// </summary>
   /// <returns>new EnemyMovingState</returns>
   public EnemyBaseState Move() {
      return new EnemyMovingState(_context, this);
   }

   /// <summary>
   /// Creates a new EnemyChaseState
   /// By Abdul
   /// </summary>
   /// <returns>new EnemyChaseState</returns>
   public EnemyBaseState Chase() {
      return new EnemyChaseState(_context, this);
   }

   /// <summary>
   /// Creates a new EnemyAttackingState
   /// By Abdul
   /// </summary>
   /// <returns>new EnemyAttackingState</returns>
   public EnemyBaseState Attack() {
      return new EnemyAttackingState(_context, this);
   }

   /// <summary>
   /// Creates a new EnmeyHAttackState
   /// By Abdul
   /// </summary>
   /// <returns>new EnmeyHAttackState</returns>
   public EnemyBaseState HeavyAttack() {
      return new EnemyHAttackState(_context, this);
   }

   /// <summary>
   /// Creates a new EnemyLAttackState
   /// By Abdul
   /// </summary>
   /// <returns>new EnemyLAttackState</returns>
   public EnemyBaseState LightAttack() {
      return new EnemyLAttackState(_context, this);
   }

   /// <summary>
   /// Creates a new EnemyMAttackState
   /// By Abdul
   /// </summary>
   /// <returns>new EnemyMAttackState</returns>
   public EnemyBaseState MediumAttack() {
      return new EnemyMAttackState(_context, this);
   }
}
