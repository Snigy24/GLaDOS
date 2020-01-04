using System;
using System.Threading;

public class G04GLaDOSAIThinker : IThinker
{
	// A random number generator instance
	private Random random;


	/// <summary>
	/// Create a new instance of G04GLaDOSAIThinker.
	/// </summary>
	public G04GLaDOSAIThinker()
	{
		random = new Random();
	}

	public FutureMove Think(Board board, CancellationToken ct)
	{
		int col = 0;
		PShape shape = PShape.Round;
		return new FutureMove(col, shape);
	}

	private int Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer)
	{

		if (depth == 0 || board.CheckWinner() != Winner.None)
		{
			board.UndoMove();
			//return
		}
		if (maximizingPlayer)
		{
			PShape shape = PShape.Round;
			int maxEval = int.MinValue;
			for (int i = 0; i < board.cols; i++)
			{
				board.DoMove(shape, i);
				int eval = Minimax(board, depth - 1, alpha, beta, false);
				//board.UndoMove();
				maxEval = Math.Max(maxEval, eval);
				alpha = Math.Max(alpha, eval);
				if (beta <= alpha) break;
			}
			shape = PShape.Square;
			for (int i = 0; i < board.cols; i++)
			{
				board.DoMove(shape, i);
				int eval = Minimax(board, depth - 1, alpha, beta, false);
				//board.UndoMove();
				maxEval = Math.Max(maxEval, eval);
				alpha = Math.Max(alpha, eval);
				if (beta <= alpha) break;
			}
			return maxEval;
		}
		else
		{
			PShape shape = PShape.Round;
			int minEval = int.MinValue;
			for (int i = 0; i < board.cols; i++)
			{
				board.DoMove(shape, i);
				int eval = Minimax(board, depth - 1, alpha, beta, true);
				//board.UndoMove();
				minEval = Math.Min(minEval, eval);
				beta = Math.Min(beta, eval);
				if (beta <= alpha) break;

			}
			shape = PShape.Square;
			for (int i = 0; i < board.cols; i++)
			{
				board.DoMove(shape, i);
				int eval = Minimax(board, depth - 1, alpha, beta, true);
				//board.UndoMove();
				minEval = Math.Min(minEval, eval);
				beta = Math.Min(beta, eval);
				if (beta <= alpha) break;
			}
			return minEval;
		}
	}
}
