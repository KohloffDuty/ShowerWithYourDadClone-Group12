using UnityEngine;

public class ChocolateCollider1 : MonoBehaviour
{
    public int points = 10;
    public GameObject ChocolateDad; // Reference to the correct dad

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
        if (collision.gameObject == ChocolateDad)
        {
            HandleSuccess("Found correct dad!");
        }
        // Check if this is a wrong dad (another chocolate)
        else if (collision.CompareTag("Enemy") || collision.CompareTag("chocolate"))
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