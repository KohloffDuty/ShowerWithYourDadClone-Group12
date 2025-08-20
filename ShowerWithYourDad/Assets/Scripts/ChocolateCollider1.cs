using UnityEngine;

public class ChocolateCollider1 : MonoBehaviour
{
    public int points = 10;
    public GameObject ChocolateDad;

    private bool roundEnded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (roundEnded) return;

        if (collision.CompareTag("Obstacle"))
        {
            roundEnded = true;
            Debug.Log("Hit an obstacle!");
            GameManager.Instance.EndRound();
            return;
        }

        if (collision.gameObject == ChocolateDad)
        {
            HandleSuccess("Found correct dad!");
        }
        else if (collision.CompareTag("Enemy") || collision.CompareTag("chocolate"))
        {
            HandleSuccess("Found correct dad!");
        }
        else
        {
            HandleWrongCollision("Unexpected collision!");
        }
    }

    private void HandleSuccess(string message)
    {
        roundEnded = true;
        GameManager.Instance.AddScore1(points);
        GameManager.Instance.PlaySound(GameManager.Instance.coinSound);
        Debug.Log(message);
        GameManager.Instance.EndRound();
    }

    private void HandleWrongCollision(string message)
    {
        roundEnded = true;
        GameManager.Instance.PlaySound(WaveSpawner.Instance.WrongHit);
        Debug.Log(message);
        GameManager.Instance.EndRound();
    }

    public void ResetRoundFlag()
    {
        roundEnded = false;
    }
}