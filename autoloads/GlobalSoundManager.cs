using Godot;
using System.Collections.Generic;

namespace HS.Sound
{
	[Tool]
	public partial class GlobalSoundManager : Node
	{
		public GlobalSoundManager SoundManager { get; private set; }

		Dictionary<string, SoundQueue> m_soundQueues;
		Dictionary<string, SoundPool> m_soundPools;
		Dictionary<string, SoundQueuePool> m_soundQueuePools;

		public override void _Ready()
		{
			SoundManager = this;

			// Load songs/sfx from here
		}

		public bool PlaySoundFromQ(string name)
		{
			if (m_soundQueues.TryGetValue(name, out SoundQueue val))
			{
				return val.PlaySound();
			}
			return false;
		}

		public bool PlaySoundFromP(string name)
		{
			if (m_soundPools.TryGetValue(name, out SoundPool val))
			{
				return val.PlayRandomSound();
			}
			return false;
		}

		public bool PlaySoundFromP(string name, int index)
		{
			if (m_soundPools.TryGetValue(name, out SoundPool val))
			{
				return val.PlaySound(index);
			}
			return false;
		}

		public bool PlaySoundFromQP(string name)
		{
			if (m_soundQueuePools.TryGetValue(name, out SoundQueuePool val))
			{
				val.PlayRandomSound();
				return true;
			}
			return false;
		}

		public bool PlaySoundFromQP(string name, int index)
		{
			if (m_soundQueuePools.TryGetValue(name, out SoundQueuePool val))
			{
				return val.PlaySound(index);
			}
			return false;
		}
	}
}

