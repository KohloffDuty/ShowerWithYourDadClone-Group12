using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float moveSpeed = 5f; // Movement speed
	private Rigidbody2D rb;
	private Vector2 moveInput;

	public GameObject chocolateSon;
	//public GameObject caramelSon;
	//public GameObject vanillaSon;

	public GameObject chocolateDad;
	public GameObject caramelDad;
	public GameObject vanillaDad;

	public UIPanel score;
	public float points = 10f;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		// Get input from arrow keys
		float moveX = Input.GetAxisRaw("Horizontal"); // Left/Right
		float moveY = Input.GetAxisRaw("Vertical");   // Up/Down

		// Store input in a vector
		moveInput = new Vector2(moveX, moveY).normalized;
	}

	void FixedUpdate()
	{
		// Move character using physics
		rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
	}

	// Score update when son and dad objects collide based on their different son-dad types.
	//private void OnTriggerEnter2D(Collider2D collision) 
	//{
	//	if (collision.gameObject.CompareTag("chocolate"))
	//	{
	//		score.AddScore(points);
	//		Debug.Log("You found your dad!");
	//	}
	//}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("Here");

		if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == LayerMask.NameToLayer("Chocolate"))
		{
			score.AddScore(points);
			Debug.Log("You found your dad!");
		}

		
		//if (collision.gameObject.CompareTag("chocolate"))
		//{
		//	score.AddScore(points);
		//	Debug.Log("You found your dad!");
		//}
	}

}
