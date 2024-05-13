## Shkolna Aryna
### Patterns
1. Factory Method: This pattern is used to create objects of various block types in a queue.
#### Create class BlockFactory:
``` 
public class BlockFactory
    {
        public Block CreateBlock(BlockType type)
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
```
#### And create BlockType:
```
public enum BlockType
    {
        I,
        J,
        L,
        O,
        S,
        T,
        Z
    }
```
2. Observer: This pattern is used to create a mechanism for subscribing to game events, such as the "new round" event, "game over" event, etc.
#### Create class interface IGameObserver:
```
 public interface IGameObserver
    {
        void RoundStarted();
        void GameOver();
    }
```
#### And modify class GameState:
```
public class GameState
    {
        private List<IGameObserver> observers = new List<IGameObserver>();

        public void Attach(IGameObserver observer)
        {
            observers.Add(observer);
        }
        public void Detach(IGameObserver observer)
        {
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
        //code continuation.....
```
3. Singleton: This pattern is used to create a single instance of the game that will be accessible from anywhere in the program.
#### Create class GameManager:
```
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
```
### Principles
1. DRY (Don’t Repeat Yourself): This code uses this principle because the code uses the `CreateBlock` factory method in
the `Block` class and `BlockFactory` to create blocks of different types, instead of repeating this code in each block class.
#### Class Block:
```
 public abstract class Block
 {
     protected abstract Position[][] Tiles { get; }
     protected abstract Position StartOffset { get; }
     public abstract int Id { get; }
     private int rotationState;
     private Position offset;
     public Block()
     {
         offset = new Position(StartOffset.Row, StartOffset.Column);
     }

     public IEnumerable<Position> TilePositions()
     {
         foreach (Position p in Tiles[rotationState])
         {
             yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
         }
     }

     public void RotateCW()
     {
         rotationState = (rotationState + 1) % Tiles.Length;
     }

     public void RotateCCW()
     {
         if (rotationState == 0)
         {
             rotationState = Tiles.Length - 1;
         }
         else
         {
             rotationState--;
         }
     }

     public void Move(int rows, int columns)
     {
         offset.Row += rows;
         offset.Column += columns;
     }

     public void Reset()
     {
         rotationState = 0;
         offset.Row = StartOffset.Row;
         offset.Column = StartOffset.Column;
     }

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
```
#### Class BlockFactory:
```
public class BlockFactory
    {
        public Block CreateBlock(BlockType type)
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
```
2. KISS (Keep It Simple, Stupid): The code is simple and clear. For example, the `Move` method in the `Block` class simply changes the block's position,
while the `RotateCW` method rotates the block clockwise.
#### Method Move in the Block class:
```
 public void Move(int rows, int columns)
 {
     offset.Row += rows;
     offset.Column += columns;
 }
```
#### Method RotateCW:
```
 public void RotateCW()
 {
     rotationState = (rotationState + 1) % Tiles.Length;
 }
```
3. SOLID:
   - Single Responsibility Principle (SRP): Each class in the code has one responsibility.
   For example, the `Block` class is responsible for managing a block, and the `BlockQueue` class is responsible for managing a queue of blocks.
#### Class BlockQueue:
  ```
 public class BlockQueue
 {
     private readonly Block[] blocks = new Block[]
     {
         new IBlock(), new JBlock(), new LBlock(), new OBlock(), new SBlock(), new TBlock(), new ZBlock()
     };
     private readonly Random random = new Random();
     public Block NextBlock { get; private set; }
     public BlockQueue()
     {
         NextBlock = RandomBlock();
     }
     private Block RandomBlock()
     {
         return blocks[random.Next(blocks.Length)];
     }
     public Block GetAndUpdate()
     {
         Block block = NextBlock;
         do
         {
             NextBlock = RandomBlock();
         }
         while (block.Id == NextBlock.Id);

         return block;
     }
 }
```
  - Open-Closed Principle (OCP): The code is open for extension, but closed for modification. For example,
  you can add a new block type by creating a new class that inherits from Block without changing the existing code.
   
