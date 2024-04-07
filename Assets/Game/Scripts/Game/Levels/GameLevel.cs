using Game.BallMechanics;
using Game.Core.Configurations;
using Game.Core.Levels;
using Game.Factories;
using Game.FieldMechanics;
using Game.Systems.PlayerInput;
using UnityEngine;
using Zenject;

namespace Game.Levels
{
    public class GameLevel : Level
    {
        [SerializeField]
        private FieldView fieldView;
        
        public BallController CurrentBallController { get; private set; }
        public FieldController CurrentFieldController { get; private set; }

        private BallFactory ballFactory;
        private IGameInput gameInput;

        [Inject]
        private void Construct(BallFactory ballFactory, IGameInput gameInput)
        {
            this.ballFactory = ballFactory;
            this.gameInput = gameInput;
        }

        public override void Initialize(LevelSettings levelSettings)
        {
            base.Initialize(levelSettings);

            CurrentBallController = ballFactory.CreateBallController(fieldView.BallRoot);
            CurrentFieldController = CreateField();
        }

        public override void Show()
        {
            base.Show();

            StartGame();
        }

        public override void DeInitialize()
        {
            base.DeInitialize();
            
            gameInput.SetEnabled(false);
            CurrentBallController?.Dispose();
            CurrentFieldController?.Dispose();
        }

        public void StartGame()
        {
            gameInput.SetEnabled(true);
            CurrentBallController.Enable();
        }

        private FieldController CreateField()
        {
            var fieldData = new FieldData();
            var fieldModel = new FieldModel(fieldData);
            return new FieldController(fieldModel, fieldView);
        }

        private void Update()
        {
            CurrentBallController?.Update();
        }
    }
}