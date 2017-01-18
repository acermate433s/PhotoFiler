using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoFiler.Logged
{
    public interface ILoggedConfiguration
    {
        /// <summary>
        /// Flag to indicate if logging is generated
        /// </summary>
        bool EnableLogging { get; set; }
    }
}
