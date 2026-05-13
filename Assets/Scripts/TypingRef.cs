using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TypingRef : MonoBehaviour
{
    List<string> recentTopics =
        new List<string>();

    float followUpCooldown = 0f;

    string[] ignoredWords =
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
        "nice",
        "pretty",
        "much",
        "sounds",
        "lot",
        "actually",
        "caught",
        "guard",
        "happening",
        "lately",
        "then",
        "writing",
        "nothing",
        "everything",
        "literally",
        "question",
        "questions",
        "mean",
        "meant",
        "what",
        "why",
        "huh",
        "explain",
        "true",
        "fair",
        "real",
        "honestly",
        "though",
        "basically",
        "probably",
        "maybe",
        "randomly",
        "anyways",
        "anywayyy",
        "alright",
        "okayyy",
        "yknow",
        "guess",
        "sorta",
        "kinda"
    };

    string[] blockedTopics =
    {
        "pretty much",
        "sounds right",
        "about right",
        "not lot",
        "just writing",
        "yeah haha",
        "fair enough",
        "sounds about",
        "been happening",
        "good hear",
        "hear again",
        "yeah here",
        "pretty much",
        "not gonna",
        "caught guard",
        "sounds like"
    };

    public string GenerateReflectiveResponse(
        string input,
        ChatManager chat
    )
    {
        string lower =
            input.ToLower();

        if (
            lower.Contains(
                "what was the question"
            )
            || lower.Contains(
                "what question"
            )
            || lower.Contains(
                "which question"
            )
            || lower.Contains(
                "what do you mean"
            )
            || lower.Contains(
                "huh"
            )
            || lower.Contains(
                "explain"
            )
            || lower == "what"
            || lower == "why"
        )
        {
            return "";
        }

        if (
            chat.analyser.ContainsAny(
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
                "true",
                "pretty much",
                "sounds about right"
            )
        )
        {
            return "";
        }

        string topic =
            ExtractTopic(
                input
            );

        if (
            string.IsNullOrEmpty(
                topic
            )
        )
        {
            return "";
        }

        if (
            recentTopics.Contains(
                topic
            )
            && Random.value < 0.7f
        )
        {
            return "";
        }

        recentTopics.Add(
            topic
        );

        if (
            recentTopics.Count > 15
        )
        {
            recentTopics.RemoveAt(
                0
            );
        }

        string response =
            ReflectTopic(
                topic,
                chat
            );

        if (
            Random.value < 0.18f
            && Time.time > followUpCooldown
        )
        {
            response +=
                ". "
                + chat.brain.GetGeneralFollowUp();

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
            lower.Split(' ').Length <= 3
        )
        {
            return "";
        }

        if (
            lower.Contains(
                "what was the question"
            )
            || lower.Contains(
                "what question"
            )
            || lower.Contains(
                "which question"
            )
        )
        {
            return "";
        }

        List<string> words =
            new List<string>(
                lower.Split(' ')
            );

        words.RemoveAll(
            w =>
                System.Array.Exists(
                    ignoredWords,
                    x => x == w
                )
                || w.Length <= 2
        );

        if (
            words.Count <= 1
        )
        {
            return "";
        }

        string combined =
            words[0]
            + " "
            + words[1];

        foreach (
            string blocked
            in blockedTopics
        )
        {
            if (
                combined.Contains(
                    blocked
                )
            )
            {
                return "";
            }
        }

        if (
            combined.StartsWith(
                "not "
            )
            || combined.StartsWith(
                "just "
            )
            || combined.StartsWith(
                "yeah "
            )
            || combined.StartsWith(
                "okay "
            )
        )
        {
            return "";
        }

        return combined;
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
            "i feel like there's a story behind the " + topic,
            topic + "? elaborate immediately",
            "not gonna lie the " + topic + " thing caught me off guard",
            "why are we talking about " + topic,
            topic + " is oddly specific",
            "you say things like " + topic + " so casually"
        );
    }

    public void ClearRecentTopics()
    {
        recentTopics.Clear();
    }
}