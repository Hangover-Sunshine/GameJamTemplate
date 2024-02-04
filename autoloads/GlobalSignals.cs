using Godot;

namespace HS.Autoloads
{
    public partial class GlobalSignals : Node
    {
        public static GlobalSignals SignalBus { get; private set; }

        public override void _Ready()
        {
            SignalBus = GetNode<GlobalSignals>("/root/GlobalSignals");
        }

        // SYSTEM LOADING //////////////////////////////////////////
        [Signal] public delegate void LoadSceneEventHandler (string sceneName);
        [Signal] public delegate void SceneLoadedEventHandler(string newSceneName);
        [Signal] public delegate void SceneTransitionStartedEventHandler();
        [Signal] public delegate void SceneTransitionFinishedEventHandler();
        // SYSTEM LOADING //////////////////////////////////////////
    }
}
