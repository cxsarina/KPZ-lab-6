using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLibrary
{
    public class User
    {
        public string Name { get; set; }
        public int Score_Easy { get; set; }
        public int Score_Normal { get; set; }
        public int Score_Hard { get; set; }
        public User() 
        {
            Name = string.Empty;
            Score_Easy = 0;
            Score_Normal = 0;
            Score_Hard = 0;
        }
    }
}
