using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimacionesWF
{
    public class XMLTagNameAttribute : System.Attribute
    {
        public String XMLTagName;

        public XMLTagNameAttribute(string xMLTagName)
        {
            XMLTagName = xMLTagName;
        }
    }
}
