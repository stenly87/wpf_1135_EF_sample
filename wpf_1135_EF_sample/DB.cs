using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace wpf_1135_EF_sample
{
    internal class DB
    {
        static _1135New2024Context context = new _1135New2024Context();

        public static _1135New2024Context Instance
        { 
            get => context; 
        }
    }
}
