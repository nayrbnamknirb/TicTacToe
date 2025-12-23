using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;

namespace Assignment4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    //personal note: 0=unmarked 1=X 2=O
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The tic tac toe object so that the ui can call the game functions
        /// </summary>
        TicTacToe game = new TicTacToe();
        /// <summary>
        /// An AI object for if the player wants to play an AI
        /// </summary>
        AI other = new AI();

        /// <summary>
        /// two lists that store information returned from the tic tac toe object in bulk
        /// game data contains a winning move, and win data contains how many wins/ties exist
        /// </summary>
        uint[] gameData;
        uint[] winData;

        public uint UpdateBoard(uint i)
        {
            uint p = game.updateBoard(i);
            if (p == 0)
            {
                return 0;
            }
            dynamic but = (this.FindName("Cell" + (i + 1)) as Button);
            switch (p)
            {
                case 2:
                    but.Content = "X";
                    but.Foreground = new SolidColorBrush(Colors.Red);
                    ListHeader.Content = "Player 2's turn";
                    break;
                case 1:
                    but.Content = "O";
                    but.Foreground = new SolidColorBrush(Colors.Blue);
                    ListHeader.Content = "Player 1's turn";
                    break;
                default:
                    break;
            }
            gameData = game.checkForWin();
            if (gameData[0] > 0)
            {
                switch (gameData[0])
                {
                    case 1:
                        ListHeader.Content = "Player 1 Wins";
                        break;
                    case 2:
                        ListHeader.Content = "Player 2 Wins";
                        break;
                    case 3:
                        ListHeader.Content = "A tie has occured";
                        break;
                    default:
                        break;
                }
                if (gameData[0] != 3)
                {
                    for (int j = 1; j <= game.getBoardSize(); j++)
                    {
                        for (int k = 1; k < gameData.Length; k++)
                        {
                            if (gameData[k] == j - 1)
                            {
                                dynamic butt = (this.FindName("Cell" + j) as Button);
                                butt.Background = new SolidColorBrush(Colors.Yellow);
                            }
                        }
                    }
                }
                winData = game.getWinData();
                P1Wins.Content = "" + winData[0];
                P2Wins.Content = "" + winData[1];
                Ties.Content = "" + winData[2];
                StartGame.IsEnabled = true;
            }
            return 1;
        }

        /// <summary>
        /// Universal button click function, per the assignment requirements
        /// Allows clicking on any of the cells to insert a symbol, and on the
        /// start game button to start a game.
        /// </summary>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(StartGame))
            {
                MessageBoxResult Res = MessageBox.Show("Would you like to play with two players?", "Information", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                if (Res == MessageBoxResult.Cancel)
                {
                    return;
                }
                else if (Res == MessageBoxResult.Yes)
                {
                    game.changePlayerCount(2);
                }
                else
                {
                    game.changePlayerCount(1);
                }
                if (game.startBoard())
                {
                    for (int i = 1; i <= game.getBoardSize(); i++)
                    {
                        dynamic but = (this.FindName("Cell" + i) as Button);
                        but.Content = "";
                        but.Foreground = new SolidColorBrush(Colors.Black);
                        but.Background = new SolidColorBrush(Colors.Silver);
                        ListHeader.Content = "";
                    }
                    ListHeader.Content = "Player 1's turn";
                    StartGame.IsEnabled = false;
                }
            }
            else
            {
                for (uint i = 0; i < game.getBoardSize(); i++)
                {
                    if (sender.Equals(this.FindName("Cell"+(i+1)) as Button))
                    {
                        if (UpdateBoard(i) == 0)
                        {
                            return;
                        }
                        if (game.getPlayerCount() == 1 && game.getMovesMade() < 9)
                        {
                            UpdateBoard(other.makeMove(game.getBoard()));
                        }
                    }
                }
            }
        }
    }
}