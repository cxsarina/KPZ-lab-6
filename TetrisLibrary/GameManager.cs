using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLibrary
{
    public class GameManager
    {
        private static GameState gameState = new GameState();

        public GameState GameState => gameState;
    }
}
