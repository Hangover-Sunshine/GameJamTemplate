using Godot;
using HS.Autoloads;

public partial class DummySceneScript : Node2D
{

	// EXPORTS ////////////////////////
	[Export] private string NextScene;
	// EXPORTS ////////////////////////

	public override void _Ready()
	{
		GlobalSignals.SignalBus.SceneLoaded += SceneLoaded_Signal;
	}
	
	public void SceneLoaded_Signal(string newScene)
	{
		if (IsInstanceValid(this))
		{
			GD.Print($"{newScene} == {Name}: {newScene == Name}");
			QueueFree();
		}
	}

	public void OnButtonPressed()
	{
		GlobalSignals.SignalBus.EmitSignal(GlobalSignals.SignalName.LoadScene, NextScene);
	}
}
