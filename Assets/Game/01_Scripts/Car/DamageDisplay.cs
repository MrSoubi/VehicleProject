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

    private void Start()
    {
        UpdateText(0);
    }

    public void UpdateText(int value)
    {
        value_1 = value % 10;
        value_10 = (value / 10) % 10;
        value_100 = (value / 100) % 10;

        image1.sprite = numericSprites[value_1];

        if (value_100 > 0)
        {
            //image100.enabled = true;
            //image10.enabled = true;

            image10.color = new Color(image10.color.r, image10.color.g, image10.color.b, 1);
            image100.color = new Color(image100.color.r, image100.color.g, image100.color.b, 1);

            image100.sprite = numericSprites[value_100];
            image10.sprite = numericSprites[value_10];
        }
        else
        {
            //image100.enabled = false;
            image100.color = new Color(image100.color.r, image100.color.g, image100.color.b, 0);
            if (value_10 > 0)
            {
                //image10.enabled = true;
                image10.color = new Color(image10.color.r, image10.color.g, image10.color.b, 1);
                image10.sprite = numericSprites[value_10];
            }
            else
            {
                //image10.enabled = false;
                image10.color = new Color(image10.color.r, image10.color.g, image10.color.b, 0);
            }
        }

        image1.transform.DOShakeRotation(0.3f, strength: 45, randomnessMode: ShakeRandomnessMode.Harmonic);
        image10.transform.DOShakeRotation(0.3f, strength: 45, randomnessMode: ShakeRandomnessMode.Harmonic);
        image100.transform.DOShakeRotation(0.3f, strength: 45, randomnessMode: ShakeRandomnessMode.Harmonic);
        percentImage.transform.DOShakeRotation(0.3f, strength: 45, randomnessMode: ShakeRandomnessMode.Harmonic);
    }
}
