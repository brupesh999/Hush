using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    float timekeeper = 0f;//this will count how much time has passed (in s) since enemy has shot a projectile
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootInterval;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timekeeper += Time.deltaTime;//increase timer every update
        
        if (timekeeper >= shootInterval){
            timekeeper = 0f;
            Instantiate(projectile, transform.position + new Vector3(-0.5f, 0, 0), Quaternion.identity);
        }
    }
}
