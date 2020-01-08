using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class G04GLaDOSAIThinker : IThinker
{
	private struct NewMove
	{
		public int Score { get; }
		public FutureMove Move { get; }

		public NewMove(int score, FutureMove move)
		{
			Score = score;
			Move = move;
		}
	}


    // A random number generator instance
    private System.Random random;

    private G04GLaDOSStaticEvaluation staticEvaluation;

    private int depth;

    private PColor playerColor;
    private PShape playerShape;

	private PColor enemyColor;
	private PShape enemyShape;

    /// <summary>
    /// Create a new instance of G04GLaDOSAIThinker.
    /// </summary>
    public G04GLaDOSAIThinker(int depth)
    {
        random = new System.Random();
        staticEvaluation = new G04GLaDOSStaticEvaluation();
        this.depth = depth;

    }

    public FutureMove Think(Board board, CancellationToken ct)
    {
		playerColor = board.Turn;
		SetPlayerColorShape();
		NewMove move = Minimax(board, depth, int.MinValue + 1, int.MaxValue - 1, true, ct);
		FutureMove bestMove = move.Move;
		return bestMove;
	}

    private NewMove Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer, CancellationToken ct)
    {
		if (ct.IsCancellationRequested) return new NewMove(0, FutureMove.NoMove);

		if (depth <= 0 || board.CheckWinner() != Winner.None)
		{
			int score = GetHeuristicValue(board.winCorridors, board);
			return new NewMove(score, FutureMove.NoMove);
		}

		if (maximizingPlayer)
		{
			if (board.PieceCount(playerColor, playerShape) <= 0)
			{
				playerShape = playerShape == PShape.Round ? PShape.Square : PShape.Round;
			}
			else SetPlayerColorShape();
			int bestScore = 0;
			FutureMove bestMove = new FutureMove();
			int maxEval = int.MinValue + 1;
			for (int i = 0; i < board.rows; i++)
			{
				board.DoMove(playerShape, i);
				NewMove move = Minimax(board, depth - 1, alpha, beta, false, ct);
				Move lastMove = board.UndoMove();
				maxEval = Math.Max(maxEval, move.Score);
				alpha = Math.Max(alpha, move.Score);
				if (beta <= alpha) break;
				if (move.Score > bestScore)
				{
					bestScore = move.Score;
					bestMove = new FutureMove(lastMove.col, lastMove.piece.shape);
				}
			}
			return new NewMove(bestScore, bestMove);
		}
		else
		{
			if (board.PieceCount(enemyColor, enemyShape) <= 0)
			{
				enemyShape = enemyShape == PShape.Round ? PShape.Square : PShape.Round;
			}
			else SetPlayerColorShape();
			int bestScore = 0;
			FutureMove bestMove = new FutureMove();
			int minEval = int.MaxValue - 1;
			for (int i = 0; i < board.rows; i++)
			{
				board.DoMove(enemyShape, i);
				NewMove move = Minimax(board, depth - 1, alpha, beta, false, ct);
				Move lastMove = board.UndoMove();
				minEval = Math.Min(minEval, move.Score);
				beta = Math.Min(beta, move.Score);
				if (beta <= alpha) break;
				if (move.Score > bestScore)
				{
					bestScore = move.Score;
					bestMove = new FutureMove(lastMove.col, lastMove.piece.shape);
				}
			}
			return new NewMove(bestScore, bestMove);
		}
    }

    private int GetHeuristicValue(IEnumerable<IEnumerable<Pos>> corridors, Board board)
    {
        return random.Next(-10, 10);
    }

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
