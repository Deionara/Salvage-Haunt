using SalvageHaunt;
using Sandbox;

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
		var camPos = Camera.WorldPosition;
		var trace = Scene.Trace.Ray( camPos, camPos + (Camera.WorldRotation.Forward * DistancePickup))
			.WithoutTags("player")
			.Run();

		if ( _currentProp != null && Input.Pressed( "use" ))
		{
			Inventory.Instance.Add( _currentProp );
		}

		if ( trace.Hit )
		{
			var obj = trace.GameObject;

			foreach ( var item in Inventory.Instance.inventory )
			{
				if ( item == _currentProp )
					_currentProp = null;
			}
			if ( obj.GetComponent<SalvageHaunt.Prop>() != null )
			{
				_currentProp = obj;
			}
			else
				_currentProp = null;
		}
		else
			_currentProp = null;
	}
	protected override void DrawGizmos()
	{
		Gizmo.Draw.Line( Camera.LocalPosition, Camera.LocalPosition + Vector3.Forward * DistancePickup );
	}

	public string GetPropName()
	{
		return _currentProp != null ? _currentProp.GetComponent<SalvageHaunt.Prop>().Name : "";
	}

	public string GetKostetProp()
	{
		if ( _currentProp == null )
			return "";
		return _currentProp != null ? _currentProp.GetComponent<SalvageHaunt.Prop>().Kostet.ToString() + "$" : "";
	}

	public string GetInventoryPropsName()
	{
		string line = "";
		foreach ( var prop in Inventory.Instance.inventory )
		{
			 line += prop.Name;
		}
		return line;
	}
}
