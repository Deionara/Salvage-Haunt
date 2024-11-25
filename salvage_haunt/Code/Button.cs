using Sandbox;
using System;

public sealed class Button : Component
{
	public event Action<bool> OnPressed;


	public void Press(bool value)
	{
		OnPressed?.Invoke(value);
	}
}
