using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porty
{
    class AutoDescriptor
    {
        /// <summary>
        /// Gets or sets the PID/VID associated with this auto descriptor.
        /// Any devices detected with this PID/VID will use this descriptor file
        /// </summary>
        public string PidVid { get; set; }

        /// <summary>
        /// Execture each of these commands
        /// </summary>
        public List<string> CommandSet { get; set; }
            
    }
}
