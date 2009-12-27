using System.Collections.Generic;
namespace AwManaged.Core.Interfaces
{
    public interface ICloneableListT<TList,TItem>  
        where TList : IList<TItem>

    {
        TList Clone();
    }
}
