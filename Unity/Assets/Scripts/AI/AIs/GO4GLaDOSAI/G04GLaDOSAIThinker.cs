using System;
using System.Collections.Generic;
using System.Threading;

public class G04GLaDOSAIThinker : IThinker
{
	// A random number generator instance
	private Random random;

	private int lastCol = -1;

	/// <summary>
	/// Create a new instance of G04GLaDOSAIThinker.
	/// </summary>
	public G04GLaDOSAIThinker()
	{
		random = new Random();
	}

	public FutureMove Think(Board board, CancellationToken ct)
	{
		Move bestMove = new Move();
		PColor playerColor = board.Turn;
		PShape playerShape;
		int bestScore = 0;
		if (playerColor == PColor.White) playerShape = PShape.Round;
		else playerShape = PShape.Square;

		do
		{
			lastCol++;
			if (lastCol >= board.cols) lastCol = 0;
			
			// Is this task to be cancelled?
			if (ct.IsCancellationRequested) return FutureMove.NoMove;
		}
		while (board.IsColumnFull(lastCol));
		Move move;
		if (board.PieceCount(playerColor, playerShape) <= 0) playerShape = PShape.Square;
		int row = board.DoMove(playerShape, lastCol);
		int score = Minimax(board, 3, int.MinValue + 1, int.MaxValue - 1, true, playerColor);
		move = board.UndoMove();
		if (score > bestScore)
		{
			bestScore = score;
			bestMove = move;
		}

		FutureMove futureMove = new FutureMove(row, bestMove.piece.shape);
		//Minimax(board, 3, int.MinValue + 1, int.MaxValue - 1, true, playerColor, ref move);
		return futureMove;
	}

	private int Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer, PColor playerColor)
	{
		if (depth == 0 || board.CheckWinner() != Winner.None)
		{
			int staticEvaluation = GetHeuristicValue(board.winCorridors, board, playerColor);
			// if currentValue < value swap move
			//board.UndoMove();
			return staticEvaluation;
		}
		if (maximizingPlayer)
		{
			PShape shape = PShape.Round;
			int maxEval = int.MinValue + 1;
			for (int i = 0; i < board.cols; i++)
			{
				if (board.PieceCount(playerColor, shape) <= 0) break;
				if (board.IsColumnFull(i)) continue;
				board.DoMove(shape, i);
				int eval = Minimax(board, depth - 1, alpha, beta, false, playerColor);
				board.UndoMove();
				maxEval = Math.Max(maxEval, eval);
				alpha = Math.Max(alpha, eval);
				if (beta <= alpha) break;
			}
			shape = PShape.Square;
			for (int i = 0; i < board.cols; i++)
			{
				if (board.PieceCount(playerColor, shape) <= 0) break;
				if (board.IsColumnFull(i)) continue;
				board.DoMove(shape, i);
				int eval = Minimax(board, depth - 1, alpha, beta, false, playerColor);
				board.UndoMove();
				maxEval = Math.Max(maxEval, eval);
				alpha = Math.Max(alpha, eval);
				if (beta <= alpha) break;
			}
			return maxEval;
		}
		else
		{
			PShape shape = PShape.Round;
			int minEval = int.MaxValue - 1;
			for (int i = 0; i < board.cols; i++)
			{
				if (board.PieceCount(playerColor, shape) <= 0) break;
				if (board.IsColumnFull(i)) continue;
				board.DoMove(shape, i);
				int eval = Minimax(board, depth - 1, alpha, beta, true, playerColor);
				board.UndoMove();
				minEval = Math.Min(minEval, eval);
				beta = Math.Min(beta, eval);
				if (beta <= alpha) break;

			}
			shape = PShape.Square;
			for (int i = 0; i < board.cols; i++)
			{
				if (board.PieceCount(playerColor, shape) <= 0) break;
				if (board.IsColumnFull(i)) continue;
				board.DoMove(shape, i);
				int eval = Minimax(board, depth - 1, alpha, beta, true, playerColor);
				board.UndoMove();
				minEval = Math.Min(minEval, eval);
				beta = Math.Min(beta, eval);
				if (beta <= alpha) break;
			}
			return minEval;
		}
	}

	private int GetHeuristicValue(IEnumerable<IEnumerable<Pos>> corridors, Board board, PColor playerColor)
	{
		PShape playerShape;
		if (playerColor == PColor.White) playerShape = PShape.Round;
		else playerShape = PShape.Square;
        int total = 0;
		int count = 0;
		foreach (IEnumerable<Pos> corridor in corridors)
		{
            count = 0;
			foreach (Pos p in corridor)
			{

				if (!board[p.row, p.col].HasValue) continue;
				Piece piece = board[p.row, p.col].Value;
				if (board[p.row, p.col].HasValue && piece.Is(playerColor, playerShape)) count *= 2;
				else if (board[p.row, p.col].HasValue && !piece.Is(playerColor, playerShape)) count /= 2;
			}
            total += count;
		}
		return total;
	}
}
