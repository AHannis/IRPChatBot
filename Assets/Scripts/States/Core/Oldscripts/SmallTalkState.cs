using UnityEngine;
//OLD ORIGINAL SCRIPT BEFORE CASUAL STATE WAS MADE
public class SmallTalkState : ChatState
{
    int exchanges = 0;

    public SmallTalkState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
    }

    public override string HandleInput(string input)
    {
        exchanges++;

        string lower = input.ToLower();

        if (
            lower.Contains("lmao")
            || lower.Contains("lol")
            || lower.Contains(chat.Emoji("cry"))
        )
        {
            return chat.RandomChoice(
                "glad you're enjoying my suffering",
                "you're laughing but it was traumatic",
                "i knew you'd find that funny",
                "absolute chaos honestly",
                "i'm never recovering from that"
            );
        }

        if (
            lower == "yeah"
            || lower == "true"
            || lower == "fair"
            || lower == "same"
            || lower == "real"
        )
        {
            return chat.RandomChoice(
                "exactly",
                "see you get it",
                "glad someone understands",
                "finally someone agrees with me",
                "i'm always right honestly"
            );
        }

        if (
            lower == "nah"
            || lower == "no"
        )
        {
            return chat.RandomChoice(
                "fair enough",
                "can't blame you",
                "probably for the best honestly",
                "that's understandable",
                "honestly valid"
            );
        }

        if (exchanges >= 4)
        {
            chat.ChangeState(new CasualState(chat));

            return chat.RandomChoice(
                "anyway what've you been up to lately?",
                "so what's been going on with you?",
                "you been keeping busy?",
                "what else is new then?"
            );
        }

        return chat.RandomChoice(
            "you really are something else",
            "honestly fair",
            "that's very you",
            "you make me laugh sometimes",
            "chaotic behaviour honestly"
        );
    }
}