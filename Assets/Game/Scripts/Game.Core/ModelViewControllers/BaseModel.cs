using System;

namespace Game.Core.ModelViewControllers
{
    public abstract class BaseModel<TData> : IDisposable where TData : BaseData
    {
        public readonly TData Data;
        
        public BaseModel(TData data)
        {
            Data = data;
        }
        
        public abstract void Dispose();
    }
}