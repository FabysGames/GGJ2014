using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public int controllerId;

	public KeyCode keyForward; 
	public KeyCode keyBack;
	public KeyCode keyRight;
	public KeyCode keyLeft;

	public GameObject innerBody;
	public GameObject middleBody;
	public GameObject outerBody;

	public bool won = false;
	public int lives = 3;


	VehicleSplineController vehicleController;
	GameController gameController;



	public VehicleSplineController VehicleController
	{
		get { return this.vehicleController; }
	}

	void Awake()
	{
		this.vehicleController = GetComponent<VehicleSplineController>();
	}

	void Start()
	{
		GameObject gameControllerObj = GameObject.FindGameObjectWithTag ("GameController");
		if(gameControllerObj)
			this.gameController = gameControllerObj.GetComponent<GameController>();

		if (Input.GetJoystickNames ().Length < this.controllerId)
			this.controllerId = 0;

		GetComponent<HUDPlayerInfo> ().Lives = this.lives;
	}

	/*public float maxTorque;
	public float maxSpeed;
	
	int forward = 0;
	int right = 0;
	
	void FixedUpdate()
	{
		if(this.forward != 0)
			rigidbody.AddForce(this.forward * transform.forward * this.maxSpeed);
		
		if (this.right != 0)
			transform.rotation = transform.rotation * Quaternion.AngleAxis(this.right * maxTorque * Mathf.Deg2Rad, Vector3.up);
		
		rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, this.maxSpeed);
	}*/
	
	void Update()
	{
		if (won) 
		{
			this.vehicleController.accelerate();
			GetComponent<CheckpointPlayerController>().checkPointPassed = -1;
		}

		if (this.gameController != null && !this.gameController.GameIsActive()) return;

		if ((this.controllerId > 0 && Input.GetAxis("Vertical_"+this.controllerId) > 0.8f) || (this.controllerId == 0 && Input.GetKey (this.keyForward)))
			this.vehicleController.accelerate();
		//else if (Input.GetKey (this.keyBack))
		//	this.vehicleController.brake();
		else
			this.vehicleController.dampSpeed();

		if ((this.controllerId > 0 && Input.GetAxis("Horizontal_"+this.controllerId) < -0.8f) || (this.controllerId == 0 && Input.GetKey(this.keyLeft)))
			this.vehicleController.strafeLeft();
		else if ((this.controllerId > 0 && Input.GetAxis("Horizontal_"+this.controllerId) > 0.8f) || (this.controllerId == 0 && Input.GetKey (this.keyRight)))
			this.vehicleController.strafeRight();
		//else
		//	this.vehicleController.dampStrafe();
	}
	
	public void DamagePlayer()
	{
		if (this.outerBody != null) 
		{
			Destroy (this.outerBody);
		}else if (this.middleBody != null) 
		{
			Destroy (this.middleBody);
		}else if (this.innerBody != null) 
		{
			Destroy (this.innerBody);
			this.gameController.LostGame(this);
		}

		this.lives--;

		GetComponent<HUDPlayerInfo> ().Lives = this.lives;
	}
}
