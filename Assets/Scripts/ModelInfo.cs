using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN
{
    [System.Serializable]
    public class ModelInfo
    {
        public GameObject model;

        [Header("Transform")]
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public ModelInfo()
        {
            model = null;
            position = Vector3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.one;
        }
    }
}
