using Godot;
using Godot.Collections;

namespace HS.Sound
{
	[Tool]
	public partial class SoundQueue : Node
	{
		// EXPORTS //////////////////////////////////////////
		/// <summary>
		/// The number of copies - on top of the original sound - to make.
		/// </summary>
		[Export] private int m_Copies = 1;
		// EXPORTS //////////////////////////////////////////

		private int m_next = 0;

		private Array<AudioStreamPlayer> m_players;

		public override void _Ready()
		{
			if(GetChildCount() == 0)
			{
				#if DEBUG
				GD.Print("//////// SOUND QUEUE WARNING ////////");
				GD.Print("No children detected in the queue!");
				GD.Print("//////// SOUND QUEUE WARNING ////////");
				#else
				// TOOD: logging system
				#endif
				return;
			}

			if (GetChild(0) is AudioStreamPlayer child)
			{
				m_players.Add(child);
				
				for(int i = 0; i < m_Copies; i++)
				{
					AudioStreamPlayer duplicate = child.Duplicate() as AudioStreamPlayer;
					AddChild(duplicate);
					m_players.Add(duplicate);
				}
			}
		}

		public bool PlaySound()
		{
			if (m_players[m_next].Playing == false)
			{
				m_players[m_next].Play();
				m_next = (m_next + 1) % m_players.Count;
				return true;
			}
			return false;
		}

		public override string[] _GetConfigurationWarnings()
		{
			if(GetChildCount() == 0)
			{
				return new string[] { "No children detected!" };
			}

			if ((GetChild(0) is AudioStreamPlayer) == false)
			{
				return new string[] { "First child is not an AudioStreamPlayer!" };
			}

			return System.Array.Empty<string>();
		}
	}
}