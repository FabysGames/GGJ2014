using UnityEngine;
using System.Collections;

public class VehicleController : MonoBehaviour 
{
	public float acceleration;
	public float rotationAcceleration;

	public float maxSpeed;
	public float speedDamping;

	public float height;
	public float groundOrientationSpeed;

	public float mass;

	public Transform entityTransform;
	public Transform orientationTransform;
	public Transform groundSensorBack;
	public Transform groundSensorFront;

	float speed;
	float rotation;
	bool gravity;
	float speedFactor;

	CharacterController characterController;

	void Awake()
	{
		speed = 0.0f;
		rotation = 0.0f;

		gravity = false;

		speedFactor = 1.0f;

		characterController = GetComponent<CharacterController>();
	}

	public void accelerate()
	{
		audio.volume = 1;
		this.speed = Mathf.Clamp(this.speed + this.acceleration * Time.deltaTime, -this.maxSpeed, +this.maxSpeed);
	}

	public void brake()
	{
		audio.volume = 0;
		this.speed = Mathf.Clamp(this.speed - this.acceleration * Time.deltaTime, -this.maxSpeed, +this.maxSpeed);
	}

	public void dampSpeed()
	{
		audio.volume = 1;
		this.speed = Mathf.Lerp(this.speed, 0.0f, Time.deltaTime * speedDamping);
	}

	public void rotateLeft()
	{
		this.rotation = this.rotation - this.rotationAcceleration * Time.deltaTime;
	}

	public void rotateRight()
	{
		this.rotation = this.rotation + this.rotationAcceleration * Time.deltaTime;
	}
	
	public void setGravity(bool on)
	{
		this.gravity = on;
	}

	public void setSpeedFactor(float f)
	{
		this.speedFactor = f;
	}
	
	void FixedUpdate()
	{
		RaycastHit groundHit;

		/*Physics.Raycast(this.groundSensorBack.position, -this.groundSensorBack.up, out groundHit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground"));
		Quaternion backRotation = Quaternion.FromToRotation(Vector3.up, groundHit.normal);
		Physics.Raycast(this.groundSensorFront.position, -this.groundSensorFront.up, out groundHit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground"));
		Quaternion frontRotation = Quaternion.FromToRotation(Vector3.up, groundHit.normal);*/

		Vector3[] sensors = new Vector3[]
		{
			new Vector3(-0.25f, 5.0f, -0.25f),
			new Vector3(0.25f, 5.0f, -0.25f),
			new Vector3(-0.25f, 5.0f, 0.25f),
			new Vector3(0.25f, 5.0f, 0.25f)
		};

		Physics.Raycast(transform.position, -transform.up, out groundHit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground"));
		Quaternion averageRotation = Quaternion.FromToRotation(Vector3.up, groundHit.normal);
		for (int i = 0; i < sensors.Length; i++)
		{
			Physics.Raycast(transform.position + sensors[i], -transform.up, out groundHit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground"));
			averageRotation = Quaternion.Lerp(averageRotation, Quaternion.FromToRotation(Vector3.up, groundHit.normal), 0.5f);
		}
		transform.rotation = Quaternion.Lerp(transform.rotation, averageRotation, Time.deltaTime * 5.0f);

		//Physics.Raycast(transform.position, -transform.up, out groundHit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground"));
		//Quaternion orientation = Quaternion.FromToRotation(Vector3.up, groundHit.normal);
		//transform.rotation = Quaternion.Lerp(transform.rotation, orientation * Quaternion.AngleAxis(this.rotation, Vector3.up), Time.deltaTime);
		//Quaternion orientation = Quaternion.Lerp(backRotation, frontRotation, 0.5f);
		//orientation = Quaternion.Lerp(transform.localRotation, orientation, Time.deltaTime * this.groundOrientationSpeed);
		//transform.localRotation = orientation;

		/*this.orientationTransform.localRotation = Quaternion.AngleAxis(this.rotation, Vector3.up);
		this.entityTransform.localRotation = this.orientationTransform.localRotation;*/

		//Vector3 movement = this.orientationTransform.localRotation * transform.forward * this.speed;
		this.characterController.Move(transform.forward * this.speed * this.speedFactor);
	}
	
	void LateUpdate()
	{
		RaycastHit groundHit;

		if (this.gravity)
		{
			Vector3 oldPosition = this.transform.position;
			this.transform.position -= transform.up * mass * 9.81f * Time.deltaTime;
			Vector3 direction = this.transform.position - oldPosition;

			if (Physics.Raycast(oldPosition, direction.normalized, out groundHit, direction.magnitude, 1 << LayerMask.NameToLayer("Ground")))
			{
				transform.position = groundHit.point + groundHit.normal * this.height;
				setGravity(false);
			}
		}
		else
		{
			Physics.Raycast(transform.position, -transform.up, out groundHit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground"));
			if (groundHit.distance > this.height)
				this.characterController.Move(-transform.up * (groundHit.distance - height));
			else
				this.characterController.Move(transform.up * (height - groundHit.distance));
		}
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		return;
		Vector3 reflectedVector = Vector3.Reflect(transform.forward, hit.normal);
		float angle = Vector3.Angle(transform.forward, reflectedVector);

		if (angle < 45.0f)
			return;

		Vector3 euler = transform.rotation.eulerAngles;
		euler.y = angle * Mathf.Sign(transform.forward.z);
		transform.rotation = Quaternion.Euler(euler);

		this.rotation = 0;
		this.speed = 0;
	}
}
