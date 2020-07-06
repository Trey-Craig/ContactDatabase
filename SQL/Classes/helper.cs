using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace SQL.Classes {
    class helper {
        public static string ConnectionValue(string name) {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
