using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.LiteDb
{
    public interface ILiteDbConfiguration
    {
        /// <summary>
        /// Gets or sets the LiteDb database path.
        /// </summary>
        /// <value>
        /// The LiteDb database path.
        /// </value>
        string DatabasePath { get; set; }
    }
}
