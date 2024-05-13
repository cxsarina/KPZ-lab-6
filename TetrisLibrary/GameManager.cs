using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLibrary
{
    public class GameManager
    {
        private static GameManager instance;
        private GameState gameState;

        private GameManager()
        {
            gameState = new GameState();
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        public GameState GameState => gameState;
    }
}
