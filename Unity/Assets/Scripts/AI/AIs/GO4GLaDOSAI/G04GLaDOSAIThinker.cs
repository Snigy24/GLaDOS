using System;
using System.Threading;

/// <summary>
///  GlaDOS AI Thinker class. Uses the minimax pruning to figure out the next moves.
/// </summary>
public class G04GLaDOSAIThinker : IThinker
{
    /// <summary>
    /// Instance of a class to get a score for the board state.
    /// </summary>
    private G04GLaDOSStaticEvaluation staticEvaluation;

    /// <summary>
    /// How deep the AI will go to foresee board moves.
    /// </summary>
    private int depth;

    /// <summary>
    /// On the AI's turn saves its color.
    /// </summary>
    private PColor playerColor;

    /// <summary>
    /// On the AI's turn saves its main shape.
    /// </summary>
    private PShape playerShape;

    /// <summary>
    /// On the Enemy's turn saves its color.
    /// </summary>
	private PColor enemyColor;

    /// <summary>
    /// On the Enemy's turn saves its main shape.
    /// </summary>
	private PShape enemyShape;

    /// <summary>
    /// Create a new instance of G04GLaDOSAIThinker.
    /// </summary>
    public G04GLaDOSAIThinker(int depth)
    {
        // Instantiates the staticEvaluation and sets the depth for how many moves the AI will foresee during the match
        staticEvaluation = new G04GLaDOSStaticEvaluation();
        this.depth = depth;

    }

    /// <summary>
    /// Picks the best move for the AI.
    /// </summary>
    /// <param name="board">Copy of the board state.</param>
    /// <param name="ct"></param>
    /// <returns>Returns the best move thought out by the AI.</returns>
    public FutureMove Think(Board board, CancellationToken ct)
    {
        // Sets the AI's and Enemy's color and shape.
		playerColor = board.Turn;
		SetPlayerColorShape();

        // Sets the best score as the lowest possible.
		int bestScore = int.MinValue + 1;
		FutureMove bestMove = new FutureMove();

        // Calls minimax on all possible plays and saves the one with the best score.
		for (int i = 0; i < board.cols; i++)
		{
            // If the AI exceeds the time limit and a cancellation is requested, we return no move.
			if (ct.IsCancellationRequested) return FutureMove.NoMove;

            // Swaps the AI's pieces shape if he runs out of them.
			if (board.PieceCount(playerColor, playerShape) < 1)
			{
				playerShape = playerShape == PShape.Round ? PShape.Square : PShape.Round;
			}

            // Ignores the column and goes to the next one if it is full.
			if (board.IsColumnFull(i)) continue;

            // Tests a move on a copy of the board.
			board.DoMove(playerShape, i);

            // Calls minimax on the board copy state
			int score = Minimax(board.Copy(), depth - 1, int.MinValue + 1, int.MaxValue - 1, false, ct);

            // Undoes the move.
			board.UndoMove();

            // Replaces the best score and the move with the previous one if it's higher.
			if (score > bestScore)
			{
				bestScore = score;
				bestMove = new FutureMove(i, playerShape);
			}
		}
        // Returns the best move calculated.
		return bestMove;
	}

    /// <summary>
    /// Checks all the possible plays by the AI and the Enemy as deep as the user wants.
    /// </summary>
    /// <param name="board">Copy of the board state.</param>
    /// <param name="depth">How many moves will the AI search.</param>
    /// <param name="alpha"></param>
    /// <param name="beta"></param>
    /// <param name="maximizingPlayer"></param>
    /// <param name="ct"></param>
    /// <returns>Returns an static evaluation score of the board state initually called on.</returns>
    private int Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer, CancellationToken ct)
    {
        // Returns 0 if a cancelation is requested.
		if (ct.IsCancellationRequested) return 0;

        // If the AI reaches the depth or if there is a winner, returns a score 
		if (depth <= 0 || board.CheckWinner() != Winner.None)
		{
			if ((board.CheckWinner() == Winner.White && playerColor == PColor.White) || (board.CheckWinner() == Winner.Red && playerColor == PColor.Red))
			{
                // If the AI is the winner returns the maximum score.
				return 1000;
			}
			else if ((board.CheckWinner() == Winner.White && enemyColor == PColor.White) || (board.CheckWinner() == Winner.Red && enemyColor == PColor.Red))
			{
                // If the enemy is the winner returns the minimum score.
				return -1000;
			}
			else if (board.CheckWinner() == Winner.Draw)
			{
                // If it's a draw returns a high negative value.
				return -500;
			}

			return staticEvaluation.GetHeuristicValue(board.winCorridors, board, playerColor, playerShape);
		}

        // Checks if it's the AI's turn and then makes sure it can do a move on the board copy and call minimax on it.
		if (maximizingPlayer)
		{
			int maxEval = int.MinValue + 1;
			for (int i = 0; i < board.cols; i++)
			{
				if (board.PieceCount(playerColor, playerShape) < 1)
				{
					break;
				}

				if (board.IsColumnFull(i)) continue;

				board.DoMove(playerShape, i);
				int score = Minimax(board.Copy(), depth - 1, alpha, beta, false, ct);
				Move lastMove = board.UndoMove();
				maxEval = Math.Max(maxEval, score);
				alpha = Math.Max(alpha, score);
                // Prunes the node.
				if (beta <= alpha) break;
			}
            // Returns the highest score.
			return maxEval;
		}
        // Plays on the Enemy's turn to foresee his move, and sets the lowest value.
		else
		{
			int minEval = int.MaxValue - 1;
			for (int i = 0; i < board.cols; i++)
			{
				if (board.PieceCount(enemyColor, enemyShape) < 1)
				{
					break;
				}

				if (board.IsColumnFull(i)) continue;

				board.DoMove(enemyShape, i);
				int score = Minimax(board.Copy(), depth - 1, alpha, beta, false, ct);
				Move lastMove = board.UndoMove();
				minEval = Math.Min(minEval, score);
				beta = Math.Min(beta, score);
                // Prunes the node.
				if (beta <= alpha) break;
			}
            // Returns the lowest score.
			return minEval;
		}
    }

    /// <summary>
    /// Sets the AI's and Enemy's Colors and Shapes.
    /// </summary>
    private void SetPlayerColorShape()
    {

		if (playerColor == PColor.White)
		{
			playerShape = PShape.Round;
			enemyColor = PColor.Red;
			enemyShape = PShape.Square;
		}
		else
		{
			playerShape = PShape.Square;
			enemyColor = PColor.White;
			enemyShape = PShape.Round;
		}
    }
}
