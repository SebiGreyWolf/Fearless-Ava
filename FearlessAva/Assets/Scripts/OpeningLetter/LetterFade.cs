using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LetterFade : MonoBehaviour
{
    public GameObject LetterUI;
    public GameObject BackstoryGameObject;
    public Image letter;
    public Text skipText;
    public Text backstoryTextField;

    [TextArea(3, 10)]
    public string backstory;

    public float fadeDuration;
    public float fadeStep;
    private bool isLetterDone;

    void Start()
    {
        skipText.color = new Color(skipText.color.r, skipText.color.g, skipText.color.b, 0);
        StartCoroutine(LetterFadeIn());
    }

    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) && !isLetterDone)
        {
            LetterUI.SetActive(false);
            isLetterDone = true;
            skipText.color = new Color(skipText.color.r, skipText.color.g, skipText.color.b, 0);

            BackstoryGameObject.SetActive(true);
            StartCoroutine(TypeSentence(backstory));
        }
        else if (Input.GetKeyUp(KeyCode.Space) && isLetterDone)
        {
            SceneManager.LoadScene("Level1");
        }
    }

    IEnumerator LetterFadeIn()
    {
        Color originalColor = letter.color;

        float alphaValue = originalColor.a;

        for (float t = 0; t < fadeDuration; t += fadeStep)
        {
            alphaValue = Mathf.Lerp(0, originalColor.a, t / fadeDuration);
            letter.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaValue);
            yield return new WaitForSeconds(fadeStep);
        }

        letter.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);

        StartCoroutine(TextFadeIn());
    }

    IEnumerator TextFadeIn()
    {
        Color originalColor = new Color(skipText.color.r, skipText.color.g, skipText.color.b, 1);

        float alphaValue = originalColor.a;

        for (float t = 0; t < fadeDuration; t += fadeStep)
        {
            alphaValue = Mathf.Lerp(0, originalColor.a, t / fadeDuration);
            skipText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaValue);
            yield return new WaitForSeconds(fadeStep);
        }

        skipText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    IEnumerator TypeSentence(string sentence)
    {
        backstoryTextField.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            backstoryTextField.text += letter;
            yield return new WaitForSeconds(0.03f);
        }

        StartCoroutine(TextFadeIn());
    }
}
