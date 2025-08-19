using UnityEngine;

public class ChocolateCollider1 : MonoBehaviour
{
    public int points = 10;
    public GameObject ChocolateSon;

    private bool roundEnded = false; // lock to prevent multiple triggers

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (roundEnded) return; // stop if round already processed

        //  Collision between Chocolate son and dad
        if (ChocolateSon != null && ChocolateSon.CompareTag("chocolate"))
        {
            if (collision.CompareTag("Enemy") &&
                collision.gameObject.layer == LayerMask.NameToLayer("Chocolate"))
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
            Player.instance2.EndRoundAndContinue(); // optional: end round on obstacle
        }
        //  Hit something else
        else
        {
            roundEnded = true;
            WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.WrongHit);
            Debug.Log($"Hit something else: {collision.tag}");
            Player.instance2.EndRoundAndContinue();
        }
    }

    // Reset lock for new round
    public void ResetRoundFlag()
    {
        roundEnded = false;
    }

}
