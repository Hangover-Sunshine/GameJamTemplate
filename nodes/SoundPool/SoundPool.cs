using Godot;
using Godot.Collections;

namespace HS.Sound
{
    /// <summary>
    /// A pool for individual sounds in a stereo manner, meant for short sounds. If you have sounds that could last a while, then consider using a SoundQueuePool instead.
    /// </summary>
    [Tool]
    public partial class SoundPool : Node
    {
        // EXPORTS //////////////////////////////////////////
        /// <summary>
        /// Should the pool play the same sound back-to-back?
        /// </summary>
        [Export] private bool StopRepeatingSounds = false;
        
        /// <summary>
        /// The number of copies - on top of the original sound - to make.
        /// </summary>
        [Export] private int Copies = 5;

        /// <summary>
        /// The number of attempts to select another song before giving up and not playing.
        /// </summary>
        [Export] private int MaxAttempts = 5;
        // EXPORTS //////////////////////////////////////////
        
        public int Count
        {
            get { return Copies + 1; }
        }

        /// <summary>
        /// ID of previously played sound.
        /// </summary>
        private int m_previousIndex = -1;

        /// <summary>
        /// ID of previously played sound.
        /// </summary>
        public int PreviousIndex
        {
            get { return m_previousIndex; }
        }

        private Array<AudioStreamPlayer> m_audioPlayers;

        public override void _Ready()
        {
            Array<Node> children = GetChildren();

            for(int i = 0; i < children.Count; i++)
            {
                if(children[i] is AudioStreamPlayer player)
                {
                    m_audioPlayers.Add(player);
                }
                else
                {
                    #if DEBUG
                    GD.Print("//////// SOUND POOL WARNING ////////");
                    GD.Print($"Child {i} is not an AudioStreamPlayer! Was this intended?");
                    GD.Print("//////// SOUND POOL WARNING ////////");
                    #else
                    // TODO: use a proper logging system...
                    // If you don't, the JIT will compile out the else branch!
                    #endif
                }
            }
        }

        public bool PlayRandomSound()
        {
            int index = Autoloads.RandomUtility.RNG.RandiRange(0, m_audioPlayers.Count - 1);
            int attempts = 0;

            if (StopRepeatingSounds)
            {
                while((index == m_previousIndex || m_audioPlayers[index].Playing) && attempts < MaxAttempts)
                {
                    index = Autoloads.RandomUtility.RNG.RandiRange(0, m_audioPlayers.Count - 1);
                    attempts++;
                }

                if (m_audioPlayers[index].Playing == false)
                {
                    m_previousIndex = index;
                    m_audioPlayers[index].Play();
                    return true;
                }
            }
            else
            {
                while(m_audioPlayers[index].Playing && attempts < MaxAttempts)
                {
                    index = Autoloads.RandomUtility.RNG.RandiRange(0, m_audioPlayers.Count - 1);
                    attempts++;
                }

                if (m_audioPlayers[index].Playing == false)
                {
                    m_audioPlayers[index].Play();
                    return true;
                }
            }

            return false;
        }

        public bool PlaySound(int index)
        {
            if (index < m_audioPlayers.Count - 1 && m_audioPlayers[index].Playing == false)
            {
                m_audioPlayers[index].Play();
                return true;
            }

            return false;
        }

        public override string[] _GetConfigurationWarnings()
        {
            int childCount = 0;
            
            foreach(var v in GetChildren())
            {
                if (v is AudioStreamPlayer)
                    childCount++;
            }

            if (childCount < 2)
            {
                return new string[] { "Expected at least two AudioStreamPlayers!" };
            }

            return System.Array.Empty<string>();
        }
    }
}

