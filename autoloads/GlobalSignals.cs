using Godot;

namespace HS.Autoloads
{
    public partial class GlobalSignals : Node
    {
        // SYSTEM LOADING //////////////////////////////////////////
        [Signal] public delegate void LoadSceneEventHandler (string sceneName);
        [Signal] public delegate void SceneLoadedEventHandler(string newSceneName);
        [Signal] public delegate void SceneTransitionStartedEventHandler();
        [Signal] public delegate void SceneTransitionFinishedEventHandler();
        // SYSTEM LOADING //////////////////////////////////////////
    }
}
