using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL.Classes {
    class properties {
        public int id { get; set;}

        public string first_name {get; set; }
        public string last_name {get; set; }

        public string cell {get; set; }

        public string work {get; set; }
        public string email {get; set; }

        public string mail {get; set;}

        public string notes {get; set;}

        public Boolean active {get; set;}

        public byte[] image {get; set;}

        public string ResultData{
            get{return first_name + " " + last_name;} 
            }
    }
}
