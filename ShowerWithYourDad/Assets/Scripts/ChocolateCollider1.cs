using UnityEngine;
using UnityEngine.UIElements;

public class ChocolateCollider1 : MonoBehaviour
{
    public int points = 10;
    public GameObject ChocolateSon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision between chocolate dad and son
        if (ChocolateSon.CompareTag("chocolate"))
        {
            if (collision.gameObject.CompareTag("Enemy") &&
                collision.gameObject.layer == LayerMask.NameToLayer("Chocolate"))
            {
                UIPanel.Instance1.AddScore(points);
                WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.Coin);
                Debug.Log("You found your dad!");
                Player.instance2.EndRoundAndContinue();


            }
        }
        else if (collision.CompareTag("Obstacle"))
        {
            // Hit an obstacle
            // WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.ObstacleHit); // different sound
            Debug.Log("Hit an obstacle!");
        }
        else
        {
            // Any other collision
            WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.WrongHit); // optional generic sound
            Debug.Log($"Hit something else: {collision.tag}");
            Player.instance2.EndRoundAndContinue();
        }
    }
}


