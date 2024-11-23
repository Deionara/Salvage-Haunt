using SalvageHaunt;
using Sandbox;
using System.Reflection.Metadata.Ecma335;

public sealed class SellZone : Component, Component.ITriggerListener
{
	public static SellZone Instance { get; set; }
	[Property]
	public TextRenderer ItemsListText { get; set; }
	[Property]
	public TextRenderer CurrentQuotaText { get; set; }

	public List<GameObject> Props = new List<GameObject>();
	void ITriggerListener.OnTriggerEnter(Sandbox.Collider other)
	{
		var item = other.GameObject;
		foreach ( var _item in Props )
			if (item == _item)
				return;
		Props.Add( item );
	}

	void ITriggerListener.OnTriggerExit(Sandbox.Collider other)
	{
		var item = other.GameObject;

		if(Props.Contains(item))
			Props.Remove( item);
	}
	protected override void OnUpdate()
	{
		string line = "";
		int quota = 0;
		foreach ( var item in Props )
		{ 
			line = Props.Count > 0 ? string.Join( "\n", Props.Where( item => item != null ).Select( item => $"{item.GetComponent<Item>().Name} - {item.GetComponent<Item>().Price}$" ) ) : "Kein Props";
		}
		foreach ( var item in Props )
		{
			quota = Props.Sum( i => i != null ? i.GetComponent<Item>().Price : 0 );
		}
		ItemsListText.Text = line;
		CurrentQuotaText.Text = quota.ToString() + " / " + Game.Instance.currentQuota.ToString() +"$";
	}

	protected override void OnAwake()
	{
		Instance = this;
	}
}
