using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayerNew.Warnings
{
    public static class Warning
    {
        public enum Types
        {
            EAC,
            EAI,
            ECC,
            ECI,
            ECA
        }

        public static TipoAviso Get(Types type)
        {
            TipoAviso av;

            using (ModelMyHealth context = new ModelMyHealth())
            {
                av = context.TipoAvisoSet.FirstOrDefault(i => i.Nome.Equals(type.ToString()));
            }

            return av;
        }
        
    }
}

    

