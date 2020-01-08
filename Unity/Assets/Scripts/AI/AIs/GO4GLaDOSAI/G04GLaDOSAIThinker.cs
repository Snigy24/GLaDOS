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

    //private int alpha;
    //private int beta;
    private int score;

    /// <summary>
    /// Create a new instance of G04GLaDOSAIThinker.
    /// </summary>
    public G04GLaDOSAIThinker(int depth)
    {
        random = new System.Random();
        staticEvaluation = new G04GLaDOSStaticEvaluation();
        ResetAIVariables();
        this.depth = depth;

    }

    public FutureMove Think(Board board, CancellationToken ct)
    {

        playerColor = board.Turn;
        ResetAIVariables();
        SetPlayerShape();
        //FutureMove futureMove = Minimax(board, depth, int.MinValue + 1, int.MaxValue - 1, true, ct);
        //Debug.LogWarning("Final Score: " + score);
        //return futureMove;
        Move move;
        FutureMove bestMove = new FutureMove();
        int bestScore = 0;
        for (int i = 0; i < board.cols; i++)
        {
            if(ct.IsCancellationRequested) return FutureMove.NoMove;

            if (board.IsColumnFull(i)) continue;

            if (board.PieceCount(playerColor, playerShape) <= 0) playerShape = PShape.Square;

            board.DoMove(playerShape, i);
            FutureMove futureMove = Minimax(board, depth - 1, int.MinValue + 1, int.MaxValue - 1, false, ct);
            move = board.UndoMove();
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = new FutureMove(move.col, move.piece.shape);
            }
        }
        Debug.LogWarning("Final Score: " + score);
        return bestMove;
        //Move bestMove = new Move();
        //playerColor = board.Turn;
        //int bestScore = -1;
        //SetPlayerShape();
        //Move move;
        //for (int i = 0; i < board.cols; i++)
        //{
        //    if (ct.IsCancellationRequested) return FutureMove.NoMove;
        //    if (board.IsColumnFull(i)) continue;
        //    if (board.PieceCount(playerColor, playerShape) <= 0) playerShape = PShape.Square;
        //    board.DoMove(playerShape, i);
        //    FutureMove futureMove = Minimax(board, 2, false, ct);
        //    move = board.UndoMove();
        //    if (maxScore > bestScore)
        //    {
        //        bestScore = maxScore;
        //        bestMove = move;
        //    }
        //}
        //return new FutureMove(bestMove.col, bestMove.piece.shape);
    }

    private FutureMove Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer, CancellationToken ct)
    {
        if (ct.IsCancellationRequested) return FutureMove.NoMove;

        if (depth <= 0 || board.CheckWinner() != Winner.None)
        {
            score = GetHeuristicValue(board.winCorridors, board);
            return new FutureMove();
        }

        if (maximizingPlayer)
        {
            FutureMove futureMove = new FutureMove();
            PShape shape = PShape.Round;
            int maxEval = int.MinValue + 1;
            for (int i = 0; i < board.cols; i++)
            {
                if (board.PieceCount(playerColor, shape) <= 0) break;
                if (board.IsColumnFull(i)) continue;
                board.DoMove(shape, i);
                futureMove = Minimax(board, depth - 1, alpha, beta, false, ct);
                Move move = board.UndoMove();
                maxEval = Math.Max(maxEval, score);
                alpha = Math.Max(alpha, score);
                if (beta <= alpha) break;
                futureMove = new FutureMove(move.col, move.piece.shape);
                if (ct.IsCancellationRequested) return FutureMove.NoMove;
            }
            shape = PShape.Square;
            for (int i = 0; i < board.cols; i++)
            {
                if (board.PieceCount(playerColor, shape) <= 0) break;
                if (board.IsColumnFull(i)) continue;
                board.DoMove(shape, i);
                futureMove = Minimax(board, depth - 1, alpha, beta, false, ct);
                Move move = board.UndoMove();
                maxEval = Math.Max(maxEval, score);
                alpha = Math.Max(alpha, score);
                if (beta <= alpha) break;
                futureMove = new FutureMove(move.col, move.piece.shape);
                if (ct.IsCancellationRequested) return FutureMove.NoMove;
            }
            return futureMove;
        }
        else
        {
            FutureMove futureMove = new FutureMove();
            PColor enemyColor;
            if (playerColor == PColor.Red)
            {
                enemyColor = PColor.White;
            }
            else enemyColor = PColor.Red;
            PShape shape = PShape.Round;
            int minEval = int.MaxValue - 1;
            for (int i = 0; i < board.cols; i++)
            {
                if (board.PieceCount(enemyColor, shape) <= 0) break;
                if (board.IsColumnFull(i)) continue;
                board.DoMove(shape, i);
                futureMove = Minimax(board, depth - 1, alpha, beta, false, ct);
                Move move = board.UndoMove();
                minEval = Math.Min(minEval, score);
                beta = Math.Min(beta, score);
                if (beta <= alpha) break;
                futureMove = new FutureMove(move.col, move.piece.shape);
                if (ct.IsCancellationRequested) return FutureMove.NoMove;
            }
            shape = PShape.Square;
            for (int i = 0; i < board.cols; i++)
            {
                if (board.PieceCount(enemyColor, shape) <= 0) break;
                if (board.IsColumnFull(i)) continue;
                board.DoMove(shape, i);
                futureMove = Minimax(board, depth - 1, alpha, beta, false, ct);
                Move move = board.UndoMove();
                minEval = Math.Min(minEval, score);
                beta = Math.Min(beta, score);
                if (beta <= alpha) break;
                futureMove = new FutureMove(move.col, move.piece.shape);
                if (ct.IsCancellationRequested) return FutureMove.NoMove;
            }
            return futureMove;
        }

        //      if (ct.IsCancellationRequested) return 0;
        //      if (depth <= 0 || board.CheckWinner() != Winner.None)
        //{
        //	int staticEvaluation = GetHeuristicValue(board.winCorridors, board, playerColor);
        //	return staticEvaluation;
        //}
        //if (maximizingPlayer)
        //{
        //	PShape shape = PShape.Round;
        //	int maxEval = int.MinValue + 1;
        //	for (int i = 0; i < board.cols; i++)
        //	{
        //		if (board.PieceCount(playerColor, shape) <= 0) break;
        //		if (board.IsColumnFull(i)) continue;
        //		board.DoMove(shape, i);
        //		int eval = Minimax(board, depth - 1, alpha, beta, false, playerColor, ct);
        //		board.UndoMove();
        //		maxEval = Math.Max(maxEval, eval);
        //		alpha = Math.Max(alpha, eval);
        //		if (beta <= alpha) break;
        //	}
        //	shape = PShape.Square;
        //	for (int i = 0; i < board.cols; i++)
        //	{
        //		if (board.PieceCount(playerColor, shape) <= 0) break;
        //		if (board.IsColumnFull(i)) continue;
        //		board.DoMove(shape, i);
        //		int eval = Minimax(board, depth - 1, alpha, beta, false, playerColor, ct);
        //		board.UndoMove();
        //		maxEval = Math.Max(maxEval, eval);
        //		alpha = Math.Max(alpha, eval);
        //		if (beta <= alpha) break;
        //	}
        //	return maxEval;
        //}
        //else
        //{
        //	PShape shape = PShape.Round;
        //	int minEval = int.MaxValue - 1;
        //	for (int i = 0; i < board.cols; i++)
        //	{
        //		if (board.PieceCount(playerColor, shape) <= 0) break;
        //		if (board.IsColumnFull(i)) continue;
        //		board.DoMove(shape, i);
        //		int eval = Minimax(board, depth - 1, alpha, beta, true, playerColor, ct);
        //		board.UndoMove();
        //		minEval = Math.Min(minEval, eval);
        //		beta = Math.Min(beta, eval);
        //		if (beta <= alpha) break;

        //	}
        //	shape = PShape.Square;
        //	for (int i = 0; i < board.cols; i++)
        //	{
        //		if (board.PieceCount(playerColor, shape) <= 0) break;
        //		if (board.IsColumnFull(i)) continue;
        //		board.DoMove(shape, i);
        //		int eval = Minimax(board, depth - 1, alpha, beta, true, playerColor, ct);
        //		board.UndoMove();
        //		minEval = Math.Min(minEval, eval);
        //		beta = Math.Min(beta, eval);
        //		if (beta <= alpha) break;
        //	}
        //	return minEval;
        //}
    }

    private int GetHeuristicValue(IEnumerable<IEnumerable<Pos>> corridors, Board board)
    {
        //if (playerColor == PColor.White) playerShape = PShape.Round;
        //else playerShape = PShape.Square;
        //int total = 0;
        //int count;
        //foreach (IEnumerable<Pos> corridor in corridors)
        //{
        //    count = 0;
        //    foreach (Pos p in corridor)
        //    {

        //        if (!board[p.row, p.col].HasValue) continue;
        //        Piece piece = board[p.row, p.col].Value;
        //        if (board[p.row, p.col].HasValue && piece.Is(playerColor, playerShape)) count *= 2;
        //        else if (board[p.row, p.col].HasValue && !piece.Is(playerColor, playerShape)) count /= 2;
        //    }
        //    total += count;
        //}
        //return total;
        return random.Next(-100, 100);
    }

    private void SetPlayerShape()
    {
        if (playerColor == PColor.White) playerShape = PShape.Round;
        else
        {
            playerShape = PShape.Square;
        }
    }

    private void ResetAIVariables()
    {
        //alpha = int.MinValue + 1;
        //beta = int.MaxValue - 1;
        score = -1;
    }
}
