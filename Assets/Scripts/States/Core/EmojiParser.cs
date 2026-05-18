using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class EmojiParser : MonoBehaviour
{
    //stores all supported emoji tags
    Dictionary<string, string> emojiMap =
        new Dictionary<string, string>();

    void Awake()
    {
        //maps text tags to unicode emojis
        //small expressive behaviours help reinforce personality illusion
        emojiMap.Add(
            "smile",
            "\U0001F642"
        );

        emojiMap.Add(
            "thumbsup",
            "\U0001F44D"
        );

        emojiMap.Add(
            "laugh",
            "\U0001F602"
        );

        emojiMap.Add(
            "cry",
            "\U0001F62D"
        );

        emojiMap.Add(
            "awkward",
            "\U0001F605"
        );

        emojiMap.Add(
            "facepalm",
            "\U0001F926"
        );

        emojiMap.Add(
            "thinking",
            "\U0001F914"
        );

        //extra aliases for more natural input
        emojiMap.Add(
            "lol",
            "\U0001F602"
        );

        emojiMap.Add(
            "lmao",
            "\U0001F602"
        );

        emojiMap.Add(
            "happy",
            "\U0001F642"
        );

        emojiMap.Add(
            "sad",
            "\U0001F62D"
        );
    }

    //converts text emoji tags into unicode emojis
    //similar to eliza style conversational cues
    //where details increase perceived personality
    public string ParseEmojiText(
        string text
    )
    {
        if (
            string.IsNullOrWhiteSpace(
                text
            )
        )
        {
            return text;
        }

        // pattern detects :emoji: format
        MatchCollection matches =
            Regex.Matches(
                text,
                @":(.*?):"
            );

        foreach (
            Match match
            in matches
        )
        {
            string fullTag =
                match.Value;

            string cleanTag =
                match.Groups[1]
                .Value
                .ToLower()
                .Trim();

            if (
                emojiMap.ContainsKey(
                    cleanTag
                )
            )
            {
                //slight randomness makes responses feel less robotic
                if (
                    Random.value < 0.92f
                )
                {
                    text =
                        text.Replace(
                            fullTag,
                            GetEmojiVariation(
                                cleanTag
                            )
                        );
                }
            }
        }

        return text;
    }

    //adds small random variation for more human feel
    string GetEmojiVariation(
        string emojiName
    )
    {
        switch (emojiName)
        {
            case "laugh":

                return Random.Range(
                    0,
                    3
                ) switch
                {
                    0 => "\U0001F602",
                    1 => "\U0001F923",
                    _ => "\U0001F605"
                };

            case "cry":

                return Random.Range(
                    0,
                    2
                ) switch
                {
                    0 => "\U0001F62D",
                    _ => "\U0001F622"
                };

            case "smile":

                return Random.Range(
                    0,
                    2
                ) switch
                {
                    0 => "\U0001F642",
                    _ => "\U0001F60A"
                };
        }

        return emojiMap[
            emojiName
        ];
    }

    //checks if emoji exists 
    public bool HasEmoji(
        string emojiName
    )
    {
        return emojiMap.ContainsKey(
            emojiName.ToLower()
        );
    }

    //returns direct emoji lookup
    public string Emoji(
        string emojiName
    )
    {
        emojiName =
            emojiName
            .ToLower()
            .Trim();

        if (
            emojiMap.ContainsKey(
                emojiName
            )
        )
        {
            return emojiMap[
                emojiName
            ];
        }

        //fallback protection for invalid emoji tags
        return "";
    }

    //returns all supported emoji names
    public List<string> GetEmojiNames()
    {
        return new List<string>(
            emojiMap.Keys
        );
    }
}