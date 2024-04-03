using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Serialization;

namespace Game.Data.Models
{
    [Serializable]
    public class GameData
    {
        public IntReactiveProperty CurrentLevelIndexReactive;
    }
}