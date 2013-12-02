using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbUpdateApp
{
    public class ScriptFileException:Exception
    {
        public ScriptFileException(ScriptFile sf):base(sf.Name)
        {
            
        }
    }
}
