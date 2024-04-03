using UniRx;
using UnityEngine;

namespace Game.Data.Models
{
    public class GameDataModel : DataModel<GameData>, IAutoBindDataModel
    {
        public IReadOnlyReactiveProperty<int> CurrentLevelIndexReactive => Data.CurrentLevelIndexReactive ??= new IntReactiveProperty(0);
        public int CurrentLevelIndex
        {
            get => CurrentLevelIndexReactive.Value;
            set 
            {
                if (value < 0)
                {
                    Debug.LogError($"You try set level {value}. Level index can't be negative!");
                    return;
                }
                
                Data.CurrentLevelIndexReactive.Value = value;
            }
        }
    }
}