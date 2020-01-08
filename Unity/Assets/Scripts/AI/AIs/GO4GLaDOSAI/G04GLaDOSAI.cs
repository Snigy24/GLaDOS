using UnityEngine;
public class G04GLaDOSAI : AIPlayer
{
	public override string PlayerName => "GLaDOSAI";

	public override IThinker Thinker => thinker;

	// Suport variable for RandomAI's thinker instance
	private IThinker thinker;

    [SerializeField] private int searchDepth = 0;

	/// <summary>
	/// This method will be called before a match starts and is used for
	/// instantiating a new <see cref="G04GLaDOSAIThinker"/>.
	/// </summary>
	/// <seealso cref="AIPlayer.Setup"/>
	public override void Setup()
	{
		thinker = new G04GLaDOSAIThinker(searchDepth);
	}
}
