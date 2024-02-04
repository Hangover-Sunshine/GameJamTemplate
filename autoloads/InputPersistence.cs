using Godot;
using Godot.Collections;

namespace HS.Input
{
    public partial class InputPersistence : Node
    {
        // NOTE: this implements a single, global keymapping system for ALL saves/players on the system running.
        //  TODO: maybe something that allows players to save different mappings out?

        public static InputPersistence Singleton { get; private set; }
        
        // All information regarding the (local) player are stroed in player/ for convenience in Hangover Sunshine games
        private const string KEY_MAPS_DIR = "user://player/keybindings.data";

        // Controller layouts are not custom, and probably pre-made for each game. Sorry controllers!

        private Dictionary<string, InputEvent> m_actionToKey;

        public override void _Ready()
        {
            Singleton = this;

            m_actionToKey = new Dictionary<string, InputEvent>();

            foreach(string action in InputMap.GetActions())
            {
                // A strong assumption that our/your InputMaps will only ever allow ONE key
                m_actionToKey[action] = InputMap.ActionGetEvents(action)[0];
            }

            LoadKeyMapping();
        }

        public void LoadKeyMapping()
        {
            // If it doesn't exist, make it and be done
            if (FileAccess.FileExists(KEY_MAPS_DIR) == false)
            {
                SaveKeyMapping();
                return;
            }

            // Open -> read and deserialize -> close
            FileAccess file = FileAccess.Open(KEY_MAPS_DIR, FileAccess.ModeFlags.Read);
            Dictionary<string, InputEvent> temp_keymap = (Dictionary<string, InputEvent>) file.GetVar(true);
            file.Close();

            // Go through every key and set up the actual, real InputEvent
            foreach(string key in temp_keymap.Keys)
            {
                m_actionToKey[key] = temp_keymap[key];

                // Now change the InputMap
                InputMap.ActionEraseEvents(key);
                InputMap.ActionAddEvent(key, temp_keymap[key]);
            }
        }

        public void SaveKeyMapping()
        {
            FileAccess file = FileAccess.Open(KEY_MAPS_DIR, FileAccess.ModeFlags.Write);
            file.StoreVar(m_actionToKey, true);
            file.Close();
        }
    }
}
