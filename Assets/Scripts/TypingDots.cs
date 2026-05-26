using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypingDots : MonoBehaviour
{
    //main container showing bot is 'thinking'
    public GameObject typingUI;

    public AudioSource typingAudio;

    public AudioClip typingLoop;

    public Image dot1;

    public Image dot2;

    public Image dot3;

    string lastCorrection = "";

 
 #region Animated Typing Loop
    public IEnumerator AnimateDots(
        int loops
    )
    {
        typingUI.SetActive(
            true
        );

        if (
            typingAudio != null
            && typingLoop != null
        )
        {
            typingAudio.clip =
                typingLoop;

            typingAudio.loop = true;

            typingAudio.Play();
        }

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
                Random.Range(
                    0.18f,
                    0.42f
                )
            );

            dot1.enabled = true;
            dot2.enabled = true;
            dot3.enabled = false;

            yield return new WaitForSeconds(
                Random.Range(
                    0.18f,
                    0.42f
                )
            );

            dot1.enabled = true;
            dot2.enabled = true;
            dot3.enabled = true;

            yield return new WaitForSeconds(
                Random.Range(
                    0.18f,
                    0.42f
                )
            );

            dot1.enabled = false;
            dot2.enabled = false;
            dot3.enabled = false;
        }
    }
    #endregion

 #region Coroutine To Trigger Animation
    public IEnumerator PlayTyping(
        int loops
    )
    {
        yield return StartCoroutine(
            AnimateDots(
                loops
            )
        );

        typingUI.SetActive(
            false
        );

        if (
            typingAudio != null
        )
        {
            typingAudio.Stop();
        }

        dot1.enabled = false;
        dot2.enabled = false;
        dot3.enabled = false;
    }

    public IEnumerator PlayTyping()
    {
        yield return StartCoroutine(
            PlayTyping(
                2
            )
        );
    }
    #endregion


 #region Determines How Long Animation Should Play
    //adding more realistic feel than instant reply and calculates length of reply to message animate ratio
    public int CalculateTypingLoops(
        string reply
    )
    {
        int length =
            reply.Length;

        int loops = 1;

        if (
            length > 35
        )
        {
            loops++;
        }

        if (
            length > 90
        )
        {
            loops++;
        }

        if (
            reply.Contains("?")
        )
        {
            loops++;
        }

        if (
            reply.Contains("...")
        )
        {
            loops++;
        }

        return Mathf.Clamp(
            loops,
            1,
            5
        );
    }

    //simulates deep thinking before bot replies
    public float CalculateThinkTime(
        string input
    )
    {
        float time =
            Random.Range(
                1.2f,
                2.8f
            );

        time +=
            input.Length * 0.025f;

        if (
            input.Contains("?")
        )
        {
            time +=
                Random.Range(
                    0.6f,
                    1.5f
                );
        }

        if (
            input.Length > 80
        )
        {
            time +=
                Random.Range(
                    1f,
                    2f
                );
        }

        return Mathf.Clamp(
            time,
            1.5f,
            8f
        );
    }
    #endregion
   
 #region HasCorrection & GetCorrection
    public string AddOccasionalTypo(
        string text
    )
    {
        //stores typo corrections that may appear after message is sent
        lastCorrection = "";

        if (
            text.Contains("take care")
            || text.Contains("you alright")
            || text.Contains("i'm listening")
            || text.Contains("here for you")
        )
        {
            return text;
        }

        if (
            Random.value > 0.05f
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

        if (
            Random.value < 0.35f
        )
        {
            lastCorrection = "";
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
#endregion