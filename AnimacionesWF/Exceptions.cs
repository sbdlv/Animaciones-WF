using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimacionesWF
{
    public class ClassDoesNotInheritFromAnimationModuleException : Exception
    {
        public Type WrongType;

        public ClassDoesNotInheritFromAnimationModuleException(Type wrongType) {
            WrongType = wrongType;
        }
    }
}
