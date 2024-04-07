using Game.BallMechanics;
using Game.Data;
using UnityEngine;

namespace Game.FieldMechanics
{
    public abstract class FieldComponent : MonoBehaviour, IBallCollided
    {
        [SerializeField]
        private BallCollisionType collisionType = BallCollisionType.Board;

        [SerializeField]
        private int collisionDamage = 0;

        [SerializeField]
        private bool isBouncing = true;
        
        public virtual void Collide(BallData ballData, out BallCollisionData collisionData)
        {
            collisionData = new BallCollisionData
            {
                BallCollisionType = collisionType,
                Damage = collisionDamage,
                IsBouncing = isBouncing 
            };
        }
    }
}