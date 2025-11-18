
using System.Threading.Tasks;
using Godot;

namespace State
{
  public partial class PlayerMovementState : BaseState
  {
    /// <summary>
    /// Reference to the PlayerController node.
    /// </summary>
    public PlayerController Player;

    /// <summary>
    /// Reference to the AnimationPlayer on the player for handling animations.
    /// </summary>
    public AnimationPlayer Animation;

    public override async void _Ready()
    {
      var owner = Owner;
      owner.Ready += OnParentReady;
    }

    private void OnParentReady()
    {
      Player = Owner as PlayerController;
      Animation = Player.AnimationPlayer;
    }
  }
}