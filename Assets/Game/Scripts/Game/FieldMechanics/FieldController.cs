using System;
using Game.Core.ModelViewControllers;

namespace Game.FieldMechanics
{
    public class FieldController : BaseController<FieldData, FieldModel, FieldView>
    {
        public FieldController(FieldModel model, FieldView view) : base(model, view)
        {
        }

        public override void Enable()
        {
            
        }

        public override void Disable()
        {
            
        }

        public void NextMove(Action onReady)
        {
            onReady?.Invoke();
        }
    }
}