using UnityEngine;

public class VanillaCollider : MonoBehaviour
{
    public int points = 10;
    public GameObject VanillaDad;

    private bool roundEnded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (roundEnded || GameManager.Instance == null) return;

        if (collision.CompareTag("Obstacle"))
        {
            roundEnded = true;
            GameManager.Instance.HandleObstacleCollision("Hit an obstacle!");
            return;
        }

        if (collision.gameObject == VanillaDad)
        {
            roundEnded = true;
            GameManager.Instance.HandleCollisionSuccess(points, "Found correct vanilla dad!");
        }
        else if (collision.CompareTag("EnemyW") || collision.CompareTag("Vanilla"))
        {
            roundEnded = true;
            GameManager.Instance.HandleCollisionSuccess(points, "Found vanilla dad!");
        }
        else
        {
            roundEnded = true;
            GameManager.Instance.HandleCollisionFailure("Unexpected vanilla collision!");
        }
    }

    public void ResetRoundFlag()
    {
        roundEnded = false;
    }
}