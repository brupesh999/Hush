using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject heartPrefab;
    private List<GameObject> hearts = new List<GameObject>();
    public float healthPerHeart = 1f; // this doesn't really work well if maxHealth is not divisible by healthPerHeart
    public Sprite fullHeart;
    private HorizontalLayoutGroup layoutGroup;

    public Sprite emptyHeart;
    public Sprite halfFullHeart;
    public Player player;
    public FitHeartsLayoutGroup layoutGroupController;
    private float maxHealth;
    private float curHealth = 0;

    void Start()
    {
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
        layoutGroupController = GetComponent<FitHeartsLayoutGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        curHealth = player.currentHP;
        maxHealth = player.maxHP;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        while (hearts.Count * healthPerHeart < maxHealth)
        {
            GameObject heart = Instantiate(heartPrefab);
            heart.transform.SetParent(layoutGroup.transform, false);
            hearts.Add(heart);
            layoutGroupController.UpdateHearts(); // Can't actively change maxhealth or it will break TODO: FIX
        }
        for (int i = 0; i < hearts.Count; i++)
        {
            if (curHealth > (i * healthPerHeart) + healthPerHeart / 2)
            {
                hearts[i].GetComponent<Image>().sprite = fullHeart;
            }
            else if (curHealth > i * healthPerHeart)
            {
                hearts[i].GetComponent<Image>().sprite = halfFullHeart;
            }
            else
            {
                hearts[i].GetComponent<Image>().sprite = emptyHeart;
            }
        }
    }
}
