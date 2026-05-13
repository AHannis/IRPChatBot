using UnityEngine;

public class EmojiParser : MonoBehaviour
{
    public string ParseEmojiText(
        string text
    )
    {
        text =
            text.Replace(
                ":smile:",
                Emoji("smile")
            );

        text =
            text.Replace(
                ":thumbsup:",
                Emoji("thumbsup")
            );

        text =
            text.Replace(
                ":laugh:",
                Emoji("laugh")
            );

        text =
            text.Replace(
                ":cry:",
                Emoji("cry")
            );

        text =
            text.Replace(
                ":awkward:",
                Emoji("awkward")
            );

        text =
            text.Replace(
                ":facepalm:",
                Emoji("facepalm")
            );

        return text;
    }

    string Emoji(
        string emojiName
    )
    {
        switch (emojiName)
        {
            case "smile":
                return "\U0001F642";

            case "thumbsup":
                return "\U0001F44D";

            case "laugh":
                return "\U0001F602";

            case "cry":
                return "\U0001F62D";

            case "awkward":
                return "\U0001F605";

            case "facepalm":
                return "\U0001F926";

            case "thinking":
                return "\U0001F914";
        }

        return "";
    }
}