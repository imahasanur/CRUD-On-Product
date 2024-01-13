using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam1.Domain.Exceptions
{
    public class DuplicateNameException:Exception
    {
        public DuplicateNameException():base("Title is Duplicate")
        {

        }
    }
}
