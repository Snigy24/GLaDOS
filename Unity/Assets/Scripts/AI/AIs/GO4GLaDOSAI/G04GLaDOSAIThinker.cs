using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class G04GLaDOSAIThinker : IThinker
{

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
		int bestScore = int.MinValue + 1;
		FutureMove bestMove = new FutureMove();
		for (int i = 0; i < board.cols; i++)
		{
			if (ct.IsCancellationRequested) return FutureMove.NoMove;

			if (board.PieceCount(playerColor, playerShape) < 1)
			{
				playerShape = playerShape == PShape.Round ? PShape.Square : PShape.Round;
			}

			if (board.IsColumnFull(i)) continue;

			board.DoMove(playerShape, i);
			board.Copy();
			int score = Minimax(board.Copy(), depth - 1, int.MinValue + 1, int.MaxValue - 1, false, ct);
			board.UndoMove();
			if (score > bestScore)
			{
				bestScore = score;
				bestMove = new FutureMove(i, playerShape);
			}
		}
		return bestMove;
	}

    private int Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer, CancellationToken ct)
    {
		if (ct.IsCancellationRequested) return 0;

		if (depth <= 0 || board.CheckWinner() != Winner.None)
		{
			if ((board.CheckWinner() == Winner.White && playerColor == PColor.White) || (board.CheckWinner() == Winner.Red && playerColor == PColor.Red))
			{
				return 1000;
			}
			else if ((board.CheckWinner() == Winner.White && enemyColor == PColor.White) || (board.CheckWinner() == Winner.Red && enemyColor == PColor.Red))
			{
				return -1000;
			}
			else if (board.CheckWinner() == Winner.Draw)
			{
				return -500;
			}

			return staticEvaluation.GetHeuristicValue(board.winCorridors, board, playerColor, playerShape);
		}

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
				if (beta <= alpha) break;
			}
			return maxEval;
		}
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
				if (beta <= alpha) break;
			}
			return minEval;
		}
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
