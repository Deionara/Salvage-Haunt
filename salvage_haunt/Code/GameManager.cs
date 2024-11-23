using Sandbox;

public sealed class GameManager : GameObjectSystem<GameManager>, ISceneStartup
{
	public GameManager( Scene scene ) : base( scene )
	{
	}
	//void ISceneStartup.OnHostInitialize()
	//{
	//	var slo = new SceneLoadOptions();
	//	slo.IsAdditive = true;
	//	slo.SetScene( "scenes/level1.scene" );
	//	Scene.Load( slo );
	//}
}
