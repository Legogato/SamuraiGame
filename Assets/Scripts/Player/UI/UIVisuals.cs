using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVisuals : MonoBehaviour
{
    [SerializeField]
    PlayerController playerControllerScript;

    private const float maxTimerDamage = 1f;

    public Image healthBarImage;
    public Image postureBarImage;
    public Image healthBarDamagedImage;
    public GameObject lifePrefab;
    public GameObject lifeTransform;
    public GameObject playerCanvas;

    List<GameObject> lifeList = new List<GameObject>();

    private Vector2 pos;

    private float damageTimer;

 
    private void Start()
    {
        SetHealth(playerControllerScript.statusScript.maxHeatlh);
        SetPosture(0);
        pos = lifeTransform.GetComponent<RectTransform>().anchoredPosition;
        SetLifes(playerControllerScript.lives);


    }
    private void Update()
    {
        healthBarImage.fillAmount = playerControllerScript.currentHealth/100;
        SetPosture(playerControllerScript.currentPosture / 100);

        damageTimer -= Time.deltaTime;
        if (damageTimer <= 0f)
        {
            if (healthBarImage.fillAmount < healthBarDamagedImage.fillAmount)
            {
                float shortenAmount = 0.5f * Time.deltaTime;

                healthBarDamagedImage.fillAmount = healthBarDamagedImage.fillAmount - shortenAmount;
            }
        }
    }

    public void DamageHealth(float damage)
    {
        SetHealth(healthBarImage.fillAmount -= damage);
    }

    public void DamagePosture(float damage) 
    {
        SetPosture(postureBarImage.fillAmount += damage);
    }

    public void RemoveLives(int pos)
    {
        if (pos <= -1)
        {
            return;
        }
        else
        {
            lifeList[pos].GetComponent<Image>().color = Color.black;
        }

    }

    public void SetHealth(float healthNormalize) 
    {
        healthBarImage.fillAmount = healthNormalize;

        if(healthBarDamagedImage.fillAmount > healthBarImage.fillAmount)
        {

            damageTimer = maxTimerDamage;
        }
        else
        {
            healthBarDamagedImage.fillAmount = healthBarImage.fillAmount;
        }
    }

    public void SetPosture(float postureNormalize)
    { 
        postureBarImage.fillAmount = postureNormalize;
        Color postureBarColor = new Color(1, 1 - postureNormalize * 1f, 0);
        postureBarImage.color = postureBarColor;
    }

    public void SetLifes(int lifes)
    {
        for (int i = 0; i < lifes; i++)
        {

            GameObject life = (GameObject)Instantiate(lifePrefab);
            lifeList.Add(life);


            life.transform.SetParent(playerCanvas.transform, true);
            int posNormalize = 0;

            if (lifeTransform.GetComponent<RectTransform>().anchoredPosition.x < 0)
            {
                posNormalize = -1;
            }
            else
            {
                posNormalize = 1;
            }

            life.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x + 30f * posNormalize, pos.y);
            pos = life.GetComponent<RectTransform>().anchoredPosition;

        }
        
    }


}
