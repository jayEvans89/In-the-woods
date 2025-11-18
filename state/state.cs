using Godot;

namespace State
{
  public partial class BaseState : Node
  {
    /// <summary>
    /// Signal emitted when a transition to a new state is requested.
    /// </summary>
    /// <param name="newStateName">The name of the new state to transition to.</param>
    [Signal]
    public delegate void TransitionEventHandler(string newStateName);

    /// <summary>
    /// Called when the state is entered.
    /// </summary>
    public virtual void Enter()
    {
    }

    /// <summary>
    /// Called when the state is exited.
    /// </summary>
    public virtual void Exit()
    {
    }

    /// <summary>
    /// Called every frame to update the state.
    /// </summary>
    /// <param name="delta">The elapsed time since the previous frame.</param>
    public virtual void Update(float delta)
    {
    }

    /// <summary>
    /// Called every physics frame to update the state.
    /// </summary>
    /// <param name="delta">The elapsed time since the previous physics frame.</param>
    public virtual void PhysicsUpdate(float delta)
    {
    }
  }
}