using Game.Core.ModelViewControllers;
using UniRx;
using UnityEngine;

namespace Game.BallMechanics
{
    public class BallData : BaseData
    {
        public Vector3 MovementDirection
        {
            get => movementDirection;
            set => movementDirection = value.normalized;
        }

        public float MovementSpeed { get; set; }
        public bool IsMovementEnabled { get; set; }
        public int InitialHealth { get; set; }
        public IntReactiveProperty Health { get; set; }

        private Vector3 movementDirection;
    }
}