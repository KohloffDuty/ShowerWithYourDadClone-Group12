using UnityEngine;

public class CaramelCollider : MonoBehaviour
{
    public int points = 10;
    public GameObject CaramelDad;

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

        if (collision.gameObject == CaramelDad)
        {
            roundEnded = true;
            GameManager.Instance.HandleCollisionSuccess(points, "Found correct caramel dad!");
        }
        else if (collision.CompareTag("EnemyC") || collision.CompareTag("Caramel"))
        {
            roundEnded = true;
            GameManager.Instance.HandleCollisionSuccess(points, "Found caramel dad!");
        }
        else
        {
            roundEnded = true;
            GameManager.Instance.HandleCollisionFailure("Unexpected caramel collision!");
        }
    }

    public void ResetRoundFlag()
    {
        roundEnded = false;
    }
}