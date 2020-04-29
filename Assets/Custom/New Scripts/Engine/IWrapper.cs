using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace helloVoRld.NewScripts.Engine
{
    /// <summary>
    /// Base Wrapper for UnityObjects that will be converted to/from JSON
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    public interface IWrapper<Type>
    {
        /// <summary>
        /// Reference to object of interest
        /// </summary>
        Type MainObject { get; set; }

        /// <summary>
        /// Used as constructor and to get information from the current object to make class ready to be called from JSONSerializer.
        /// </summary>
        /// <param name="t"></param>
        void SetProperties(Type t);
    }
}
