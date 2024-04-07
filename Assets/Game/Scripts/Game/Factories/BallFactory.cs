using Game.BallMechanics;
using Game.Configs;
using Game.Data;
using UnityEngine;
using Zenject;

namespace Game.Factories
{
    public class BallFactory
    {
        private readonly BallsConfigs ballsConfigs;
        private readonly DiContainer diContainer;

        private BallConfigData BallConfig => ballsConfigs.DefaultBall;

        public BallFactory(BallsConfigs ballsConfigs, DiContainer diContainer)
        {
            this.ballsConfigs = ballsConfigs;
            this.diContainer = diContainer;
        }

        public BallController CreateBallController(Transform ballRoot)
        {
            BallView ballView = CreateView(ballRoot);
            BallModel ballModel = CreateBallModel();
            
            var ballController = new BallController(ballModel, ballView, BallConfig);
            
            return ballController;
        }
        
        private BallView CreateView(Transform root)
        {
            var ballView = diContainer.InstantiatePrefab(BallConfig.View).GetComponent<BallView>();
            ballView.transform.SetParent(root);
            return ballView;
        }

        private BallModel CreateBallModel()
        {
            var ballData = new BallData
            {
                InitialHealth = BallConfig.Health
            };
            var ballModel = new BallModel(ballData);
            return ballModel;
        }
    }
}