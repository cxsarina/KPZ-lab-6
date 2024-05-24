using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLibrary
{
    public class GameState
    {
        private readonly List<IGameObserver> observers = new List<IGameObserver>();
        private Block currentBlock;
        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }
        public Block CurrentBlock
        {
            get => currentBlock;
            set
            {
                currentBlock = value;
                currentBlock.Reset();
                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);
                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }
        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }
        public void Attach(IGameObserver observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            observers.Add(observer);
        }

        public void Detach(IGameObserver observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            observers.Remove(observer);
        }

        public void NotifyRoundStarted()
        {
            foreach (var observer in observers)
            {
                observer.RoundStarted();
            }
        }

        public void NotifyGameOver()
        {
            foreach (var observer in observers)
            {
                observer.GameOver();
            }
        }

        private bool BlockFits()
        {
            foreach (var position in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmpty(position.Row, position.Column))
                {
                    return false;
                }
            }
            return true;
        }

        public void HoldBlock()
        {
            if (!CanHold) return;

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                var temp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = temp;
            }

            CanHold = false;
        }

        public void RotateBlockCW()
        {
            RotateBlock(() => CurrentBlock.RotateCW(), () => CurrentBlock.RotateCCW());
        }

        public void RotateBlockCCW()
        {
            RotateBlock(() => CurrentBlock.RotateCCW(), () => CurrentBlock.RotateCW());
        }

        public void MoveBlockLeft()
        {
            MoveBlock(0, -1);
        }

        public void MoveBlockRight()
        {
            MoveBlock(0, 1);
        }

        public void MoveBlockDown()
        {
            MoveBlock(1, 0);
            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

        private void RotateBlock(Action rotate, Action undoRotate)
        {
            rotate();
            if (!BlockFits())
            {
                undoRotate();
            }
        }

        private void MoveBlock(int rowOffset, int colOffset)
        {
            CurrentBlock.Move(rowOffset, colOffset);
            if (!BlockFits())
            {
                CurrentBlock.Move(-rowOffset, -colOffset);
            }
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (var position in CurrentBlock.TilePositions())
            {
                GameGrid[position.Row, position.Column] = CurrentBlock.Id;
            }

            Score += 4;
            Score += GameGrid.ClearFullRows() * 10;

            if (IsGameOver())
            {
                GameOver = true;
                NotifyGameOver();
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        private int TileDropDistance(Position position)
        {
            int dropDistance = 0;
            while (GameGrid.IsEmpty(position.Row + dropDistance + 1, position.Column))
            {
                dropDistance++;
            }
            return dropDistance;
        }

        public int BlockDropDistance()
        {
            int minDropDistance = GameGrid.Rows;

            foreach (var position in CurrentBlock.TilePositions())
            {
                minDropDistance = Math.Min(minDropDistance, TileDropDistance(position));
            }
            return minDropDistance;
        }
    }
}
