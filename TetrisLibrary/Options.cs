using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TetrisLibrary
{
    public class Options
    {
        public int Theme { get; set; }
        public int Difficult { get; set; }
        public int Language { get; set; }
        public int Ghost_Block { get; set; }
        public Options() 
        {
            Theme = 0;
            Difficult = 0;
            Language = 0;
            Ghost_Block = 0;
        }
    }
}
