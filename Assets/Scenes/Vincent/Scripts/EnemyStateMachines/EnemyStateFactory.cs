public class EnemyStateFactory {
   private EnemyStateMachine _context;
   
   public EnemyStateFactory(EnemyStateMachine currentContext) {
      _context = currentContext;
   }

   public EnemyBaseState Idle() {
      return new EnemyIdleState(_context, this);
   }

   public EnemyBaseState Hurt() {
      return new EnemyHurtState(_context, this);
   }

   public EnemyBaseState KnockedDown() {
      return new EnemyKnockedDownState(_context, this);
   }
}
