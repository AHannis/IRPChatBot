using UnityEngine;

public class NameLoopState : ChatState
{
    int changeCount = 0;

    public NameLoopState(ChatManager manager) : base(manager)
    {
    }

    public override string HandleInput(string input)
    {
        string lower =
            input.ToLower().Trim();

        if (
            lower.Contains("wait no")
            || lower.Contains("actually")
            || lower.Contains("change it")
            || lower.Contains("different")
        )
        {
            return chat.RandomChoice(
                "OH my god",
                "here we go again honestly",
                "you cannot keep changing identities",
                "this is becoming administrative warfare honestly",
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
                "good because i was running out of patience honestly. anyway how are you?",
                "finally we've settled it honestly. so what've you been up to lately?",
                "right perfect. so how's life been treating you?"
            );
        }
        string possibleName =
            chat.analyser.CleanName(
                chat.analyser.ExtractNameFlexible(input)
            );

        if (
            !chat.analyser.IsLikelyName(
                possibleName
            )
        
        )
        {
            return chat.RandomChoice(
                "that is absolutely not helping honestly",
                "you're making this harder than it needs to be",
                "i just need ONE normal name",
                "this contact list is fighting for survival",
                "you're confusing me honestly"
            );
        }

        if (
            possibleName.ToLower()
            == "that"
            || possibleName.ToLower()
            == "okay"
            || possibleName.ToLower()
            == "fine"
        )
        {
            return chat.RandomChoice(
                "those are not names honestly",
                "i refuse to believe your parents named you that",
                "be serious for two seconds",
                "right actual human name please"
            );
        }

        if (
            possibleName.ToLower().Contains("power")
            || possibleName.ToLower().Contains("wizard")
            || possibleName.ToLower().Contains("hero")
            || possibleName.ToLower().Contains("ranger")
        )
        {
            return chat.RandomChoice(
                "i'm not saving a superhero alias",
                "you sound like a comic book character honestly",
                "right and your nemesis is who exactly?",
                "you absolutely made that up",
                "try again with a believable name honestly"
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
                "you changing identities already honestly?"
            );
        }

        if (changeCount == 2)
        {
            return chat.RandomChoice(
                "make your mind up honestly",
                "i'm starting to think you're messing with me now",
                "this contact list's becoming a disaster honestly",
                "okay apparently we're " + chat.userName + " now"
            );
        }

        if (changeCount == 3)
        {
            return chat.RandomChoice(
                "i swear you've had more names than witnesses in a crime documentary",
                "you're impossible honestly",
                "at this point i'm just saving you as mystery person",
                "right. changing it AGAIN"
            );
        }

        if (changeCount == 4)
        {
            return chat.RandomChoice(
                "i'm fighting for my life trying to save this contact honestly",
                "you're absolutely doing this on purpose now",
                "one more change and i'm calling you random citizen",
                "this is why old people hate technology honestly"
            );
        }

        if (changeCount >= 5)
        {
            chat.ChangeState(
                new PostNameState(chat)
            );

            return chat.RandomChoice(
                "right that's it i'm ACTUALLY locking it in as " + chat.userName + ". no more changes honestly. anyway how've you been?",
                "okay final answer apparently. " + chat.userName + ". i'm not changing it again. so how've things been lately?",
                "done. locked. finished. you are now officially " + chat.userName + ". anyway what's been going on with you lately?",
                "alright that's the final form apparently. " + chat.userName + ". how've you been anyway?"
            );
        }

        return chat.RandomChoice(
            "okay now i've got you as " + chat.userName,
            "right changing it to " + chat.userName,
            "alright apparently we're using " + chat.userName + " now",
            "you really can't settle on one name huh?"
        );
    }
}