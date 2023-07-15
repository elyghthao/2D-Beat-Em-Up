/// <summary>
/// Factory class that creates new states.
/// </summary>
public class PlayerStateFactory {
    private PlayerStateMachine _context;

    /// <summary>
    /// Default constructor for the factory class
    /// </summary>
    /// <param name="currentContext">Context file that is provided to all new states</param>
    public PlayerStateFactory(PlayerStateMachine currentContext) {
        _context = currentContext;
    }
    
    /// <summary>
    /// Creates a new PlayerIdleState
    /// </summary>
    /// <returns>new PlayerIdleState</returns>
    public PlayerBaseState Idle() {
        return new PlayerIdleState(_context, this);
    }

    /// <summary>
    /// Creates new PlayerMoveState
    /// </summary>
    /// <returns>new PlayerMoveState</returns>
    public PlayerBaseState Move() {
        return new PlayerMoveState(_context, this);
    }

    /// <summary>
    /// Create new PlayerForwardMovementState
    /// </summary>
    /// <returns>new PlayerForwardMovementState</returns>
    public PlayerBaseState Forward() {
        return new PlayerForwardMovementState(_context, this);
    }

    /// <summary>
    /// Creates new PlayerBackwardMovementState
    /// </summary>
    /// <returns>new PlayerBackwardMovementState</returns>
    public PlayerBaseState Backward() {
        return new PlayerBackwardMovementState(_context, this);
    }

    /// <summary>
    /// Creates new PlayerHurtState
    /// </summary>
    /// <returns>new PlayerHurtState</returns>
    public PlayerBaseState Hurt() {
        return new PlayerHurtState(_context, this);
    }

    /// <summary>
    /// Creates new PlayerAttackState
    /// </summary>
    /// <returns>new PlayerAttackState</returns>
    public PlayerBaseState Attack() {
        return new PlayerAttackState(_context, this);
    }

    /// <summary>
    /// Creates new PlayerBlockState
    /// </summary>
    /// <returns>new PlayerBlockState</returns>
    public PlayerBaseState Block() {
        return new PlayerBlockState(_context, this);
    }

    /// <summary>
    /// Creates new PlayerLightAttackState
    /// </summary>
    /// <returns>new PlayerLightAttackState</returns>
    public PlayerBaseState LightAttack() {
        return new PlayerLAttackState(_context, this);
    }

    /// <summary>
    /// Creates new PlayerMediumAttackState
    /// </summary>
    /// <returns>new PlayerMediumAttackState</returns>
    public PlayerBaseState MediumAttack() {
        return new PlayerMAttackState(_context, this);
    }

    /// <summary>
    /// Creates new PlayerHeavyAttackState
    /// </summary>
    /// <returns>new PlayerHeavyAttackState</returns>
    public PlayerBaseState HeavyAttack() {
        return new PlayerHAttackState(_context, this);
    }
}
