using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Tir
    {
        public int coord { get; }
        public bool hit { get; set; }
        public string status { get; set; }

        public Tir(int coord)
        {
            this.coord = coord;
            this.hit = false;
        }
    }
}
