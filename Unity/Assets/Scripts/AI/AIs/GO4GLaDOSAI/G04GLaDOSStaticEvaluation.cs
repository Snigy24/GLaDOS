using System.Collections.Generic;

/// <summary>
/// Provides methods to get a static evaluation for a node.
/// </summary>
public class G04GLaDOSStaticEvaluation
{
	
    /// <summary>
    /// Evaluates the board and compares it to the win corridors, returning a score.
    /// </summary>
    /// <param name="corridors">Victory conditions, the set in a row to win.</param>
    /// <param name="board">Game board copy.</param>
    /// <param name="playerColor">The AI color.</param>
    /// <param name="playerShape">And the AI main shape.</param>
    /// <returns>Score of how good the board is to the AI or the enemy.</returns>
	public int GetHeuristicValue(IEnumerable<IEnumerable<Pos>> corridors, Board board, PColor playerColor, PShape playerShape)
	{
        // AI score.
		int aiTotal = 1;
		
        // Enemy score.
        int enemyTotal = 1;
		
        // Count multiplier for each piece in a row.
        int count;

        // Checks all the corridors and compares it to the board, increasing the score for each piece from the AI that is found.
		foreach (IEnumerable<Pos> corridor in corridors)
		{
			count = 0;
			foreach (Pos p in corridor)
			{
                // Checks if there is a piece in that position.
				if (!board[p.row, p.col].HasValue) continue;

                // Gets the piece and checks if it is the AI's piece and shape.
				Piece piece = board[p.row, p.col].Value;
				if (piece.Is(playerColor, playerShape))
				{
                    // Increases the board score if there are pieces in a line.
					count++;
					aiTotal += 1 * count;
				}
				else
				{
                    // Decreases if the line is interrupted.
					aiTotal -= 1 * count;
					break;
				}
			}
		}

        // Checks all the corridors and compares it to the board, increasing the score for each piece from the enemy that is found.
        foreach (IEnumerable<Pos> corridor in corridors)
		{
			count = 0;
			foreach (Pos p in corridor)
			{
				if (!board[p.row, p.col].HasValue) continue;

				Piece piece = board[p.row, p.col].Value;
				if (!piece.Is(playerColor, playerShape))
				{
					count++;
					enemyTotal -= 1 * count;
				}
			}
		}

        // Returns the ai score minus the enemy score.
		return aiTotal - enemyTotal;
    }
}
