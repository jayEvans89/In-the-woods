using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StateMachine : Node
{
	[Export]
	public State.BaseState CurrentState;

	public Dictionary<string, State.BaseState> States = [];

	public override void _Ready()
	{
		foreach (State.BaseState child in GetChildren().Cast<State.BaseState>())
		{
			if (child is State.BaseState)
			{
				States[child.Name] = child;
				child.Transition += OnChangeTransition;
			}
			else
			{
				GD.PushWarning("State machine contains incompatible child node " + child.Name);
			}
		}

		var owner = Owner;
		owner.Ready += OnParentReady;
	}

	private void OnParentReady()
	{
		CurrentState.Enter();
	}

	public override void _Process(double delta)
	{
		CurrentState.Update((float)delta);
		Globals.Debug.AddProperty("Current State", CurrentState.Name, 1);
	}

	public override void _PhysicsProcess(double delta)
	{
		CurrentState.PhysicsUpdate((float)delta);
	}

	public void OnChangeTransition(string newStateName)
	{
		var newState = States[newStateName];
		if (newState == null)
		{
			GD.PushWarning("State does not exist: " + newStateName);
		}

		if (newState != CurrentState)
		{
			CurrentState.Exit();
			newState.Enter();
			CurrentState = newState;
		}
	}
}
