using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypingDots : MonoBehaviour
{
    public GameObject typingUI;

    public Image dot1;

    public Image dot2;

    public Image dot3;

    string lastCorrection = "";

    public IEnumerator AnimateDots(
        int loops
    )
    {
        typingUI.SetActive(true);

        for (
            int i = 0;
            i < loops;
            i++
        )
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

    public IEnumerator PlayTyping(
        int loops
    )
    {
        yield return StartCoroutine(
            AnimateDots(
                loops
            )
        );
    }

    public IEnumerator PlayTyping()
    {
        yield return StartCoroutine(
            AnimateDots(
                2
            )
        );
    }

    public int CalculateTypingLoops(
        string reply
    )
    {
        int length =
            reply.Length;

        if (
            length < 25
        )
        {
            return 1;
        }

        if (
            length < 70
        )
        {
            return 2;
        }

        return 3;
    }

    public float CalculateThinkTime(
        string input
    )
    {
        float time =
            0.4f
            + (
                input.Length
                * 0.015f
            );

        return Mathf.Clamp(
            time,
            0.4f,
            2.5f
        );
    }

    public string AddOccasionalTypo(
        string text
    )
    {
        lastCorrection = "";

        if (
            Random.value > 0.08f
        )
        {
            return text;
        }

        if (
            text.Contains("what's")
        )
        {
            text =
                text.Replace(
                    "what's",
                    "thats"
                );

            lastCorrection =
                "what's*";
        }
        else if (
            text.Contains("whatever")
        )
        {
            text =
                text.Replace(
                    "whatever",
                    "what'ev"
                );

            lastCorrection =
                "whatever*";
        }
        else if (
            text.Contains("you're")
        )
        {
            text =
                text.Replace(
                    "you're",
                    "your"
                );

            lastCorrection =
                "you're*";
        }
        else if (
            text.Contains("that's")
        )
        {
            text =
                text.Replace(
                    "that's",
                    "thats"
                );

            lastCorrection =
                "that's*";
        }
        else if (
            text.Contains("been")
        )
        {
            text =
                text.Replace(
                    "been",
                    "beeen"
                );

            lastCorrection =
                "been*";
        }

        return text;
    }

    public bool HasCorrection()
    {
        return
            !string.IsNullOrEmpty(
                lastCorrection
            );
    }

    public string GetCorrection()
    {
        return lastCorrection;
    }
}