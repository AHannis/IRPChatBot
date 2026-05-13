using System.Collections.Generic;
using UnityEngine;

public class ResponseProcessor : MonoBehaviour
{
    public string ProcessResponse(
        string reply,
        ChatManager chat
    )
    {
        reply =
            PreventQuestionSpam(
                reply,
                chat
            );

        reply =
            PreventRepeatReplies(
                reply,
                chat
            );

        reply =
            chat.typingDots
            .AddOccasionalTypo(
                reply
            );

        return reply;
    }

    string PreventQuestionSpam(
        string reply,
        ChatManager chat
    )
    {
        if (
            chat.exchangesSinceQuestion < 2
            && reply.Contains("?")
            && Random.value < 0.7f
        )
        {
            reply =
                reply.Replace(
                    "?",
                    "."
                );

            reply =
                reply.Replace(
                    "what's",
                    "thats"
                );

            reply =
                reply.Replace(
                    "you okay",
                    "hope you're alright"
                );

            reply =
                reply.Replace(
                    "you alright",
                    "hope you're alright"
                );
        }

        return reply;
    }

    string PreventRepeatReplies(
        string reply,
        ChatManager chat
    )
    {
        if (
            chat.recentAIReplies.Contains(
                reply
            )
            || StartsSimilar(
                reply,
                chat.recentAIReplies
            )
        )
        {
            return chat.brain
                .GetNaturalReply();
        }

        return reply;
    }

    public bool StartsSimilar(
        string newReply,
        List<string> oldReplies
    )
    {
        foreach (
            string oldReply
            in oldReplies
        )
        {
            if (
                oldReply.Length < 8
                || newReply.Length < 8
            )
            {
                continue;
            }

            string oldStart =
                oldReply.Substring(
                    0,
                    Mathf.Min(
                        20,
                        oldReply.Length
                    )
                );

            string newStart =
                newReply.Substring(
                    0,
                    Mathf.Min(
                        20,
                        newReply.Length
                    )
                );

            int matchingChars = 0;

            for (
                int i = 0;
                i < Mathf.Min(
                    oldStart.Length,
                    newStart.Length
                );
                i++
            )
            {
                if (
                    oldStart[i]
                    == newStart[i]
                )
                {
                    matchingChars++;
                }
            }

            if (
                matchingChars >= 14
            )
            {
                return true;
            }
        }

        return false;
    }
}