using UnityEngine;
using System.Collections;

public class VehicleSplineController : MonoBehaviour 
{
	public float acceleration;
	public float strafeAcceleration;

	public float maxSpeed;
	public float speedDamping;

	public float maxStrafe;
	public float strafeDamping;

	public Transform anchorTransform;

	public float speed;
	float strafe;
	float speedFactor;
	float targetSpeedFactor;

	SplineInterpolator splineInterpolator;

	void Awake()
	{
		this.speed = 0.0f;
		this.strafe = 0.0f;
		this.speedFactor = 1.0f;
		this.targetSpeedFactor = 1.0f;

		this.splineInterpolator = GetComponent<SplineInterpolator>();

		this.audio.volume = 0f;
	}

	public void accelerate()
	{
		changeDrivingSoundLvl ();
		this.speed = Mathf.Clamp(this.speed + this.acceleration * Time.deltaTime, -this.maxSpeed, +this.maxSpeed);
	}
	
	public void brake()
	{
		this.speed = Mathf.Clamp(this.speed - this.acceleration * Time.deltaTime, -this.maxSpeed, +this.maxSpeed);
	}

	public void changeDrivingSoundLvl() {
		if (this.speed >= 0.1f * this.maxSpeed && this.speed < 0.25f * this.maxSpeed) {
			this.audio.volume = 0.25f;
		} else if (this.speed >= 0.25f * this.maxSpeed && this.speed < 0.5f * this.maxSpeed) {
			this.audio.volume = 0.5f;
		} else if (this.speed >= 0.5f * this.maxSpeed && this.speed < 0.75f * this.maxSpeed) {
			this.audio.volume = 0.75f;
		} else if (this.speed >= 0.75f * this.maxSpeed) {
			this.audio.volume = 1f;
		} else {
			this.audio.volume = 0f;
		}
	}

	public void dampSpeed()
	{
		changeDrivingSoundLvl ();
		this.speed = Mathf.Lerp(this.speed, 0.0f, Time.deltaTime * this.speedDamping);
	}

	public void strafeLeft()
	{
		dampSpeed ();
		this.strafe = Mathf.Clamp(this.strafe - this.strafeAcceleration * Time.deltaTime, -this.maxStrafe, +this.maxStrafe);
	}
	
	public void strafeRight()
	{
		dampSpeed ();
		this.strafe = Mathf.Clamp(this.strafe + this.strafeAcceleration * Time.deltaTime, -this.maxStrafe, +this.maxStrafe);
	}
	
	public void dampStrafe()
	{
		this.strafe = Mathf.Lerp(this.strafe, 0.0f, Time.deltaTime * this.strafeDamping);
	}

	public void setSpeedFactor(float f)
	{
		this.targetSpeedFactor = f;
	}

	void Update()
	{
		this.speedFactor = Mathf.Lerp(this.speedFactor, this.targetSpeedFactor, Time.deltaTime * 2.0f);

		this.splineInterpolator.Speed = this.speed * this.speedFactor;
		this.anchorTransform.localPosition = new Vector3(this.strafe, 0, 0);
	}

	void LateUpdate()
	{
		RaycastHit groundHit;
		Physics.Raycast(anchorTransform.position, -transform.up, out groundHit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground"));

		float yDiff = 0;
		if (groundHit.distance > 1.2f)
			yDiff = 1.2f - groundHit.distance;

		this.anchorTransform.localPosition = new Vector3(this.anchorTransform.localPosition.x, yDiff, 0);
	}
}
