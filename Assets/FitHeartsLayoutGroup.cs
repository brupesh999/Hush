using UnityEngine;
using UnityEngine.UI;

public class FitHeartsLayoutGroup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    RectTransform uiRecTransform;
    HorizontalLayoutGroup layout;

    void Start()
    {
        uiRecTransform = GetComponent<RectTransform>();
        layout = GetComponent<HorizontalLayoutGroup>();
    }

    // Update is called once per frame -> should probably change so it only changes given an update to hearts
    public void UpdateHearts()
    {
        int childCount = uiRecTransform.childCount;
        float widthPerElt =
            (uiRecTransform.rect.width - layout.spacing * (childCount - 1)) / childCount; // need to have padding too
        float heightPerElt = uiRecTransform.rect.height;
        float heartSize = Mathf.Min(widthPerElt, heightPerElt);
        foreach (RectTransform heart in transform)
        {
            heart.sizeDelta = new Vector2(heartSize, heartSize);
        }
    }
}
