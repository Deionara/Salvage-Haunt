using SalvageHaunt;
using Sandbox;
using System;

public sealed class PlayerPickup : Component
{

	[Property]
	public float DistancePickup { get; set; }

	[Property]
	public GameObject Camera;

	[Property]
	public GameObject _currentProp;
	

	protected override void OnUpdate()
	{
		Tracing();
		
	}

	private void Tracing()
	{
		var camPos = Camera.WorldPosition;
		var trace = Scene.Trace.Ray( camPos, camPos + (Camera.WorldRotation.Forward * DistancePickup) )
			.WithoutTags( "player" )
			.Run();

		if ( _currentProp != null && Input.Pressed( "use" ) )
		{
			PlayerInventory.Instance.Add( _currentProp );
		}

		//Item trace
		if ( trace.Hit )
		{
			var obj = trace.GameObject;

			foreach ( var item in PlayerInventory.Instance.inventory )
			{
				if ( item == _currentProp )
					_currentProp = null;
			}
			if ( obj.GetComponent<SalvageHaunt.Item>() != null )
			{
				_currentProp = obj;
			}
			else
				_currentProp = null;
		}
		else
			_currentProp = null;
		//Button trace
		if(trace.Hit && trace.GameObject.GetComponent<Button>() != null)
		{
			//sell items
			var button = trace.GameObject.GetComponent<Button>();
			button.OnPressed += ( bool value ) =>
			{
				if ( value )
					SellZone.Instance.Sell();
			};
			if ( Input.Pressed( "use" ) )
			{
				button.GetComponent<SoundBoxComponent>().StartSound();
				button.Press( true );
			}
			else
				button.Press( false );
		}

	}

	protected override void DrawGizmos()
	{
		Gizmo.Draw.Line( Camera.LocalPosition, Camera.LocalPosition + Vector3.Forward * DistancePickup );
	}

	public string GetPropName()
	{
		return _currentProp != null ? _currentProp.GetComponent<SalvageHaunt.Item>().Name : "";
	}

	public string GetKostetProp()
	{
		if ( _currentProp == null )
			return "";
		return _currentProp != null ? _currentProp.GetComponent<SalvageHaunt.Item>().Price.ToString() + "$" : "";
	}

	public string GetInventoryPropsName()
	{
		string line = "";
		foreach ( var prop in PlayerInventory.Instance.inventory )
		{
			 line += prop.Name;
		}
		return line;
	}
}
