using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class G04GLaDOSAIThinker : IThinker
{
	// A random number generator instance
	private System.Random random;

    PColor playerColor;
    PShape playerShape;

    /// <summary>
    /// Create a new instance of G04GLaDOSAIThinker.
    /// </summary>
    public G04GLaDOSAIThinker()
	{
		random = new System.Random();
	}

	public FutureMove Think(Board board, CancellationToken ct)
	{
		Move bestMove = new Move();
        playerColor = board.Turn;
        int bestScore = -1;
        SetPlayerShape();
        Move move;
        for (int i = 0; i < board.cols; i++)
        {
            if (ct.IsCancellationRequested) return FutureMove.NoMove;
            if (board.IsColumnFull(i)) continue;
            if (board.PieceCount(playerColor, playerShape) <= 0) playerShape = PShape.Square;
			board.DoMove(playerShape, i);
            int score = Minimax(board, 2, int.MinValue + 1, int.MaxValue - 1, false, playerColor, ct);
            move = board.UndoMove();
			if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
		}
		return new FutureMove(bestMove.col, bestMove.piece.shape);
    }

	private int Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer, PColor playerColor, CancellationToken ct)
	{
        if (ct.IsCancellationRequested) return 0;
        if (depth <= 0 || board.CheckWinner() != Winner.None)
		{
			int staticEvaluation = GetHeuristicValue(board.winCorridors, board, playerColor);
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
				int eval = Minimax(board, depth - 1, alpha, beta, false, playerColor, ct);
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
				int eval = Minimax(board, depth - 1, alpha, beta, false, playerColor, ct);
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
				int eval = Minimax(board, depth - 1, alpha, beta, true, playerColor, ct);
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
				int eval = Minimax(board, depth - 1, alpha, beta, true, playerColor, ct);
				board.UndoMove();
				minEval = Math.Min(minEval, eval);
				beta = Math.Min(beta, eval);
				if (beta <= alpha) break;
			}
			return minEval;
		}
	}

	private int GetHeuristicValue(IEnumerable<IEnumerable<Pos>> corridors, Board board, PColor _playerColor)
	{
		PShape _playerShape;
		if (_playerColor == PColor.White) _playerShape = PShape.Round;
		else _playerShape = PShape.Square;
        int total = 0;
		int count = 0;
		foreach (IEnumerable<Pos> corridor in corridors)
		{
            count = 0;
			foreach (Pos p in corridor)
			{

				if (!board[p.row, p.col].HasValue) continue;
				Piece piece = board[p.row, p.col].Value;
				if (board[p.row, p.col].HasValue && piece.Is(_playerColor, _playerShape)) count *= 2;
				else if (board[p.row, p.col].HasValue && !piece.Is(_playerColor, _playerShape)) count /= 2;
			}
            total += count;
		}
		return total;
	}

    private void SetPlayerShape()
    {
		if (playerColor == PColor.White) playerShape = PShape.Round;
		else
		{
			playerShape = PShape.Square;
		}
    }
}
