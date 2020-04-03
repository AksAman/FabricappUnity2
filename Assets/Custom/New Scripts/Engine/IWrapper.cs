using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace helloVoRld.NewScripts.Engine
{
    public interface IWrapper<Type>
    {
        Type MainObject { get; set; }

        void SetProperties(Type t);
    }
}
