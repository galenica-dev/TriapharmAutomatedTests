using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFromDb
{
    public class ProductLore
    {
        public string GtinEanCode { get; set; }
        public string Pharmacode { get; set; }
        public string Description { get; set; }
        public string Error { get; set; }

        public override string ToString()
        {
            return $@"
            GtinEanCode: {GtinEanCode},
            Pharmacode: {Pharmacode},
            Description: {Description}
            Error: {Error}";
        }
    }
}

    
