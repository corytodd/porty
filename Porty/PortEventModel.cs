using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porty
{
    class PortEventModel
    {
        public String PortName { get; set; }
        public String PID { get; set; }
        public String VID { get; set; }

        public Boolean isComplete
        {
            get
            {
                return (!String.IsNullOrEmpty(PortName) &&
                    !String.IsNullOrEmpty(PID) &&
                    !String.IsNullOrEmpty(VID));
            }
        }

        /// <summary>
        /// Extract the PID/VID string from the dcbb.name struct field. The original string 
        /// is full of _ and / and # and is tailed with a GUID. We don't want that stuff.
        /// </summary>
        /// <param name="input"></param>
        public void ExtractPIDVID(string input)
        {
            var start = input.IndexOf("PID");
            PID = input.Substring(start+4, 4);

            start = input.IndexOf("VID");
            VID = input.Substring(start+4, 4);
        }

        override public String ToString()
        {
            return String.Format("{0}\nPID: {1} VID: {2}", PortName, PID, VID);
        }
    }
}
