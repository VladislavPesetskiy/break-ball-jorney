using Game.Core.ModelViewControllers;

namespace Game.FieldMechanics
{
    public class FieldModel : BaseModel<FieldData>
    {
        public FieldModel(FieldData data) : base(data)
        {
        }

        public override void Dispose()
        {
            
        }
    }
}