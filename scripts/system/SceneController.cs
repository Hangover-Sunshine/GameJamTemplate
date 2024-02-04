using Godot;
using HS.Autoloads;

public partial class SceneController : Node
{
	// EXPORTS //////////////////////////////////////////
	[Export] private string SceneFolderPath = "scenes/";

	[Export] private Node2D CurrentScene;

	[Export] private AnimationPlayer AnimPlayer;
	
	[ExportGroup("Transition Fade")]
	[Export] private bool PlayFadeAnimInFull = false;
	[Export] private float TimeToStayFaded = 2.5f;
	// EXPORTS //////////////////////////////////////////
	
	private Node2D m_currScene = null;

	private string m_sceneName = null;
	private string m_scenePath = null;

	private bool m_fadeIn = false;

	private bool m_fadeOutComplete = false;

	public override void _Ready()
	{
		m_currScene = CurrentScene;
		GlobalSignals.SignalBus.LoadScene += LoadScene_Signal;
		SetProcess(false);
		AnimPlayer.Play("Fade");
	}

	public override void _Process(double delta)
	{
		Godot.Collections.Array prog = new Godot.Collections.Array();
		
		ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(m_scenePath, prog);

		if (status == ResourceLoader.ThreadLoadStatus.Failed)
		{
			#if DEBUG
			GD.PrintErr("//////// SCENE CONTROLLER ERROR ////////");
			GD.PrintErr($"Unable to load scene: {m_sceneName}.\nProvided path: ${m_scenePath}");
			GD.PrintErr("//////// SCENE CONTROLLER ERROR ////////");
			#else
			// TODO: add in proper logging...
			#endif

			SetProcess(false);
			return;
		}
		else if (status == ResourceLoader.ThreadLoadStatus.InvalidResource)
		{
			#if DEBUG
			GD.PrintErr("//////// SCENE CONTROLLER ERROR ////////");
			GD.PrintErr($"Unable to get scene: {m_sceneName}.\nProvided path: ${m_scenePath}");
			GD.PrintErr("//////// SCENE CONTROLLER ERROR ////////");
			#else
			// TODO: add in proper logging...
			#endif

			SetProcess(false);
			return;
		}

		float progress = (float)prog[0];

		if (progress >= 1.0f && (PlayFadeAnimInFull == false || m_fadeOutComplete))
		{
			// Done!
			// Emit signal that the scene has been loaded
			m_currScene.ZIndex = -100;
			GlobalSignals.SignalBus.EmitSignal(GlobalSignals.SignalName.SceneLoaded, m_sceneName);

			PackedScene sceneResource = ResourceLoader.LoadThreadedGet(m_scenePath).Duplicate(true) as PackedScene;
			
			// Now we actually have the scene!
			Node2D scene = sceneResource.Instantiate() as Node2D;
			m_currScene = scene;
			AddChild(scene);
			
			m_fadeIn = true;
			AnimPlayer.Play("Fade");

			SetProcess(false);
		}
		else
		{
			// Still loading...
			GD.Print($"Progress: {Mathf.Floor(progress * 100)}%");
		}
	}

	public void LoadScene_Signal(string sceneName)
	{
		m_sceneName = sceneName;
		m_scenePath = "res://" + SceneFolderPath + sceneName + ".tscn";

		Error error = ResourceLoader.LoadThreadedRequest(m_scenePath);

		if (error != 0)
		{
			#if DEBUG
			GD.PrintErr("//////// SCENE CONTROLLER ERROR ////////");
			GD.PrintErr($"A critical error occured! Unable to load scene as a request: {m_sceneName}.\nProvided path: ${m_scenePath}");
			GD.PrintErr("//////// SCENE CONTROLLER ERROR ////////");
			#else
			// TODO: add in proper logging...
			#endif
			return;
		}

		// emit signal that we've begun transitioning
		GlobalSignals.SignalBus.EmitSignal(GlobalSignals.SignalName.SceneTransitionStarted);

		m_fadeIn = false;
		m_fadeOutComplete = false;
		AnimPlayer.PlayBackwards("Fade");

		SetProcess(true);
	}

	public void OnFadeFinished(string animationName)
	{
		if (m_fadeIn)
		{
			GlobalSignals.SignalBus.EmitSignal(GlobalSignals.SignalName.SceneTransitionFinished);
		}

		if (PlayFadeAnimInFull)
		{
			m_fadeOutComplete = true;
		}
	}
}
