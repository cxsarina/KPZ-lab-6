using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TetrisLibrary;
using TetrisLibrary.Constants;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        private static MainWindow instance;
        public static MainWindow Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainWindow();
                }
                return instance;
            }
        }

        public List<User> userList = new List<User>();
        public string filePath = "..\\..\\..\\..\\ScoreTable.txt";
        public List<User> updatedUserList = new List<User>();
        public Options option;
        public User users;
        public int delay;
        Menu menu = new Menu();

        private static ImageSource[] LoadImageSources(string basePath, string[] fileNames)
        {
            return fileNames.Select(fileName => new BitmapImage(new Uri(System.IO.Path.Combine(basePath, fileName), UriKind.Relative))).ToArray();
        }

        private readonly ImageSource[] tileImages1 = LoadImageSources("Assets", new string[] {
        "TileEmpty.png",
        "TileCyan.png",
        "TileBlue.png",
        "TileOrange.png",
        "TileYellow.png",
        "TileGreen.png",
        "TilePurple.png",
        "TileRed.png"
    });

        private readonly ImageSource[] tileImages2 = LoadImageSources("Assets", new string[] {
        "TileEmpty.png",
        "TileCyan1.png",
        "TileBlue1.png",
        "TileOrange1.png",
        "TileYellow1.png",
        "TileGreen1.png",
        "TilePurple1.png",
        "TileRed1.png"
    });

        private readonly ImageSource[] blockImages1 = LoadImageSources("Assets", new string[] {
        "Block-Empty.png",
        "Block-I.png",
        "Block-J.png",
        "Block-L.png",
        "Block-O.png",
        "Block-S.png",
        "Block-T.png",
        "Block-Z.png"
    });

        private readonly ImageSource[] blockImages2 = LoadImageSources("Assets", new string[] {
        "Block-Empty.png",
        "Block-I(1).png",
        "Block-J(1).png",
        "Block-L(1).png",
        "Block-O(1).png",
        "Block-S(1).png",
        "Block-T(1).png",
        "Block-Z(1).png"
    });

        private readonly Image[,] imageControls;
        private GameState gameState;

        private void InitializeGame()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private MainWindow()
        {
            instance = this;
            InitializeGame();
            gameState = new GameState();
            gameState.RoundStartedEvent += OnRoundStarted;
            gameState.GameOverEvent += OnGameOver;

            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        public static MainWindow GetInstance()
        {
            if (instance == null)
            {
                instance = new MainWindow();
            }
            return instance;
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = option.Theme == 0 ? tileImages1[id] : tileImages2[id];
                }
            }
        }

        private void DrawBlock(IFigure block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = option.Theme == 0 ? tileImages1[block.Id] : tileImages2[block.Id];
            }
        }

        private IFigure CreateNewBlock()
        {
            BlockType type = (BlockType)new Random().Next(0, 7);
            return BlockFactory.CreateBlock(type);
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            IFigure next = blockQueue.NextBlock;
            NextImage.Source = option.Theme == 0 ? blockImages1[next.Id] : blockImages2[next.Id];
        }

        private void DrawHeldBlock(IFigure heldBlock)
        {
            ImageSource[] selectedBlockImages = option.Theme == 0 ? blockImages1 : blockImages2;
            HoldImage.Source = heldBlock == null ? selectedBlockImages[0] : selectedBlockImages[heldBlock.Id];
        }

        private void DrawGhostBlock(IFigure block)
        {
            int dropDistance = gameState.BlockDropDistance();
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = option.Theme == 0 ? tileImages1[block.Id] : tileImages2[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            if (option.Ghost_Block == 0)
            {
                DrawGhostBlock(gameState.CurrentBlock);
            }
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            SetScoreLabelText(gameState.Score);
        }

        private void SetScoreLabelText(int score)
        {
            string labelText = option.Language == 0 ? Constants.ScoreLabelTextEn : Constants.ScoreLabelTextUa;
            ScoreText.Text = $"{labelText} {score}";
        }

        private async Task GameLoop()
        {
            gameState.CurrentBlock = CreateNewBlock();
            Draw(gameState);

            while (!gameState.GameOver)
            {
                delay = CalculateDelay(option.Difficult, gameState.Score);
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            UpdateFinalScoreText(option.Language, gameState.Score);

            var difficulty = (Difficulty)option.Difficult;
            users = UpdateUserScores(users, difficulty, gameState.Score);

            SaveUserScores(filePath, userList, users);
            GameOver.Text = GetGameOverText(option.Language);
        }

        private int CalculateDelay(int difficulty, int score)
        {
            int baseDelay = 1000;
            double delayDecrease = 0.1;

            return difficulty switch
            {
                0 => Math.Max(Constants.MinDelay, baseDelay - (score / 3)),
                1 => Math.Max(Constants.MinDelay, baseDelay - score),
                2 => Math.Max(Constants.MinDelay, baseDelay - (int)(score * delayDecrease)),
                _ => baseDelay,
            };
        }

        private void UpdateFinalScoreText(int language, int score)
        {
            FinalScoreText.Text = language == 0 ? $"{Constants.FinalScoreLabelTextEn} {score}" : $"{Constants.FinalScoreLabelTextUa} {score}";
        }

        private User UpdateUserScores(User user, Difficulty difficulty, int score)
        {
            bool userFound = false;

            foreach (var existingUser in userList)
            {
                if (existingUser.Name == user.Name)
                {
                    switch (difficulty)
                    {
                        case Difficulty.Easy:
                            existingUser.Score_Easy = Math.Max(existingUser.Score_Easy, score);
                            break;
                        case Difficulty.Normal:
                            existingUser.Score_Normal = Math.Max(existingUser.Score_Normal, score);
                            break;
                        case Difficulty.Hard:
                            existingUser.Score_Hard = Math.Max(existingUser.Score_Hard, score);
                            break;
                    }

                    userFound = true;
                }

                updatedUserList.Add(existingUser);
            }

            if (!userFound)
            {
                switch (difficulty)
                {
                    case Difficulty.Easy:
                        user.Score_Easy = score;
                        break;
                    case Difficulty.Normal:
                        user.Score_Normal = score;
                        break;
                    case Difficulty.Hard:
                        user.Score_Hard = score;
                        break;
                }

                updatedUserList.Add(user);
            }

            return user;
        }

        private void SaveUserScores(string filePath, List<User> userList, User updatedUser)
        {
            using (var sw = new StreamWriter(filePath, false))
            {
                foreach (var user in updatedUserList)
                {
                    string line = $"{user.Name},{user.Score_Easy},{user.Score_Normal},{user.Score_Hard}";
                    sw.WriteLine(line);
                }
            }
        }

        private string GetGameOverText(int language)
        {
            return language == 0 ? "Game Over" : "Ви програли!";
        }

        private enum Difficulty
        {
            Easy,
            Normal,
            Hard
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                default:
                    return;
            }
            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            gameState.RoundStartedEvent += OnRoundStarted;
            gameState.GameOverEvent += OnGameOver;
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        private void ReturnToMenu_Click(object sender, RoutedEventArgs e)
        {
            menu.settings = option;
            menu.Show();
            this.Close();
        }

        private void HighScoreTable_Click(object sender, RoutedEventArgs e)
        {
            HighScoreTablee highScoreTablee = new HighScoreTablee();
            highScoreTablee.userlist = updatedUserList;
            highScoreTablee.userss = users;
            highScoreTablee.optionss = option;
            highScoreTablee.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        string name = parts[0];
                        int score_easy = Convert.ToInt32(parts[1]);
                        int score_normal = Convert.ToInt32(parts[2]);
                        int score_hard = Convert.ToInt32(parts[3]);
                        User user = new User { Name = name, Score_Easy = score_easy, Score_Normal = score_normal, Score_Hard = score_hard };
                        userList.Add(user);
                    }
                }
            }

            string imagePath = option.Theme == 0 ? Constants.BackgroundImagePath : Constants.BackgroundImage2Path;
            string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
            Uri imageUri = new Uri(fullPath, UriKind.Absolute);
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(imageUri));
            this.Background = imageBrush;

            ScoreText.Text = option.Language == 0 ? Constants.ScoreLabelTextEn : Constants.ScoreLabelTextUa;
            Next.Text = option.Language == 0 ? Constants.NextButtonTextEn : Constants.NextButtonTextUa;
            Hold.Text = option.Language == 0 ? Constants.HoldButtonTextEn : Constants.HoldButtonTextUa;
            FinalScoreText.Text = option.Language == 0 ? Constants.FinalScoreLabelTextEn : Constants.FinalScoreLabelTextUa;
            PlayAgain.Content = option.Language == 0 ? Constants.PlayAgainButtonTextEn : Constants.PlayAgainButtonTextUa;
            HighScoreTable.Content = option.Language == 0 ? Constants.HighScoreTableButtonTextEn : Constants.HighScoreTableButtonTextUa;
            ReturnToMenu.Content = option.Language == 0 ? Constants.ReturnToMenuButtonTextEn : Constants.ReturnToMenuButtonTextUa;
        }

        private void OnRoundStarted()
        {
            Draw(gameState);
        }

        private void OnGameOver()
        {
            GameOverMenu.Visibility = Visibility.Visible;
            UpdateFinalScoreText(option.Language, gameState.Score);

            Difficulty difficulty = (Difficulty)option.Difficult;
            users = UpdateUserScores(users, difficulty, gameState.Score);

            SaveUserScores(filePath, userList, users);
            GameOver.Text = GetGameOverText(option.Language);
        }
    }
}
