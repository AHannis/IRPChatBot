using System.Collections.Generic;
using UnityEngine;

public class ResponseProcessor : MonoBehaviour
{
    //main post-processing layer
    //helps hide repetition + robotic patterns
    //similar to eliza style conversational illusion techniques
    public string ProcessResponse(
        string reply,
        ChatManager chat
    )
    {
        //reduces excessive questioning
        reply =
            PreventQuestionSpam(
                reply,
                chat
            );

        //prevents repeated phrasing
        reply =
            PreventRepeatReplies(
                reply,
                chat
            );

        //small cleanup pass for more natural text flow
        reply =
            CleanupResponse(
                reply
            );

        return reply;
    }

    //prevents bot from sounding too interrogative
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
            //softens questions into statements
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
                    "whatve",
                    "you've mentioned"
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

    //avoids repeated responses ruining realism
    string PreventRepeatReplies(
        string reply,
        ChatManager chat
    )
    {
        //exact duplicate check
        if (
            chat.recentAIReplies.Contains(
                reply
            )
        )
        {
            return chat.brain
                .GetNaturalReply();
        }

        //checks for similar sentence openings
        if (
            StartsSimilar(
                reply,
                chat.recentAIReplies
            )
        )
        {
            //occasionally redirects into followup instead
            if (
                Random.value < 0.35f
            )
            {
                return
                    chat.brain.GetNaturalReply()
                    + ". "
                    + chat.brain.GetGeneralFollowUp();
            }

            return chat.brain
                .GetNaturalReply();
        }

        return reply;
    }

    //detects responses beginning too similarly
    //helps prevent repetitive bot 
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

            //high similarity threshold
            if (
                matchingChars >= 14
            )
            {
                return true;
            }
        }

        return false;
    }

    //small text cleanup system
    //helps responses feel less generated
    string CleanupResponse(
        string reply
    )
    {
        reply =
            reply.Replace(
                "  ",
                " "
            );

        reply =
            reply.Replace(
                "..",
                "."
            );

        reply =
            reply.Replace(
                " .",
                "."
            );

        return reply.Trim();
    }
}