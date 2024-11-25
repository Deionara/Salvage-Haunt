using Sandbox;
using Sandbox.Citizen;

public enum ViewState
{
	First,
	Third
}
public sealed class Player : Component
{
	public int Money { get; set; } = 100;
	public static Player Instance { get; set; }
	[Property]
	public ViewState viewState { get; set; }
	[Property]
	public float MoveSpeed { get; set; }
	[Property]
	public float RunSpeed { get; set; } = 500f;
	[Property]
	public float DuckSpeed { get; set;}
	[Property]
	public float JumpStrong { get; set; }
	[Property]
	public GameObject Camera;

	public PlayerPickup PlayerPickup { get; set; }

	SkinnedModelRenderer skinnedModelRenderer { get; set; }
	public CharacterController controller;
	CitizenAnimationHelper animationHelper;
	protected override void OnFixedUpdate()
	{
		Movement();
		

		if ( Input.Pressed( "attack1" ) )
			Money -= 5;
	}

	protected override void OnUpdate()
	{
		Look(); 
	}
	bool isDuck = false;

	private void Movement()
	{
		var currentSpeed = Input.Down( "run" ) && !isDuck ? RunSpeed : MoveSpeed;
		var duckSpeed = Input.Down("duck") ? DuckSpeed : 0;

		var moveVel = Input.AnalogMove.Normal * WorldRotation * (currentSpeed - duckSpeed);

		controller.Accelerate( moveVel );

		if ( controller.IsOnGround )
		{
			animationHelper.DuckLevel = 0;
			isDuck = false;
			controller.ApplyFriction( 5f );
			controller.Acceleration = 10f;
			animationHelper.WithVelocity( moveVel );

			if(Input.Down("duck"))
			{
				isDuck = true;
				animationHelper.DuckLevel = 1;
			}
			if ( Input.Pressed( "Jump" ) )
			{
				controller.Punch( Vector3.Up * JumpStrong );
				animationHelper.TriggerJump();
			}
		}
		else
		{
			controller.Acceleration = 3f;
			controller.Velocity += Scene.PhysicsWorld.Gravity * Time.Delta;
		}
		controller.Move();
		
		animationHelper.IsGrounded = controller.IsOnGround;
	}

	[Property]
	public Vector3 EyesPosition { get; set; }

	[Property] 
	public Vector3 DuckEyePosition { get; set; }
	private Angles EyeAngles { get; set; }
	private Transform _cameraPosition;
	private void Look()
	{
		EyeAngles += Input.AnalogLook;
		EyeAngles = EyeAngles.WithPitch( MathX.Clamp( EyeAngles.pitch, -89, 89 ) );
		WorldRotation = Rotation.FromYaw( EyeAngles.yaw );

		if ( Camera != null )
		{
				Camera.Transform.Local = _cameraPosition.RotateAround( EyesPosition, EyeAngles.WithYaw( 0 ) );
			if(isDuck) Camera.LocalPosition -= DuckEyePosition;
		}
		animationHelper.WithLook( Camera.WorldRotation.Forward );

	}
	protected override void OnAwake()
	{
		if(Instance == null)
			Instance = this;
		controller = GetComponent<CharacterController>();
		animationHelper = GetComponent<CitizenAnimationHelper>();
		PlayerPickup = GetComponent<PlayerPickup>();

		_cameraPosition = Camera.Transform.Local;
	}

	protected override void DrawGizmos()
	{
		Gizmo.Draw.LineSphere(EyesPosition, 9);
	}

	protected override void OnValidate()
	{
		if ( skinnedModelRenderer == null )
			skinnedModelRenderer = GetComponent<SkinnedModelRenderer>();
		switch ( viewState )
		{
			case ViewState.First:
				skinnedModelRenderer.RenderOptions.Game = false;
				break;
			case ViewState.Third:
				skinnedModelRenderer.RenderOptions.Game = true;
				break;
			default:
				break;
		}
	}
}
