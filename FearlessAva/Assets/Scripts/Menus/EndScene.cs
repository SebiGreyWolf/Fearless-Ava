using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    public GameObject imageBox;
    public Image image;

    public float fadeDuration;
    public float fadeStep;

    public GameObject textbox;
    public Text text;
    public Text skipText;

    private bool imageIsShown;

    [TextArea(3, 10)]
    public string story;

    private void Start()
    {
        StartCoroutine(TypeText(story));
    }

    void Update()
    {
        if ( Input.GetKeyUp(KeyCode.Space) && !imageIsShown)
        {
            textbox.SetActive(false);
            imageIsShown = true;
            imageBox.SetActive(true);
            StartCoroutine(ImageFadeIn());
        }
        else if(Input.GetKeyUp(KeyCode.Space) && imageIsShown)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator ImageFadeIn()
    {
        Color originalColor = image.color;

        float alphaValue = originalColor.a;

        for (float t = 0; t < fadeDuration; t += fadeStep)
        {
            alphaValue = Mathf.Lerp(0, originalColor.a, t / fadeDuration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaValue);
            yield return new WaitForSeconds(fadeStep);
        }

        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    IEnumerator TypeText(string sentence)
    {
        text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
