using Godot;

public partial class SceneController : Node
{
	// EXPORTS //////////////////////////////////////////
	[Export] private string SceneFolderPath = "scenes/";

	[Export] private Node CurrentScene;

	[Export] private AnimationPlayer AnimPlayer;
	// EXPORTS //////////////////////////////////////////
	
	private Node2D m_currScene = null;

	private string m_sceneName = null;
	private string m_scenePath = null;

	private bool m_fadeIn = false;

	public override void _Ready()
	{
		// TODO: connect a signal
		SetProcess(false);
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

		if (progress >= 1.0f)
		{
			// Done!
			// Emit signal that the scene has been loaded
			m_currScene.ZIndex = -100;

			// Slight bit of work around because of C#
			Resource sceneResource = ResourceLoader.LoadThreadedGet(m_scenePath);
			sceneResource.ResourceLocalToScene = true;
			
			// Now we actually have the scene!
			Node2D scene = sceneResource.GetLocalScene() as Node2D;
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

		m_fadeIn = false;
		AnimPlayer.PlayBackwards("Fade");

		SetProcess(true);
	}
}
