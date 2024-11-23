using Sandbox;
using System;

public sealed class Inventory : Component
{
	public static Inventory Instance {  get; set; }

	[Property]
	public List<GameObject> inventory = new List<GameObject>();

	[Property]
	public GameObject TargetPivotPickableDrop;
	private GameObject _currentSlotProp;

	[Property]
	public GameObject WeaponParent;

	[Property]
	public int _currentSlot { get; set; } = 0;

	[Property]
	public int maxSlots { get; set; } = 3;

	public bool _isFull { get; set; }

	protected override void OnAwake()
	{
		Instance = this;
	}

	protected override void OnUpdate()
	{
		Debug();


		if ( Input.Pressed( "reload" ) && _currentSlotProp != null )
			Drop( );

		if(Input.Pressed("slot1"))
			_currentSlot = 0;
		if ( Input.Pressed( "slot2" ) )
			_currentSlot = 1;
		if ( Input.Pressed( "slot3" ) )
			_currentSlot = 2;

		SelectSlot(_currentSlot );
		_isFull = inventory.Count >= maxSlots;
	}

	private void SelectSlot( int currentSlot )
	{
		if ( inventory.Count > 0 )
		{
			foreach ( var prop in inventory )
			{
				if( prop != null )
					prop.Enabled = false;
				_currentSlotProp = null;
			}
			for ( int i = 0; i < inventory.Count; i++ )
			{
				if ( inventory[i] != null && inventory[_currentSlot] != null )
				{
					_currentSlotProp = inventory[currentSlot];
					_currentSlotProp.Enabled = true;
				}
			}

		}
	}
	public void Drop()
	{
		for ( int i = 0; i < inventory.Count; i++ )
		{
			if ( inventory[i] != null && _currentSlotProp== inventory[i] && _currentSlotProp != null)
			{
				_currentSlotProp.Parent = null;
				_currentSlotProp.GetComponent<Collider>().IsTrigger = false;
				_currentSlotProp.GetComponent<Rigidbody>().Gravity = true;
				_currentSlotProp.WorldPosition = TargetPivotPickableDrop.WorldPosition;
				_currentSlotProp.WorldRotation = TargetPivotPickableDrop.WorldRotation;
				inventory.Remove(_currentSlotProp);
				Log.Info( $"{_currentSlotProp.Name} ist aus weg" );
			}
		}
	}
	public void Add( GameObject gameObject )
	{
		if ( !_isFull )
		{
			inventory.Add( gameObject );
			gameObject.Parent = WeaponParent;
			gameObject.WorldPosition = WeaponParent.WorldPosition;
			gameObject.WorldRotation = WeaponParent.WorldRotation;
			gameObject.GetComponent<Rigidbody>().Gravity = false;
			gameObject.GetComponent<Collider>().IsTrigger = true;
			gameObject.Enabled = false;
		}
		else
		{
			Log.Warning( "Inventory is full" );
			return;
		}
	}
	private void Debug()
	{
		var item_list = "";
		if ( inventory.Count > 0 )
			foreach ( var item in inventory )
			{
				if( item != null )
					item_list += item.Name + ",";
			}
		Gizmo.Draw.ScreenText( $"current slot{_currentSlot.ToString()}" +
			"\n" +
			$"Inventory items: {item_list}",
			new Vector2( 20, 100 ) );
	}
}
