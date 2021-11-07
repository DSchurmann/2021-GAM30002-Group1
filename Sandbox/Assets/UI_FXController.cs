using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FXController : MonoBehaviour
{
    public Image Fader;


    // Start is called before the first frame update
    void Start()
    {
        Fader = transform.Find("Fader").GetComponent<Image>();
        Fader.gameObject.SetActive(false);

        // fade out
        StartCoroutine(FadeBlack(0.5f, 0, 1));
        // fade in
        //FadeInBlack(0.5f, 1, 1);
        //StartCoroutine(FadeBlack(0f, 1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator FadeBlack(float delay, int value, float time)
    {
        Fader.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay);

        Color col = Color.black;

        if (value == 0)
        {
            col.a = 1;
        }
        else if (value == 1)
        {
            col.a = 0;
        }

        Fader.GetComponent<Image>().color = col;

        Fader.CrossFadeAlpha(value, time, false);
    }

    public IEnumerator FadeInBlack(float delay, int value, float time)
    {
        Fader.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay);

        Color col = Color.black;
        col.a = 1;

        Fader.GetComponent<Image>().color = col;

        Fader.CrossFadeAlpha(0f , 0f, false);
        Fader.CrossFadeAlpha(value, time, false);
    }

    public IEnumerator FadeInWhite(float delay, int value, float time)
    {
        Fader.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay);

        Color col = Color.white;
        col.a = 1;

        Fader.GetComponent<Image>().color = col;

        Fader.CrossFadeAlpha(0f, 0f, false);
        Fader.CrossFadeAlpha(value, time, false);
    }

}
