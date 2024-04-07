using Game.Data;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Configs/Balls/" + nameof(BallsConfigs), fileName = nameof(BallsConfigs), order = 0)]
    public class BallsConfigs : ScriptableObject
    {
        [field: SerializeField]
        public BallConfigData DefaultBall { get; private set; }
    }
}