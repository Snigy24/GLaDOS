using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G04GLaDOSStaticEvaluation
{
	

	public int GetHeuristicValue(IEnumerable<IEnumerable<Pos>> corridors, Board board, PColor playerColor, PShape playerShape)
	{
		int aiTotal = 1;
		int enemyTotal = 1;
		int total = 0;
		int count;

		foreach (IEnumerable<Pos> corridor in corridors)
		{
			count = 0;
			foreach (Pos p in corridor)
			{
				if (!board[p.row, p.col].HasValue) continue;

				Piece piece = board[p.row, p.col].Value;
				if (piece.Is(playerColor, playerShape))
				{
					count++;
					aiTotal += 1 * count;
				}
				else
				{
					aiTotal -= 1 * count;
					break;
				}
			}
		}

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
		total = aiTotal - enemyTotal;
		return total;
	}
}
