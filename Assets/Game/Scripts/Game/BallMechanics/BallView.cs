using Game.Core.ModelViewControllers;
using Game.Systems.PlayerInput;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Game.BallMechanics
{
    public class BallView : BaseView
    {
        [field: SerializeField]
        public Transform MovementRoot { get; private set; }
        
        public Vector2ReactiveProperty InputDirection { get; private set; }
        public ReactiveCommand EventBallPush { get; private set; }
        public ReactiveCommand<Collision> EventCollisionEnter { get; private set; }
        
        private IGameInput gameInput;
        private CompositeDisposable disposables;
        
        private Vector2 downScreenPosition;

        [Inject]
        private void Construct(IGameInput gameInput)
        {
            this.gameInput = gameInput;
            
            disposables = new CompositeDisposable();
            InputDirection = new Vector2ReactiveProperty(Vector2.zero);
            EventBallPush = new ReactiveCommand();
            EventCollisionEnter = new ReactiveCommand<Collision>();
            
            gameInput.OnDown.Subscribe(OnInputDown).AddTo(disposables);
            gameInput.OnDrag.Subscribe(OnInputDrag).AddTo(disposables);
            gameInput.OnUp.Subscribe(OnInputUp).AddTo(disposables);

            this.OnCollisionEnterAsObservable().Subscribe(OnCollision).AddTo(disposables);
        }

        private void OnDestroy()
        {
            disposables.Dispose();   
        }
        
        private void OnInputDown(Vector2 screenPosition)
        {
            downScreenPosition = screenPosition;
        }

        private void OnInputDrag(Vector2 screenPosition)
        {
            InputDirection.Value = screenPosition - downScreenPosition;   
        }

        private void OnInputUp(Vector2 screenPosition)
        {
            InputDirection.Value = screenPosition - downScreenPosition;
            EventBallPush.Execute();
        }

        private void OnCollision(Collision collision)
        {
            EventCollisionEnter.Execute(collision);
        }
    }
}