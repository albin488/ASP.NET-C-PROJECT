using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taxes.Models
{
    public class usercontribuable
    {
        public int id_taxea { get; set; }
        public Nullable<int> id_contria { get; set; }
        public Nullable<int> tauxa { get; set; }
        public string annea { get; set; }
        public string datea { get; set; }
        public string etata { get; set; }
        public string bordereaua { get; set; }
    }
}