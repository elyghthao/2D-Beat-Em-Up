# Current implementation:

Player movement via the arrow keys or D-pad on any controller (controller untested)

**If you wish to use movement, the scripts you need are**:
1. **PlayerStateMachine**: Root of all states, this is where many of the important functions in all player states are called.
   1. **Where does it go?**: This script should go directly on your player. In fact, it's the only script that needs to be placed on any gameobject. All other state scripts can comfortable stay where they're at or be placed in the same folder.
2. **PlayerStateFactory**: Helper class that assists with the switching of states. Ignore this unless you plan to make more states of your own. If you do, follow the template and just make your state initialization the same as every other one.
3. **PlayerBaseState**: This is mainly an abstract class for other states to derive from. However, the Player base state class does contain important functionality. Please just consult Vincent if you wish to dive into editing this. You most likely wont need to touch this class at all.
4. **PlayerBackwardMovementState**: Simply handles backward movement. If you want to change how moving backward works, go ahead and play around with what's in here. The implementation is currently exactly similar to forward movement outside of checking to switch states. This state exists separately from forward movement for the sake of animation and behavior handling in the future.
5. **PlayerForwardMovementState**: Simply handles forward movement. Functionally similar to backward movement outside of CheckSwitchStates. Same rules and purpose applies here as does what's explained in PlayerBackwardMovementState.

# WARNING
**The implementation should be fairly robust so long as you don't break too much in the root classes: PlayerStateMachine, PlayerStateFactory, and PlayerBaseState. Feel free to play around with what you want otherwise, but just beware that I make no guarantees until this system in more fleshed out.**
