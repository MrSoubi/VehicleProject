using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using DG.Tweening;

public class DamageDisplay : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<int, Sprite> numericSprites;
    [SerializeField] Image percentImage, image1, image10, image100;

    int value_1, value_10, value_100;
    int localValue = 0;
    public bool debugMode = false;
    private void Update()
    {
        if (debugMode && Input.GetMouseButtonDown(0))
        {
            localValue++;
            UpdateText(localValue);
        }
    }

    public void UpdateText(int value)
    {
        value_1 = value % 10;
        value_10 = (value / 10) % 10;
        value_100 = (value / 100) % 10;

        image1.sprite = numericSprites[value_1];

        if (value_100 > 0)
        {
            image100.enabled = true;
            image10.enabled = true;
            image100.sprite = numericSprites[value_100];
            image10.sprite = numericSprites[value_10];
        }
        else
        {
            image100.enabled = false;
            if (value_10 > 0)
            {
                image10.enabled = true;
                image10.sprite = numericSprites[value_10];
            }
            else
            {
                image10.enabled = false;
            }
        }

        image1.transform.DOShakeRotation(0.3f, strength: 45, randomnessMode: ShakeRandomnessMode.Harmonic);
        image10.transform.DOShakeRotation(0.3f, strength: 45, randomnessMode: ShakeRandomnessMode.Harmonic);
        image100.transform.DOShakeRotation(0.3f, strength: 45, randomnessMode: ShakeRandomnessMode.Harmonic);
        percentImage.transform.DOShakeRotation(0.3f, strength: 45, randomnessMode: ShakeRandomnessMode.Harmonic);
    }
}
