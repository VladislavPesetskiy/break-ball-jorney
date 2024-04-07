using Game.Core.ModelViewControllers;
using UniRx;
using UnityEngine;

namespace Game.BallMechanics
{
    public class BallModel : BaseModel<BallData>
    {
        public BallModel(BallData data) : base(data)
        {
            Data.Health = new IntReactiveProperty(Data.InitialHealth);
        }

        public Vector3 GetMovementVelocity()
        {
            return Data.MovementDirection * (Data.MovementSpeed * Time.deltaTime);
        }

        public Vector3 GetMovementDirection()
        {
            return Data.MovementDirection;
        }

        public void SetMovementDirection(Vector2 direction)
        {
            Data.MovementDirection = new Vector3(direction.x, 0f, direction.y);
        }
        
        public void SetMovementDirection(Vector3 direction)
        {
            Data.MovementDirection = new Vector3(direction.x, 0, direction.z);
        }

        public void SetMovementSpeed(float speed)
        {
            Data.MovementSpeed = speed;
        }

        public bool IsMovementEnabled()
        {
            return Data.IsMovementEnabled;
        }

        public void SetMovementEnabled(bool isEnabled)
        {
            Data.IsMovementEnabled = isEnabled;
        }

        public int GetHealth()
        {
            return Data.Health.Value;
        }

        public void SetHealth(int health)
        {
            Data.Health.Value = health;
        }

        public void ResetHealth()
        {
            Data.Health.Value = Data.InitialHealth;
        }

        public override void Dispose()
        {
            Data.MovementSpeed = 0f;
            Data.MovementDirection = Vector3.zero;
        }
    }
}