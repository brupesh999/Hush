using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Player owner;
    void Start()
    {
        Debug.Log("Shield spawned! Collider enabled: " + GetComponent<Collider2D>().enabled);
    }
    public void Init(Player player)
    {
        owner = player;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Shield collider touched by: " + other.name);
    }

    public void AbsorbDamage(int damage)
    {
        int reduced = damage / 2;
        Debug.Log("Shield absorbed hit");
        owner.ApplyDamage(reduced);
        Break();
    }
    public void Break()
    {
        owner.ShieldBroken();
        Destroy(gameObject);
    }
}
