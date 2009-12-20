using System.Collections.Generic;
using AwManaged.Interfaces;

namespace AwManaged.Configuration.Interfaces
{
    public interface IConfigurable: ICanLog, IInitialize
    {
        IList<IConfigurable> Children { get; set; }
        IConfiguration Configuration {get;set;}
    }
}