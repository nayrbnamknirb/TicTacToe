using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
    /// <summary>
    /// A class that contains a computer player AI separately, to allow easier editing of AI programing
    /// </summary>
    internal class AI
    {
        /// <summary>
        /// Empty constructor function because nothing special is needed for the class
        /// </summary>
        public AI() { }

        /// <summary>
        /// Separate logic to decide how important a filled row is
        /// returns 2 for the AI matching, 1 for the human matching, and 0 if false positive
        /// </summary>
        private uint pickPriority(uint[] move)
        {
            if (move[0] == 2 && move[1] == 2)
            {
                return 2;
            }
            else if (move[0] == 1 && move[1] == 1)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// The main logic to decide what move the AI should make.
        /// Returns the requested move to make, which the main window uses to render on the window.
        /// </summary>
        public uint makeMove(uint[] board)
        {
            //create a priority list of cells that could be matched
            //obviously giving more priority to wins over blocks
            uint[] priorityList = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            //check the rows
            for (uint i = 0; i < 9; i += 3)
            {
                if (board[0+i] > 0 && board[0+i] == board[1+i] && board[2+i] == 0)
                {
                    priorityList[2 + i] += pickPriority(new uint[] { board[0+i], board[1+i] });
                }
                else if (board[0+i] > 0 && board[0+i] == board[2+i] && board[1+i] == 0)
                {
                    priorityList[1 + i] += pickPriority(new uint[] { board[0 + i], board[2 + i] });
                }
                else if (board[1+i] > 0 && board[1+i] == board[2+i] && board[0+i] == 0)
                {
                    priorityList[0 + i] += pickPriority(new uint[] { board[2 + i], board[1 + i] });
                }
            }

            //check the columns
            for (uint i = 0; i < 3; i++)
            {
                if (board[0 + i] > 0 && board[0 + i] == board[3 + i] && board[6 + i] == 0)
                {
                    priorityList[6 + i] += pickPriority(new uint[] { board[0 + i], board[3 + i] });
                }
                else if (board[0 + i] > 0 && board[0 + i] == board[6 + i] && board[3 + i] == 0)
                {
                    priorityList[3 + i] += pickPriority(new uint[] { board[0 + i], board[6 + i] });
                }
                else if (board[3 + i] > 0 && board[3 + i] == board[6 + i] && board[0 + i] == 0)
                {
                    priorityList[0 + i] += pickPriority(new uint[] { board[6 + i], board[3 + i] });
                }
            }

            //check the diagonals
            if (board[0] > 0 && board[0] == board[4] && board[8] == 0)
            {
                priorityList[8] += pickPriority(new uint[] { board[0], board[4] });
            }
            else if (board[0] > 0 && board[0] == board[8] && board[4] == 0)
            {
                priorityList[4] += pickPriority(new uint[] { board[0], board[8] });
            }
            else if (board[4] > 0 && board[4] == board[8] && board[0] == 0)
            {
                priorityList[0] += pickPriority(new uint[] { board[8], board[4] });
            }

            if (board[2] > 0 && board[2] == board[4] && board[6] == 0)
            {
                priorityList[6] += pickPriority(new uint[] { board[2], board[4] });
            }
            else if (board[2] > 0 && board[2] == board[6] && board[4] == 0)
            {
                priorityList[4] += pickPriority(new uint[] { board[2], board[6] });
            }
            else if (board[4] > 0 && board[4] == board[6] && board[2] == 0)
            {
                priorityList[2] += pickPriority(new uint[] { board[4], board[6] });
            }

            //if center is not taken, always take center
            //(note that if player 1 takes a corner and the ai does not pick center, it can always loose)
            if (board[4] == 0)
            {
                priorityList[4] = 2;
            }

            //after, compare move priority of all actions, and do the action with the highest priority
            List<uint> possibleMoves = new List<uint>();
            Random rnd = new Random();
            uint currentPick = 9;
            uint highestPriority = 0;
            for (uint i = 0; i < 9; i++)
            {
                if (priorityList[i] > highestPriority)
                {
                    highestPriority = priorityList[i];
                    currentPick = i;
                }
            }
            if (currentPick < 9)
            {
                return currentPick;
            }
            
            //failsafe: in-case the above checks don't return a particular move, pick a move at random
            for (uint i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                {
                    possibleMoves.Add(i);
                }
            }
            return possibleMoves[rnd.Next(0, possibleMoves.Count)];
        }
    }
}
