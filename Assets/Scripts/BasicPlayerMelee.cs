using UnityEngine;

public class BasicPlayerMelee : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.15f; // how long the hitbox lasts
    [SerializeField] private float offset = 0.5f;    // distance from player
    [SerializeField] private float damage = 1f;
    private Transform player;
    void Start()
    {
        Destroy(gameObject, lifetime); // disappear after short time
    }
    public void Init(Transform playerTransform)
    {
        player = playerTransform;
        transform.position = player.position + new Vector3(offset, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
