using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLibrary
{
    public static class BlockFactory
    {
        public static Block CreateBlock(BlockType type)
        {
            switch (type)
            {
                case BlockType.I: return new IBlock();
                case BlockType.J: return new JBlock();
                case BlockType.L: return new LBlock();
                case BlockType.O: return new OBlock();
                case BlockType.S: return new SBlock();
                case BlockType.T: return new TBlock();
                case BlockType.Z: return new ZBlock();
                default: throw new ArgumentException("Invalid block type");
            }
        }
    }
}
