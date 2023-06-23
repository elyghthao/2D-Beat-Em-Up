public class PlayerStateFactory {
    private PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext) {
        _context = currentContext;
    }

    public PlayerBaseState Idle() {
        return new PlayerIdleState(_context, this);
    }

    public PlayerBaseState Forward() {
        return new PlayerForwardMovementState(_context, this);
    }

    public PlayerBaseState Backward() {
        return new PlayerBackwardMovementState(_context, this);
    }

    public PlayerBaseState Hurt() {
        return new PlayerHurtState(_context, this);
    }

    public PlayerBaseState Block() {
        return new PlayerBlockState(_context, this);
    }

    public PlayerBaseState LightAttack() {
        return new PlayerLAttackState(_context, this);
    }

    public PlayerBaseState MediumAttack() {
        return new PlayerMAttackState(_context, this);
    }

    public PlayerBaseState HeavyAttack() {
        return new PlayerHAttackState(_context, this);
    }
}
