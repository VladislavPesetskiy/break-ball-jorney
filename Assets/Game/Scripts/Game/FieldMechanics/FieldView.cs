using Game.Core.ModelViewControllers;
using UnityEngine;

namespace Game.FieldMechanics
{
    public class FieldView : BaseView
    {
        [field: SerializeField]
        public Transform BallRoot { get; private set; }
    }
}