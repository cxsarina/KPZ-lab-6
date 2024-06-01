using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisLibrary.Blocks;

namespace TetrisLibrary
{
    public static class BlockFactory
    {
        private static readonly Dictionary<BlockType, Func<IFigure>> blockCreators = new Dictionary<BlockType, Func<IFigure>>
        {
            { BlockType.I, () => new IBlock() },
            { BlockType.J, () => new JBlock() },
            { BlockType.L, () => new LBlock() },
            { BlockType.O, () => new OBlock() },
            { BlockType.S, () => new SBlock() },
            { BlockType.T, () => new TBlock() },
            { BlockType.Z, () => new ZBlock() }
        };

        public static IFigure CreateBlock(BlockType type)
        {
            if (blockCreators.ContainsKey(type))
            {
                return blockCreators[type]();
            }
            throw new ArgumentException("Invalid block type");
        }
    }
}
