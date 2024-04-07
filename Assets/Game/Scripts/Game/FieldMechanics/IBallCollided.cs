using Game.BallMechanics;
using Game.Data;

namespace Game.FieldMechanics
{
    public interface IBallCollided
    {
        void Collide(BallData ballData, out BallCollisionData collisionData);
    }
}