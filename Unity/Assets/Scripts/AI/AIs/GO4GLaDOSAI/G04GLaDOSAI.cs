
public class G04GLaDOSAI : AIPlayer
{
	public override string PlayerName => "GLaDOSAI";

	public override IThinker Thinker => thinker;

	// Suport variable for RandomAI's thinker instance
	private IThinker thinker;

	/// <summary>
	/// This method will be called before a match starts and is used for
	/// instantiating a new <see cref="RandomAIThinker"/>.
	/// </summary>
	/// <seealso cref="AIPlayer.Setup"/>
	public override void Setup()
	{
		thinker = new RandomAIThinker();
	}
}
