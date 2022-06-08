using Com.Dot.SZN.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.Dot.SZN
{
    public class MapHandler
    {
        readonly IReadOnlyCollection<string> maps;
        readonly int numberOfRounds;

        int currentRound;
        List<string> remainingMaps;

        public MapHandler(MapSet mapSet, int numberOfRounds)
        {
            maps = mapSet.Maps;
            this.numberOfRounds = numberOfRounds;

            ResetMaps();
        }

        public bool IsComplete => currentRound == numberOfRounds;

        public string NextMap
        {
            get
            {
                if (IsComplete) { return null; }

                currentRound++;

                if (remainingMaps.Count == 0) { ResetMaps(); }

                string map = remainingMaps[Random.Range(0, remainingMaps.Count)];

                remainingMaps.Remove(map);

                return map;
            }
        }

        void ResetMaps() => remainingMaps = maps.ToList();
    }
}

