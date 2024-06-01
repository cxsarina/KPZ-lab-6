using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLibrary
{
    public interface IFigure
    {
        int Id { get; }
        IEnumerable<Position> TilePositions();
        void RotateCW();
        void RotateCCW();
        void Move(int rows, int columns);
        void Reset();
    }
}
