using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
    /// <summary>
    /// This class contains all the logic for the tic tac toe game. All visuals would have to be
    /// handled by an external program.
    /// </summary>
    public class TicTacToe
    {
        /// <summary>
        /// A list of 9 unsigned ints that track what move is in each cell.
        /// </summary>
        private uint[] gameBoard = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// 3 variables that track how many wins the two players have, and the ties.
        /// </summary>
        private uint p1wins = 0;
        private uint p2wins = 0;
        private uint ties = 0;

        /// <summary>
        /// playing tracks if a game is going on or not
        /// </summary>
        private bool playing = false;

        /// <summary>
        /// who the current player is
        /// </summary>
        private uint currentPlayer = 0;

        /// <summary>
        /// how many human players are playing. This class doesn't use the variable directly.
        /// It is used as part of the rendering checks on the main window.
        /// </summary>
        private uint playerCount = 0;
        
        /// <summary>
        /// Empty constructor function because nothing special is needed for the class
        /// </summary>
        public TicTacToe()
        {

        }

        /// <summary>
        /// a function that updates the given board slot and changes the current player
        /// returns the player number it changed to, or 0 if the move was invalid
        /// </summary>
        public uint updateBoard(uint index)
        {
            if (playing && index < gameBoard.Length && gameBoard[index] == 0)
            {
                gameBoard[index] = currentPlayer;
                currentPlayer++;
                if (currentPlayer > 2)
                {
                    currentPlayer = 1;
                }
                return currentPlayer;
            }
            return 0;
        }

        /// <summary>
        /// A function created to check the board for a winning move separately from the win check function
        /// returns a list of 4 unsigned ints: index 0 contains the winning player, and indexes 1-3
        /// contain the 3 squares involved in the winning move
        /// </summary>
        private uint[] winCheckLoop()
        {
            uint[] conditions = { 3, 0, 0, 0 };
            //you could argue that the checks could all be in one if statement, but separating them makes it
            //easier for me to verify each check is correct

            //CHECKS FOR IF A PLAYER WON
            //first, check the 3 rows
            for (uint i = 0; i < 9; i += 3)
            {
                if (gameBoard[0+i] > 0 && gameBoard[0 + i] == gameBoard[1 + i] && gameBoard[0 + i] == gameBoard[2 + i]
                    && gameBoard[1 + i] == gameBoard[2 + i])
                {
                    conditions = new uint[] { gameBoard[0 + i], 0 + i, 1 + i, 2 + i};
                    return conditions;
                }
            }
            //second, check the 3 columns
            for (uint i = 0; i < 3; i++)
            {
                if (gameBoard[0 + i] > 0 && gameBoard[0 + i] == gameBoard[3 + i] && gameBoard[0 + i] == gameBoard[6 + i]
                    && gameBoard[3 + i] == gameBoard[6 + i])
                {
                    conditions = new uint[] { gameBoard[0 + i], 0 + i, 3 + i, 6 + i };
                    return conditions;
                }
            }
            //check for the diagonals
            if (gameBoard[0] > 0 && gameBoard[0] == gameBoard[4] && gameBoard[0] == gameBoard[8] && gameBoard[4] == gameBoard[8])
            {
                conditions = new uint[] { gameBoard[0], 0, 4, 8 };
                return conditions;
            }
            if (gameBoard[2] > 0 && gameBoard[2] == gameBoard[4] && gameBoard[2] == gameBoard[6] && gameBoard[4] == gameBoard[6])
            {
                conditions = new uint[] { gameBoard[2], 2, 4, 6 };
                return conditions;
            }

            //CHECKS IF A TIE OCCURED
            //this check is done after the win check to verify any positives there are covered before
            //a tie is declared. This loop specifically checks if moves can still be made
            for (int i = 0; i < gameBoard.Length; i++)
            {
                //0 means unfilled space, so moves can still be made
                if (gameBoard[i] == 0)
                {
                    conditions[0] = 0;
                    return conditions;
                }
            }

            //default return is that a tie occured
            return conditions;
        }

        /// <summary>
        /// This function checks if a winning move was made, and what that winning move was.
        /// If a game end exists, it also increments either the win count, or the ties count.
        /// returns a list of 4 unsigned ints: index 0 contains the winning player, and indexes 1-3
        /// contain the 3 squares involved in the winning move
        /// </summary>
        public uint[] checkForWin()
        {
            if (playing == true)
            {
                uint[] data = winCheckLoop();
                switch (data[0])
                {
                    //1=player 1, 2=player 2, 3=tie
                    case 1:
                        p1wins++;
                        playing = false;
                        break;
                    case 2:
                        p2wins++;
                        playing = false;
                        break;
                    case 3:
                        ties++;
                        playing = false;
                        break;
                    default:
                        break;
                }
                return data;
            }
            return new uint[] {0,0,0,0 };
        }

        /// <summary>
        /// A function to start a game up. It verifies if a game already is running before it runs
        /// returns whether or not it was successful in starting a new game
        /// </summary>
        public bool startBoard()
        {
            if (playing == false)
            {
                for (int i = 0; i < gameBoard.Length; i++)
                {
                    gameBoard[i] = 0;
                }
                playing = true;
                currentPlayer = 1;
                return true;
            }
            return false;
        }

        /// <summary>
        /// A function to display how many times someone won or how many ties.
        /// returns a list of 3 unsigned ints: index 0 contains player 1 wins, index 1 contains player
        /// 2 wins, and index 2 contains the amount of ties.
        /// </summary>
        public uint[] getWinData()
        {
            return new uint[] {p1wins, p2wins, ties };
        }

        /// <summary>
        /// A function to get the size of the board. 
        /// returns the board size as an int.
        /// </summary>
        public int getBoardSize()
        {
            return gameBoard.Length;
        }

        /// <summary>
        /// A function to get the board. Returns the board itself.
        /// </summary>
        public uint[] getBoard()
        {
            return gameBoard;
        }

        /// <summary>
        /// A function to change the player count. Used to validate input
        /// </summary>
        public void changePlayerCount(uint c)
        {
            if (c == 1 || c == 2)
            {
                playerCount = c;
            }
        }

        /// <summary>
        /// This lets a user check how many human players are present
        /// returns the human player count.
        /// </summary>
        public uint getPlayerCount()
        {
            return playerCount;
        }

        /// <summary>
        /// A function to check how many squares are filled. This function is mostly to make sure the
        /// players have non-matching turn counts.
        /// return how many moves were made.
        /// </summary>
        public uint getMovesMade()
        {
            uint m = 0;
            for (int i = 0; i < gameBoard.Length; i++)
            {
                if (gameBoard[i] != 0)
                {
                    m++;
                }
            }
            return m;
        }
    }
}
