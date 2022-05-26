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

		/*[FormerlySerializedAs("m_VoiceChatChannels")]
		internal Dictionary<KcippPlayer, byte> voiceChatChannels = new Dictionary<KcippPlayer, byte>();*/

        [FormerlySerializedAs("m_VoiceChatClients")]
        internal List<KcippClient> voiceChatClients = new List<KcippClient>();

		/// <summary>The one and only KcippManager</summary>
		public static KcippManager singleton { get; internal set; }

		bool DictToList;
		bool ListToDict;

		public void Awake()
        {
            singleton = this;
			DontDestroyOnLoad(gameObject);
        }

		/*public void Update()
		{
			if (DictToList)
			{
				DictToList = false;
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
			else if (ListToDict)
			{
				ListToDict = false;
				voiceChatChannels.Clear();
				foreach (var vc in voiceChatClients)
				{
					voiceChatChannels.Add(vc.plr, vc.channel);
				}
			}
		}*/
	}
}
