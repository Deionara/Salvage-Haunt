using Sandbox;
using System;
namespace SalvageHaunt
{
	public sealed class Item : Component
	{
		[Property] public string Name { get; set; }
		[Property] public int Price { get; set; } = 50;
		[Property] public string IconPath { get; set; } = "Assets/models/Crates_and_barrels_2.png";

		protected override void OnStart()
		{
			var rand = new Random();
			Price = rand.Next(10, Price);
		}
	}
}
