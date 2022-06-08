using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Map Set", menuName = "Rounds/Map Set")]
    public class MapSet : ScriptableObject
    {
        [Scene]
        [SerializeField] List<string> maps = new List<string>();

        public IReadOnlyCollection<string> Maps => maps.AsReadOnly();
    }
}
