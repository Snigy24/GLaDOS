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

	}

	private float Minimax(Board board, Pos pos, int depth, int alpha, int beta)
	{
		
		if (depth == 0 || board.CheckWinner() != Winner.None)
		{
			//return
		}

		if (board.Turn == PColor.)
		{
			int maxEval = int.MinValue;

		}
	}
}
