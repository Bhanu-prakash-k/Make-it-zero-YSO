using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;	
	
	private Vector3 currentPosition;
	private Quaternion currentRotation;

	public float distance;
	public float jumpHeight;
	public float duration;

	public bool canMove = true;
	public bool win = false;

	public bool left, right, up, down;

	public int firstValue;
	public TMP_Text valueText;

	public int numberOfObjects;
	public GameObject objectCollisionParticles;
	public ParticleSystem collisionParticles;

	public TMP_Text zeroText;

	Color firstColor;

	[SerializeField] private Vector3 fp;   //First touch position
	[SerializeField] private Vector3 lp;   //Last touch position
	public float dragDistance;  //minimum distance for a swipe to be registered
	//public GameObject[] allPlayers;
	public Material playerMaterial;
	public Color[] differentColors;

	private void Awake()
    {
        if(instance == null)
			instance = this;
    }

    // Start is called before the first frame update
    void Start()
	{
		left = right = up = down = false;
		canMove = true;
		currentPosition = transform.position;
		//valueText.text = firstValue.ToString();
		numberOfObjects = GameObject.FindGameObjectsWithTag("Objects").Length;
		//firstColor = GetComponent<Renderer>().material.color;
		zeroText.enabled = true;
		//dragDistance = 0.5f; //dragDistance is 15% height of the screen
		//allPlayers = GameObject.FindGameObjectsWithTag("Player");
		
		//transform.GetComponent<Renderer>().material.color = playerMaterial.color;
		collisionParticles.startColor = playerMaterial.color;
	}

	// Update is called once per frame
	void Update()
	{
		PlayerMovement();
		UpRayMethod();
		DownRayMethod();
		RightRayMethod();
		LeftRayMethod();
	}
	void PlayerMovement()
    {
		//if (Input.GetMouseButtonDown(0)) // user is touching the screen with a single touch
		//{
			//Touch touch = Input.GetTouch(0); // get the touch
			if (Input.GetMouseButtonDown(0)) //check for the first touch
			{
				fp = Input.mousePosition;
				lp = Input.mousePosition;
			}
			else if (Input.GetMouseButton(0)) // update the last position based on where they moved
			{
				lp = Input.mousePosition;
			}
			else if (Input.GetMouseButtonUp(0)) //check if the finger is removed from the screen
			{
				lp = Input.mousePosition;  //last touch position. Ommitted if you use list

				//Check if drag distance is greater than 20% of the screen height
				if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance && canMove)
				{//It's a drag
				 //check if the drag is vertical or horizontal
					if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
					{   //If the horizontal movement is greater than the vertical movement...
						if ((lp.x > fp.x) && !right)  //If the movement was to the right)
						{   //Right swipe
							StartCoroutine(Movement(MoveDirection.RIGHT, duration));
							//Debug.Log("Right Swipe");
						}
						else if((lp.x < fp.x) && !left)
						{   //Left swipe
							StartCoroutine(Movement(MoveDirection.LEFT, duration));
							//Debug.Log("Left Swipe");
						}
					}
					else
					{   //the vertical movement is greater than the horizontal movement
						if ((lp.y > fp.y) && !up)  //If the movement was up
						{   //Up swipe
							StartCoroutine(Movement(MoveDirection.UP, duration));
							Debug.Log("Up Swipe");
						}
						else if((lp.y < fp.y) && !down)
						{   //Down swipe
							StartCoroutine(Movement(MoveDirection.DOWN, duration));
							//Debug.Log("Down Swipe");
						}
					}
				}
				else
				{   //It's a tap as the drag distance is less than 20% of the screen height
					//Debug.Log("Tap");
				}
			}
		//}
	}
	IEnumerator Movement(MoveDirection moveDirection, float duration)
	{
		canMove = false;
		float progress = 0;

		//var endRotation = Quaternion.Euler(0, 0, 0);
		var targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);

		if (moveDirection == MoveDirection.LEFT)
		{
			//endRotation = Quaternion.Euler(0, 0, -180);
			targetPosition = new Vector3(currentPosition.x - distance, currentPosition.y, currentPosition.z);
		}
		else if (moveDirection == MoveDirection.RIGHT)
		{
			//endRotation = Quaternion.Euler(0, 0, 180);
			targetPosition = new Vector3(currentPosition.x + distance, currentPosition.y, currentPosition.z);
		}
		else if (moveDirection == MoveDirection.UP)
		{
			//endRotation = Quaternion.Euler(-180, 0, 0);
			targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + distance);
		}
		else if (moveDirection == MoveDirection.DOWN)
		{
			//endRotation = Quaternion.Euler(180, 0, 0);
			targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - distance);
		}
		var endPosition = new Vector3(targetPosition.x, currentPosition.y, targetPosition.z);

		while (progress < duration)
		{
			progress += Time.deltaTime;
			var percent = Mathf.Clamp01(progress / duration);
			float height = jumpHeight * Mathf.Sin(Mathf.PI * percent);

			transform.position = Vector3.Lerp(currentPosition, endPosition, percent) + new Vector3(0, height, 0);
			//transform.rotation = Quaternion.Lerp(currentRotation, endRotation, percent);
			yield return null;
		}
		currentPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
		if(numberOfObjects != 0 && firstValue != 0)
        {
			canMove = true;
		}
	}
	public enum MoveDirection
	{
		RIGHT,
		LEFT,
		UP,
		DOWN
	}
	public void MoveUp()
	{
		if (!up && canMove)
		{
			StartCoroutine(Movement(MoveDirection.UP, duration));
		}
	}
	public void MoveLeft()
	{
		if (!left && canMove)
		{
			StartCoroutine(Movement(MoveDirection.LEFT, duration));
		}
	}
	public void MoveDown()
	{
		if (!down && canMove)
		{
			StartCoroutine(Movement(MoveDirection.DOWN, duration));
		}
	}
	public void MoveRight()
	{
		if (!right && canMove)
		{
			StartCoroutine(Movement(MoveDirection.RIGHT, duration));
		}
	}
	void UpRayMethod()
	{
		Vector3 origin = transform.position;
		Ray UpRay = new Ray(origin, transform.forward);
		RaycastHit UpRaycastHit;
		Debug.DrawRay(origin, transform.forward * 1.25f, Color.red);
		if (Physics.Raycast(UpRay, out UpRaycastHit, 1.25f))
		{
			if (UpRaycastHit.collider.CompareTag("Wall"))
			{
				//RayDetetction.instance.up = true;
				up = true;
				
			}
			else
			{
				
			}
		}
		else
		{
			//RayDetetction.instance.up = false;
			up = false;
			
		}
	}
	void DownRayMethod()
	{
		Vector3 origin = transform.position;
		Ray DownRay = new Ray(origin, -transform.forward);
		RaycastHit DownRaycastHit;
		Debug.DrawRay(origin, -transform.forward * 1.25f, Color.red);
		if (Physics.Raycast(DownRay, out DownRaycastHit, 1.25f))
		{
			if (DownRaycastHit.collider.CompareTag("Wall"))
			{
				//RayDetetction.instance.down = true;
				down = true;
				//gameObject.tag = "Wall";
			}
			else
			{
				//RayDetetction.instance.down = false;
				down = false;
				//gameObject.tag = "Untagged";
			}
		}
		else
		{
			//RayDetetction.instance.down = false;
			down = false;
			//gameObject.tag = "Untagged";
		}
	}
	void RightRayMethod()
	{
		Vector3 origin = transform.position;
		Ray RightRay = new Ray(origin, transform.right);
		RaycastHit RightRaycastHit;
		Debug.DrawRay(origin, transform.right * 1.25f, Color.red);
		if (Physics.Raycast(RightRay, out RightRaycastHit, 1.25f))
		{
			if (RightRaycastHit.collider.CompareTag("Wall"))
			{
				//RayDetetction.instance.right = true;
				right = true;
				//gameObject.tag = "Wall";
			}
			else
			{
				//RayDetetction.instance.right = false;
				right = false;
				//gameObject.tag = "Untagged";
			}
		}
		else
		{
			//RayDetetction.instance.right = false;
			right = false;
			//gameObject.tag = "Untagged";
		}
	}
	void LeftRayMethod()
	{
		Vector3 origin = transform.position;
		Ray LeftRay = new Ray(origin, -transform.right);
		RaycastHit LeftRaycastHit;
		Debug.DrawRay(origin, -transform.right * 1.25f, Color.red);
		if (Physics.Raycast(LeftRay, out LeftRaycastHit, 1.25f))
		{
			if (LeftRaycastHit.collider.CompareTag("Wall"))
			{
				//RayDetetction.instance.left = true;
				left = true;
				//gameObject.tag = "Wall";
			}
			else
			{
				//RayDetetction.instance.left = false;
				left = false;
				//gameObject.tag = "Untagged";
			}
		}
		else
		{
			//RayDetetction.instance.left = false;
			left = false;
			//gameObject.tag = "Untagged";
		}
	}
	
	void OnTriggerEnter(Collider other)
    {
  
        if (other.gameObject.CompareTag("Subraction"))
        {
			Destroy(other.gameObject);
			
			numberOfObjects--;
			objectCollisionParticles.transform.GetComponent<ParticleSystem>().Play();
			int value;
			int.TryParse(other.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text, out value);
			firstValue -= value;
			valueText.text = firstValue.ToString();
			collisionParticles.Play();
			//transform.GetChild(7).GetComponent<ParticleSystem>().Play();
			//GetComponent<Renderer>().material.color = Color.white;
			//StartCoroutine(MaterialShining());
			if (firstValue == 0)
			{
				canMove = false;
				GameObject[] remainingObjects = GameObject.FindGameObjectsWithTag("Objects");
				foreach(GameObject obj in remainingObjects)
                {
					Destroy(obj.transform.parent.gameObject);
                }
				Camera.main.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
				Camera.main.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
				Camera.main.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
				StartCoroutine(LoadWinScreen());
			}
			if(numberOfObjects == 0 && firstValue != 0)
            {
				//Debug.Log("Lose");
				canMove = false;
				StartCoroutine(LoadLoseScreen());
            }
		}
        if (other.gameObject.CompareTag("Addition"))
        {
			Destroy(other.gameObject);
			
			numberOfObjects--;
			objectCollisionParticles.transform.GetComponent<ParticleSystem>().Play();
			int value;
			int.TryParse(other.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text, out value);
			firstValue += value;
			valueText.text = firstValue.ToString();
			collisionParticles.Play();
			//transform.GetChild(7).GetComponent<ParticleSystem>().Play();
			//GetComponent<Renderer>().material.color = Color.white;
			//StartCoroutine(MaterialShining());
			if (firstValue == 0)
			{
				canMove = false;
				GameObject[] remainingObjects = GameObject.FindGameObjectsWithTag("Objects");
				foreach (GameObject obj in remainingObjects)
				{
					Destroy(obj.transform.parent.gameObject);
				}
				Camera.main.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
				Camera.main.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
				Camera.main.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
				StartCoroutine(LoadWinScreen());
			}
			if (numberOfObjects == 0 && firstValue != 0)
			{
				//Debug.Log("Lose");
				canMove = false;
				StartCoroutine(LoadLoseScreen());
			}
		}
    }
	IEnumerator LoadWinScreen()
    {
		zeroText.enabled = false;
		win = true;
		yield return new WaitForSeconds(0.25f);
		GameManager.instance.winPanel.SetActive(true);
		
	}
	IEnumerator LoadLoseScreen()
    {
		zeroText.enabled = false;
		win = false;
		
		yield return new WaitForSeconds(0.25f);
		GameManager.instance.losePanel.SetActive(true);
		
    }
	IEnumerator MaterialShining()
    {
		
		yield return new WaitForSeconds(0.25f);
		GetComponent<Renderer>().material.color = firstColor;
    }
}
