using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kcipp
{
	[AddComponentMenu("Kcipp/Kcipp Manager")]
	public class KcippManager : MonoBehaviour
	{
		[FormerlySerializedAs("m_Proximity")]
		public bool proximity = false;

		[FormerlySerializedAs("m_MaxDistance")]
		public float maxDistance = 500f / 2f;

		[FormerlySerializedAs("m_MicFrequency")]
		[Tooltip("Its frequency of input (mic) in khz")]
		public int micFrequency;

		[FormerlySerializedAs("m_MicRecordLenght")]
		[Tooltip("Its in loop but its for saving resources (because audio clip cannot be eg. 1hour long)")]
		public int micRecordLenght = 120;

		[FormerlySerializedAs("m_MicSamplePacketSize")]
		[Tooltip("If its lower you might hear \"clipping\"")]
		public int micSamplePacketSize = 7350;

		[FormerlySerializedAs("m_VoiceChatChannels"), HideInInspector]
		public Dictionary<KcippPlayer, byte> voiceChatChannels = new Dictionary<KcippPlayer, byte>();

        [FormerlySerializedAs("m_VoiceChatClients"), HideInInspector]
        public List<KcippClient> voiceChatClients = new List<KcippClient>();

		/// <summary>The one and only KcippManager</summary>
		public static KcippManager singleton { get; internal set; }

		bool dictToList;
		bool listToDict;

		public void Reset()
		{
			micFrequency = AudioSettings.outputSampleRate;
		}

		public void Awake()
        {
            singleton = this;
			DontDestroyOnLoad(gameObject);
        }

		public void Update()
		{
			if (dictToList)
			{
				dictToList = false;
				voiceChatClients.Clear();
				foreach (var kvp in voiceChatChannels)
				{
					KcippClient newClient = new KcippClient()
					{
						plr = kvp.Key,
						channel = kvp.Value
					};

					voiceChatClients.Add(newClient);
				}
			}
			else if (listToDict)
			{
				listToDict = false;
				voiceChatChannels.Clear();
				foreach (var vc in voiceChatClients)
				{
					voiceChatChannels.Add(vc.plr, vc.channel);
				}
			}
		}
	}
}
