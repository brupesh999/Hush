using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Player owner;
    void Start()
    {

    }
    public void Init(Player player)
    {
        owner = player;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Break()
    {
        owner.ShieldBroken();
        Destroy(gameObject);
    }
}
