using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Information
{
    public class Author : Attribute
    {
        private string name;
        public double Version { get; set; }

        public Author(string name)
        {
            this.name = name;
            Version = 1.0;
        }
    }
}
