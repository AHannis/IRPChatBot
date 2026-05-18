using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TypingRef : MonoBehaviour
{
    //storing recently reflected topics so reflective callback isnt used too often
    List<string> recentTopics =
        new List<string>();
    // controlling how often follow up questions can be asked throughout topics
    float followUpCooldown = 0f;
    // words to be ignored so bot doesn't misread conversation
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
    // contains known bad phrase combos that previously caused unnatural responses
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
    { //converts to lowercase for understanding user
        string lower =
            input.ToLower();

        if (
            chat.currentState is ComfortState
            || chat.currentState is ConcernState
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
            Random.value < 0.08f
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
    //attempting to understand the user
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
            lower.Split(' ').Length <= 5
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
            string.Join(
                " ",
                words.GetRange(
                    0,
                    Mathf.Min(
                        3,
                        words.Count
                    )
                )
            );

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
    //contains library of reflective responses used to mirror and create eliza style effect
    public string ReflectTopic(
        string topic,
        ChatManager chat
    )
    {
        return chat.RandomChoice(
            topic + " wasn't what i expected you to say honestly",

            "you mention " + topic + " so casually",

            "there's definitely a story behind the "
            + topic
            + " thing",

            topic + " feels oddly specific",

            "why do i feel like "
            + topic
            + " has context i'm missing",

            "somehow "
            + topic
            + " sounds believable coming from you",

            topic + " again huh",

            "i wasn't expecting the conversation to become about "
            + topic,

            "honestly the "
            + topic
            + " thing caught me off guard",

            "you always say things like "
            + topic
            + " so casually"
        );
    }
    //clears stored topic when resetting or restarting convo
    public void ClearRecentTopics()
    {
        recentTopics.Clear();
    }
}