using System;
using Game.BallMechanics;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class BallConfigData
    {
        [field: SerializeField]
        public string ID { get; private set; }
        
        [field: SerializeField]
        public BallView View { get; private set; }
        
        [field: SerializeField]
        public float MovementSpeed { get; private set; }
        
        [field: SerializeField]
        public int Health { get; private set; }
    }
}