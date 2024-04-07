using Game.Core.ModelViewControllers;
using Game.Data;
using Game.FieldMechanics;
using UniRx;
using UnityEngine;

namespace Game.BallMechanics
{
    public class BallController : BaseController<BallData, BallModel, BallView>
    {
        public ReactiveCommand BallDestroyed { get; private set; }
        
        private readonly BallConfigData configData;
        
        private CompositeDisposable disposables;
        
        private bool isEnabled;

        public BallController(BallModel model, BallView view, BallConfigData configData) : base(model, view)
        {
            this.configData = configData;

            BallDestroyed = new ReactiveCommand();
            disposables = new CompositeDisposable();
            model.SetMovementSpeed(configData.MovementSpeed);
        }
        
        public override void Enable()
        {
            isEnabled = true;

            Model.Data.Health.Subscribe(OnHealthChanged).AddTo(disposables);
            View.EventBallPush.Subscribe(PushBall).AddTo(disposables);
            View.EventCollisionEnter.Subscribe(OnCollision).AddTo(disposables);
        }

        public override void Disable()
        {
            isEnabled = false;
            disposables.Clear();
        }
        
        public void Reset()
        {
            View.MovementRoot.localPosition = Vector3.zero;
            Model.SetMovementDirection(Vector2.zero);
            Model.SetMovementEnabled(false);
            Model.ResetHealth();
        }

        public void Update()
        {
            if (isEnabled == false)
            {
                return;
            }

            UpdateMovement();
        }

        public override void Dispose()
        {
            base.Dispose();
            
            Disable();
            disposables.Dispose();
        }

        private void OnHealthChanged(int health)
        {
            if (health <= 0)
            {
                DestroyBall();
            }
        }

        private void PushBall(Unit unit)
        {
            if (Model.IsMovementEnabled())
            {
                return;
            }
            
            Model.SetMovementDirection(-View.InputDirection.Value);
            Model.SetMovementEnabled(true);
        }

        private void UpdateMovement()
        {
            if (Model.IsMovementEnabled() == false)
            {
                return;
            }
            
            View.MovementRoot.position += Model.GetMovementVelocity();
        }

        private void DestroyBall()
        {
            BallDestroyed?.Execute();
            Reset();
        }

        private void OnCollision(Collision collision)
        {
            if (collision.transform.TryGetComponent(out IBallCollided collided))
            {
                collided.Collide(Model.Data, out BallCollisionData collisionData);

                if (collisionData.BallCollisionType == BallCollisionType.BreakCell)
                {
                    DestroyBall();
                    return;
                }
                
                if (collisionData.IsBouncing)
                {
                    Vector3 currentDirection = Model.GetMovementDirection();
                    Vector3 reflection = Vector3.Reflect(currentDirection, collision.GetContact(0).normal);
                    Model.SetMovementDirection(reflection);
                }
            }
        }
    }
}