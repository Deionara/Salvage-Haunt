using Sandbox;
using System;

public sealed class Game : Component
{
	public static Game Instance {  get; set; }
	public int currentQuota = 0;
	public int neededQuota = 1200;
	public int money = 0;
	int seed = -1;
	protected override void OnAwake()
	{
		Instance = this;
		StartGame();

	}

	public void StartGame()
	{
		var rand = new Random();
		seed = rand.Next();
		currentQuota = rand.Next( 350,neededQuota );
		Log.Info( $"{seed}, { currentQuota}");
	}
}
