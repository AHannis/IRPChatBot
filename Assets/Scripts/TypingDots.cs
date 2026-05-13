using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypingDots : MonoBehaviour
{
    public GameObject typingUI;

    public Image dot1;

    public Image dot2;

    public Image dot3;

    void Awake()
    {
        if (typingUI != null)
        {
            typingUI.SetActive(false);
        }
    }

    public IEnumerator AnimateDots(
        int loops
    )
    {
        if (typingUI == null)
        {
            yield break;
        }

        typingUI.SetActive(true);

        for (int i = 0; i < loops; i++)
        {
            dot1.enabled = true;
            dot2.enabled = false;
            dot3.enabled = false;

            yield return new WaitForSeconds(
                0.3f
            );

            dot1.enabled = true;
            dot2.enabled = true;
            dot3.enabled = false;

            yield return new WaitForSeconds(
                0.3f
            );

            dot1.enabled = true;
            dot2.enabled = true;
            dot3.enabled = true;

            yield return new WaitForSeconds(
                0.3f
            );

            dot1.enabled = false;
            dot2.enabled = false;
            dot3.enabled = false;
        }

        typingUI.SetActive(false);
    }

    public float CalculateThinkTime(
        string input
    )
    {
        float thinkTime =
            Random.Range(0.3f, 0.8f);

        if (input.Length > 80)
        {
            thinkTime +=
                Random.Range(0.6f, 1.4f);
        }

        string lower =
            input.ToLower();

        if (
            lower.Contains("sad")
            || lower.Contains("upset")
            || lower.Contains("stressed")
            || lower.Contains("crying")
            || lower.Contains("depressed")
            || lower.Contains("lonely")
            || lower.Contains("love")
            || lower.Contains("relationship")
        )
        {
            thinkTime +=
                Random.Range(0.8f, 1.8f);
        }

        return thinkTime;
    }

    public int CalculateTypingLoops(
        string reply
    )
    {
        int loops =
            Mathf.Clamp(
                Mathf.RoundToInt(
                    reply.Length / 18f
                )
                + Random.Range(1, 4),
                2,
                12
            );

        if (reply.Contains("?"))
        {
            loops -= 1;
        }

        if (reply.Length > 120)
        {
            loops += 2;
        }

        return loops;
    }

    public string AddOccasionalTypo(
        string text
    )
    {
        if (Random.value > 0.12f)
        {
            return text;
        }

        string[] typoWords =
        {
            "probably",
            "definitely",
            "actually",
            "because",
            "weird",
            "people",
            "something",
            "typing",
            "conversation"
        };

        string[] typoVersions =
        {
            "probabaly",
            "definitley",
            "actaully",
            "becuase",
            "wierd",
            "poeple",
            "somthing",
            "typign",
            "converastion"
        };

        for (int i = 0; i < typoWords.Length; i++)
        {
            if (
                text.ToLower().Contains(
                    typoWords[i]
                )
            )
            {
                text =
                    System.Text.RegularExpressions.Regex.Replace(
                        text,
                        typoWords[i],
                        typoVersions[i],
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase
                    );

                return text;
            }
        }

        if (
            text.Length > 12
            && Random.value < 0.35f
        )
        {
            int randomIndex =
                Random.Range(
                    4,
                    text.Length - 3
                );

            char[] chars =
                text.ToCharArray();

            char temp =
                chars[randomIndex];

            chars[randomIndex] =
                chars[randomIndex + 1];

            chars[randomIndex + 1] =
                temp;

            text =
                new string(chars);
        }

        return text;
    }
}