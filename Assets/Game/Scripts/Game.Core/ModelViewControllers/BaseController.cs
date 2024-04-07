using System;
using Object = UnityEngine.Object;

namespace Game.Core.ModelViewControllers
{
    public abstract class BaseController<TData, TModel, TView> : IDisposable
        where TData : BaseData
        where TModel : BaseModel<TData>
        where TView : BaseView
    {
        public readonly TModel Model;
        public readonly TView View;
        
        public BaseController(TModel model, TView view)
        {
            Model = model;
            View = view;
        }

        public abstract void Enable();
        public abstract void Disable();
        
        public virtual void Dispose()
        {
            Model.Dispose();
            Object.Destroy(View.gameObject);
        }
    }
}