using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Operations

{
    public static class NameOperation
    {
        public static string CharRegulator(string name)
        =>  Regex.Replace(name, "[^a-z0-9]", "-");
            
        
    }
}
