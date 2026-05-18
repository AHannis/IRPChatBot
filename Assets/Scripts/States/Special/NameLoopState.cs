using UnityEngine;

public class NameLoopState : ChatState
{
    int changeCount = 0;

    public NameLoopState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override string HandleInput(
        string input
    )
    {
        string lower =
            input.ToLower().Trim();

        // repeated corrections create a running joke across messages
        if (
            chat.ContainsAny(
                lower,
                "wait no",
                "actually",
                "change it",
                "different"
            )
        )
        {
            return chat.RandomChoice(
                "OH my god",
                "here we go again",
                "you cannot keep changing identities",
                "this is becoming administrative warfare",
                "right what's the new name then?"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "keep",
                "stick",
                "fine",
                "yes",
                "that works",
                "leave it"
            )
        )
        {
            chat.ChangeState(
                new PostNameState(chat)
            );

            return chat.RandomChoice(
                "alright sticking with " + chat.userName + " then. how've you been lately?",
                "good because i was running out of patience. anyway how are you?",
                "finally we've settled it. so what've you been up to lately?",
                "right perfect. so how's life been treating you?"
            );
        }

        string possibleName =
            chat.analyser.CleanName(
                chat.analyser.ExtractNameFlexible(
                    input
                )
            );

        if (
            chat.analyser.IsLikelyName(
                input,
                chat.userName
            )
        )
        {
            return chat.RandomChoice(
                "that is absolutely not helping",
                "you're making this harder than it needs to be",
                "i just need ONE normal name",
                "this contact list is fighting for survival",
                "you're confusing me"
            );
        }

        if (
            possibleName.ToLower() == "that"
            || possibleName.ToLower() == "okay"
            || possibleName.ToLower() == "fine"
        )
        {
            return chat.RandomChoice(
                "those are not names",
                "i refuse to believe your parents named you that",
                "be serious for two seconds",
                "right actual human name please"
            );
        }

        chat.userName =
            possibleName;

        PlayerPrefs.SetString(
            "UserName",
            chat.userName
        );

        changeCount++;

        if (changeCount == 1)
        {
            return chat.RandomChoice(
                "okay so NOW you're " + chat.userName + "?",
                "right got it. " + chat.userName + " this time",
                "alright changing it to " + chat.userName,
                "you changing identities already?"
            );
        }

        if (changeCount == 2)
        {
            return chat.RandomChoice(
                "make your mind up",
                "i'm starting to think you're messing with me now",
                "this contact list's becoming a disaster",
                "okay apparently we're " + chat.userName + " now"
            );
        }

        if (changeCount >= 3)
        {
            chat.ChangeState(
                new PostNameState(chat)
            );

            return chat.RandomChoice(
                "right that's it i'm locking it in as " + chat.userName,
                "okay final answer apparently",
                "done. no more identity changes",
                "i refuse to rename this contact again"
            );
        }

        return chat.RandomChoice(
            "okay now i've got you as " + chat.userName,
            "right changing it to " + chat.userName,
            "apparently we're using " + chat.userName + " now",
            "you really can't settle on one name huh?"
        );
    }
}