using UnityEngine;

public class CaramelCollider : MonoBehaviour
{
    public GameObject CaramelSon;
    public int points = 10;

    private bool roundEnded = false; // lock to stop multiple triggers

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (roundEnded) return; // prevent multiple triggers

        // Collision between Caramel son and dad
        if (CaramelSon != null && CaramelSon.CompareTag("Caramel"))
        {
            if (collision.CompareTag("EnemyC") &&
                collision.gameObject.layer == LayerMask.NameToLayer("Caramel"))
            {
                roundEnded = true;
                UIPanel.Instance1.AddScore(points);
                WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.Coin);
                Debug.Log("You found your dad!");
                Player.instance2.EndRoundAndContinue();
            }
        }
        // Hit an obstacle
        else if (collision.CompareTag("Obstacle"))
        {
            roundEnded = true;
            // WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.ObstacleHit);
            Debug.Log("Hit an obstacle!");
           // UIPanel.Instance1.EndRoundAndContinue(); // optional: if obstacle ends round
        }
        //  Hit something else
        else
        {
            roundEnded = true;
            WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.WrongHit);
            Player.instance2.EndRoundAndContinue();
            Debug.Log($"Hit something else: {collision.tag}");
        }
    }

    //  Reset flag when new round starts
    public void ResetRoundFlag()
    {
        roundEnded = false;
    }
}