#### For Example Class IBlock:
```
public class IBlock : Block
{
    private readonly Position[][] tiles = new Position[][]
    {
        new Position[] {new(1,0),new(1,1),new(1,2),new(1,3)},
        new Position[] {new(0,2),new(1,2),new(2,2),new(3,2)},
        new Position[] {new(2,0),new(2,1),new(2,2),new(2,3)},
        new Position[] {new(0,1),new(1,1),new(2,1),new(3,1)}
    };
    public override int Id => 1;
    protected override Position StartOffset => new Position(-1,3);
    protected override Position[][] Tiles => tiles;
}
```
  - Liskov Substitution Principle (LSP): The code uses this principle because objects of classes that inherit `Block` can be replaced by a `Block` object without changing the correctness of the program.
  - Interface Segregation Principle (ISP): In the code, `IGameObserver` is an interface that contains two methods: `RoundStarted()` and `GameOver()`. This is an example of the ISP principle, since any class
    that implements this interface will only have the methods it really needs to observe the game. This provides flexibility and reduces dependency.
#### IGameObserver:
```
 public interface IGameObserver
 {
     void RoundStarted();
     void GameOver();
 }
```
- Dependency Inversion Principle (DIP): The code uses this principle because the `BlockQueue` class depends on the abstraction `Block` and not on concrete classes.
#### Class BlockQueue:
```
 public class BlockQueue
 {
     private readonly Block[] blocks = new Block[]
     {
         new IBlock(), new JBlock(), new LBlock(), new OBlock(), new SBlock(), new TBlock(), new ZBlock()
     };
     private readonly Random random = new Random();
     public Block NextBlock { get; private set; }
     public BlockQueue()
     {
         NextBlock = RandomBlock();
     }
     private Block RandomBlock()
     {
         return blocks[random.Next(blocks.Length)];
     }
     public Block GetAndUpdate()
     {
         Block block = NextBlock;
         do
         {
             NextBlock = RandomBlock();
         }
         while (block.Id == NextBlock.Id);

         return block;
     }
 } 
```
4. YAGNI (You Aren’t Gonna Need It): The code does not contain anything extra. All classes, methods and properties have a clear purpose.
5. Fail Fast: The code uses this principle because it throws an ArgumentException in the `CreateBlock` method if an invalid block type is passed.
#### Method CreateBlock:
```
public Block CreateBlock(BlockType type)
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
```
### Refactoring Techniques
1. Extract Method: Highlighting the `InitializeGame` method
#### InitializeGame method:
```
   private void InitializeGame()
   {
       InitializeComponent();
       WindowState = WindowState.Maximized;
   }
```
2. Replace Magic Number with Symbolic Constant: Added more сonstant for sentences and numbers
#### For Example:
```
 private readonly int maxDelay = 500;
 private readonly int minDelay = 100;
 private readonly int delayDecrease = 2;
 private readonly string backgroundImagePath = ".\\Assets\\tetris_background.png";
 private readonly string backgroundImage2Path = ".\\Assets\\tetris_background2.png";
 private readonly string scoreLabelTextEn = "Score:";
 private readonly string scoreLabelTextUa = "Рахунок:";
 private readonly string nextButtonTextEn = "Next";
 private readonly string nextButtonTextUa = "Наступна";
 private readonly string holdButtonTextEn = "Hold";
 private readonly string holdButtonTextUa = "Утримувати";
 private readonly string finalScoreLabelTextEn = "Score:";
 private readonly string finalScoreLabelTextUa = "Рахунок:";
 private readonly string playAgainButtonTextEn = "Play Again";
 private readonly string playAgainButtonTextUa = "Спробувати ще раз";
 private readonly string highScoreTableButtonTextEn = "High Score Table";
 private readonly string highScoreTableButtonTextUa = "Рекордна Таблиця";
 private readonly string returnToMenuButtonTextEn = "Return To Menu";
 private readonly string returnToMenuButtonTextUa = "Повернення до меню";
```
3. Singleton to Monostate: This technique turns into replacing the “Singleton” pattern with “Monostet”. `gameState` is a static field that is initialized once.
The `GameState` property returns this static `gameState`. This way, all `GameManager` instances will have access to the same `gameState`, which is equivalent to the “Singleton” pattern,
but with more flexibility for testing and fewer dependencies.
#### Class GameManager:
```
public class GameManager
    {
       private static GameState gameState = new GameState();

       public GameState GameState => gameState;
    }
```
