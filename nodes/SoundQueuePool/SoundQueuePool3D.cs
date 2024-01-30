using System.Linq;
using Godot;
using Godot.Collections;

namespace HS.Sound
{
	[Tool]
	public partial class SoundQueuePool3D : Node3D
	{
		// EXPORTS //////////////////////////////////////////
        /// <summary>
        /// Should the pool play the same sound back-to-back?
        /// </summary>
        [Export] private bool StopRepeatingSounds = false;
		// EXPORTS //////////////////////////////////////////
        
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

		private Array<SoundQueue3D> m_audioPlayers;

		public override void _Ready()
		{
            Array<Node> children = GetChildren();

			for(int i = 0; i < children.Count; i++)
            {
                if (children[i] is SoundQueue3D sq)
                {
                    m_audioPlayers.Append(sq);
                }
                else
                {
                    #if DEBUG
                    GD.Print("//////// SOUND QUEUE POOL 3D WARNING ////////");
                    GD.Print($"Non-SoundQueue object detected as a child at index {i}!");
                    GD.Print("//////// SOUND QUEUE POOL 3D WARNING ////////");
                    #else
                    // TOOD: logging system
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
                while(index == m_previousIndex)
                {
                    index = Autoloads.RandomUtility.RNG.RandiRange(0, m_audioPlayers.Count - 1);
                    attempts++;
                }

                m_previousIndex = index;
                return m_audioPlayers[index].PlaySound();
            }
            else
            {
                return m_audioPlayers[index].PlaySound();
            }
        }

        public bool PlaySound(int index)
        {
            if (index < m_audioPlayers.Count - 1)
            {
                m_audioPlayers[index].PlaySound();
                return true;
            }

            return false;
        }

		public override string[] _GetConfigurationWarnings()
		{
			int childCount = 0;
            
            foreach(var v in GetChildren())
            {
                if (v is SoundQueue3D)
                    childCount++;
            }

            if (childCount < 2)
            {
                return new string[] { "Expected at least two SoundQueue3D!" };
            }

            return System.Array.Empty<string>();
		}
	}
}
