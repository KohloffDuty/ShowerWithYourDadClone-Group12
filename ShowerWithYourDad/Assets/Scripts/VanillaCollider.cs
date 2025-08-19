using UnityEngine;

public class VanillaCollider : MonoBehaviour
{
    public int points = 10;
    public GameObject VanillaDad; // Reference to the correct dad

    private bool roundEnded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (roundEnded) return;

        Debug.Log($"{gameObject.name} (son) collided with {collision.gameObject.name}");

        // Handle obstacles first
        if (collision.CompareTag("Obstacle"))
        {
            roundEnded = true;
            Debug.Log("Hit an obstacle!");
            Player.instance2.EndRoundAndContinue();
            return;
        }

        // Check if this is the correct dad
        if (collision.gameObject == VanillaDad)
        {
            HandleSuccess("Found correct dad!");
        }
        // Check if this is a wrong dad (another vanilla)
        else if (collision.CompareTag("EnemyW") || collision.CompareTag("Vanilla"))
        {
            HandleSuccess("Found correct dad!");
        }
        // Anything else
        else
        {
            HandleWrongCollision("Unexpected collision!");
        }
    }

    private void HandleSuccess(string message)
    {
        roundEnded = true;
        UIPanel.Instance1.AddScore(points);
        if (WaveSpawner.Instance != null)
        {
            WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.Coin);
        }
        Debug.Log(message);
        Player.instance2.EndRoundAndContinue();
    }

    private void HandleWrongCollision(string message)
    {
        roundEnded = true;
        if (WaveSpawner.Instance != null)
        {
            WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.WrongHit);
        }
        Debug.Log(message);
        Player.instance2.EndRoundAndContinue();
    }

    public void ResetRoundFlag()
    {
        roundEnded = false;
    }
}