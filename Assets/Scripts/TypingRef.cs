using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TypingRef : MonoBehaviour
{
    List<string> recentTopics =
        new List<string>();

    float followUpCooldown = 0f;

    public string GenerateReflectiveResponse(
        string input,
        ChatManager chat
    )
    {
        string lower =
            input.ToLower();

        if (
            chat.ContainsAny(
                lower,
                "yes",
                "yeah",
                "ok",
                "okay",
                "cool",
                "nice",
                "haha",
                "lol",
                "lmao",
                "sure",
                "right",
                "fine",
                "fair",
                "true"
            )
        )
        {
            return "";
        }

        string topic =
            ExtractTopic(input);

        if (string.IsNullOrEmpty(topic))
        {
            return "";
        }

        if (
            recentTopics.Contains(topic)
            && Random.value < 0.7f
        )
        {
            return "";
        }

        recentTopics.Add(topic);

        if (recentTopics.Count > 15)
        {
            recentTopics.RemoveAt(0);
        }

        string response =
            ReflectTopic(
                topic,
                chat
            );

        if (
            Random.value < 0.25f
            && Time.time > followUpCooldown
        )
        {
            response +=
                ". "
                + chat.GetDynamicFollowUp();

            followUpCooldown =
                Time.time + 35f;
        }

        return response;
    }

    public string ExtractTopic(
        string input
    )
    {
        string lower =
            input.ToLower();

        lower =
            Regex.Replace(
                lower,
                @"[^\w\s]",
                ""
            );

        if (
            lower.Split(' ').Length <= 2
        )
        {
            return "";
        }

        string[] ignore =
        {
            "im",
            "i",
            "am",
            "the",
            "a",
            "an",
            "and",
            "to",
            "anyway",
            "sing",
            "with",
            "me",
            "please",
            "can",
            "could",
            "would",
            "should",
            "want",
            "go",
            "come",
            "tell",
            "say",
            "listen",
            "look",
            "been",
            "still",
            "just",
            "really",
            "very",
            "like",
            "is",
            "are",
            "was",
            "were",
            "have",
            "has",
            "had",
            "ive",
            "you",
            "your",
            "my",
            "our",
            "that",
            "haha",
            "lol",
            "lmao",
            "hehe",
            "hahaha",
            "yeah",
            "yep",
            "nah",
            "okay",
            "ok",
            "right",
            "sure",
            "fine",
            "time",
            "this",
            "with",
            "from",
            "about",
            "because",
            "thing",
            "things",
            "stuff",
            "today",
            "yesterday",
            "tomorrow",
            "good",
            "bad",
            "cool",
            "weird",
            "tired",
            "okay",
            "fine",
            "nice",
            "actually",
            "literally"
        };

        List<string> words =
            new List<string>(
                lower.Split(' ')
            );

        words.RemoveAll(
            w =>
                System.Array.Exists(
                    ignore,
                    x => x == w
                )
                || w.Length <= 2
        );

        if (words.Count == 0)
        {
            return "";
        }

        if (words.Count >= 2)
        {
            return
                words[0]
                + " "
                + words[1];
        }

        return words[0];
    }

    public string ReflectTopic(
        string topic,
        ChatManager chat
    )
    {
        return chat.RandomChoice(
            topic + "? that's oddly specific",
            "why " + topic + "?",
            topic + " actually sounds like something you'd do",
            "you've been into " + topic + " lately?",
            topic + " again?",
            topic + " sounds concerning",
            "i feel like there's a story behind the " + topic + " thing",
            topic + "? elaborate immediately",
            "not gonna lie the " + topic + " thing caught me off guard",
            "right but why are we talking about " + topic,
            topic + " is a very strange hobby",
            "you say things like " + topic + " so casually"
        );
    }
}