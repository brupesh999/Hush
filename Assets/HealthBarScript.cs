using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarScript : MonoBehaviour
{
    public BasicEnemy enemy;

    [SerializeField]
    private Slider slider;

    // enemies only
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemy.currentHP / enemy.maxHP;
    }
}
