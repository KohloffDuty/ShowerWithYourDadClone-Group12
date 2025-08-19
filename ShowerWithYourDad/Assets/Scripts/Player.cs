
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public float moveSpeed = 5f; // Movement speed
	private Rigidbody2D rb;
	private Vector2 moveInput;

	public GameObject chocolateSon;
	public GameObject caramelSon;
	public GameObject vanillaSon;

	private SpriteRenderer sr;
	private Color originalColor;

	//public SpriteRenderer chocSon;
	//public SpriteRenderer caraSon;
	//public SpriteRenderer vaniSon;

	private float originalSpee;

	//public UIPanel score;
	public float points = 10f;
	public static Player instance2;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		originalColor = sr.color;
	}
	private void Awake()
	{
		if (instance2 == null)
		{
			instance2 = this;
		}
		Time.timeScale = 1.0f;
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
		//if (chocolateSon.CompareTag("chocolate"))
		//{
		//	if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == LayerMask.NameToLayer("Chocolate"))
		//	{
		//		UIPanel.Instance1.AddScore(points);
		//		Debug.Log("You found your dad!");
		//		WaveSpawner.Instance.EndRoundAndContinue();
		//	}
		//}



		if (collision.gameObject.CompareTag("Obstacle") && collision.gameObject.layer == LayerMask.NameToLayer("Puddle"))
		{
			StartCoroutine(SlowDown());
		}


		if (collision.gameObject.CompareTag("Obstacle") && collision.gameObject.layer == LayerMask.NameToLayer("Sign"))
		{
			StartCoroutine(Stop());
			StartCoroutine(FlashRedCoroutine());
		}
	}

	private System.Collections.IEnumerator SlowDown()
	{
		moveSpeed = 2f; // Apply slow
		yield return new WaitForSeconds(1f);
		moveSpeed = 5f; // Restore normal speed
	}

	private System.Collections.IEnumerator Stop()
	{
		moveSpeed = 0f; // Apply slow
		yield return new WaitForSeconds(0.75f);
		moveSpeed = 5f; // Restore normal speed
	}

	public void EndRoundAndContinue()
	{
		// Destroy leftover enemies/sons
		WaveSpawner.Instance.DestroyPreviousEnemies();

		// Reset round state
		WaveSpawner.Instance.ResetRound();

		// Start the next round after 2 seconds
		StartCoroutine(StartNextRound());
	}

	public System.Collections.IEnumerator StartNextRound()
	{
		yield return new WaitForSeconds(2f); // Delay before next wave
		StartCoroutine(WaveSpawner.Instance.StartWaves());
	}

	
	private IEnumerator FlashRedCoroutine()
	{
		float duration = 2f; // flash time
		float elapsed = 0f;

		while (elapsed < duration)
		{
			// Alternate between red and original
			sr.color = Color.red;
			yield return new WaitForSeconds(0.1f);

			sr.color = originalColor;
			yield return new WaitForSeconds(0.1f);

			elapsed += 0.2f;
		}

		// Ensure it ends on original color
		sr.color = originalColor;
	}
}
