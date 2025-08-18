using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float moveSpeed = 5f; // Movement speed
	private Rigidbody2D rb;
	private Vector2 moveInput;

	public GameObject chocolateSon;
	public GameObject caramelSon;
	public GameObject vanillaSon;

	//public SpriteRenderer chocSon;
	//public SpriteRenderer caraSon;
	//public SpriteRenderer vaniSon;
		
	private float originalSpee;

	public UIPanel score;
	public float points = 10f;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		// Input from arrow keys
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


	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Collision between chocolate dad and son
		if (chocolateSon.CompareTag("Chocolate"))
		{
			if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == LayerMask.NameToLayer("Chocolate"))
			{
				score.AddScore(points);
				Debug.Log("You found your dad!");
			}
		}

		//Collision between caramel dad and son
		//if (caramelSon.CompareTag("Caramel"))
		//{
		//	if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == LayerMask.NameToLayer("Caramel"))
		//	{
		//		score.AddScore(points);
		//		Debug.Log("You found your dad!");
		//	}
		//}

		////Collision between vanilla dad and son
		//if (vanillaSon.CompareTag("Vanilla"))
		//{
		//	if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == LayerMask.NameToLayer("Vanilla"))
		//	{
		//		score.AddScore(points);
		//		Debug.Log("You found your dad!");
		//	}
		//}


		if (collision.gameObject.CompareTag("Obstacle"))
		{
			StartCoroutine(SlowDown());
		}
	}

	private System.Collections.IEnumerator SlowDown()
	{
		moveSpeed = 2f; // Apply slow
		yield return new WaitForSeconds(2f);
		moveSpeed = 5f; // Restore normal speed
	}
}
