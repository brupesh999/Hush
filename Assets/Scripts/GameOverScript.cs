using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject GameOverDisplay;
    public Player player;

    void StopGame()
    {
        Time.timeScale = 0;
        GameOverDisplay.SetActive(true);
    }

    // Update is called once per frame
    float lastHP;

    void Update()
    {
        if (player.currentHP == 0 && lastHP != 0)
        {
            StopGame();
        }
        lastHP = player.currentHP;
    }
}
