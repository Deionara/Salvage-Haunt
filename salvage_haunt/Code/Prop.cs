using Sandbox;
namespace SalvageHaunt
{
	public sealed class Prop : Component
	{
		[Property] public string Name { get; set; }
		[Property] public int Kostet { get; set; } = 50;
	}
}
