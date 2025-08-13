using UnityEngine;

public class EnemyClickHandler : MonoBehaviour
{
 
    private void OnMouseDown()
    {
        if (CompareTag("Enemy"))
        { 
            Destroy(gameObject);    
            WaveSpawner.Instance.OnDadClicked();
        }
    }
}

