using UnityEngine;

public class ConcernState : ChatState
{
    int concernExchanges = 0;

    public ConcernState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood =
            ChatManager.Mood.Concerned;

        chat.SendAIImmediate(
            chat.RandomChoice(
                "you alright?",
                "something feels off with you lately",
                "talk to me properly for a second",
                "you seem stressed lately",
                "you've sounded a bit drained lately",
                "you don't sound fully yourself today"
            )
        );
    }

    public override string HandleInput(
        string input
    )
    {
        concernExchanges++;

        string lower =
            input.ToLower();

        chat.relationshipLevel += 2;

        if (
            lower.Contains("lol")
            || lower.Contains("lmao")
            || lower.Contains(":cry:")
            || lower.Contains("haha")
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "alright dramatic",
                "you worried me for a second there",
                "okay so we're not having a breakdown then",
                "you're impossible to read sometimes",
                "you really cope through humour huh"
            );
        }

        if (
            lower.Contains("fine")
            || lower.Contains("okay")
            || lower.Contains("alright")
            || lower.Contains("i'm good")
            || lower.Contains("im good")
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "if you say so",
                "alright just checking",
                "don't run yourself into the ground",
                "good. just making sure",
                "you've got to look after yourself too",
                "okay. just don't bottle everything up"
            );
        }

        if (
            lower.Contains("not okay")
            || lower.Contains("bad")
            || lower.Contains("awful")
            || lower.Contains("struggling")
            || lower.Contains("terrible")
            || lower.Contains("overwhelmed")
        )
        {
            chat.ChangeState(
                new ComfortState(chat)
            );

            return chat.RandomChoice(
                "yeah i figured something was wrong",
                "come on then talk to me properly",
                "don't just sit with it alone",
                "you've been carrying a lot mentally huh?",
                "alright tell me what's going on",
                "sounds like you've been struggling quietly for a while"
            );
        }

        if (
            lower.Contains("tired")
            || lower.Contains("stressed")
            || lower.Contains("busy")
            || lower.Contains("exhausted")
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "you seriously need a break",
                "life catching up with you?",
                "you sound exhausted",
                "your brain sounds overloaded",
                "you've had way too much going on lately huh?",
                "you've been running on fumes lately"
            );
        }

        if (
            lower.Contains("school")
            || lower.Contains("uni")
            || lower.Contains("work")
        )
        {
            return chat.RandomChoice(
                "that kind of pressure builds up fast",
                "people underestimate how draining daily life gets",
                "sounds like you've had way too much on",
                "your brain never really gets chance to rest does it?",
                "everything piles up ridiculously fast sometimes"
            );
        }

        if (
            lower.Contains("thanks")
            || lower.Contains("thank you")
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "always",
                "don't mention it",
                "i've got you",
                "you're alright kid",
                "just don't vanish for another month afterwards"
            )
            + " "
            + chat.Emoji("smile");
        }

        if (
            Random.value < 0.15f
        )
        {
            return chat.RandomChoice(
                "seriously though take care of yourself",
                "your brain deserves rest too",
                "sometimes surviving the week is enough",
                "you don't have to fix everything immediately",
                "people forget how exhausting life gets mentally"
            );
        }

        if (concernExchanges >= 2)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "just don't disappear into your own head alright?",
                "make sure you're actually taking care of yourself",
                "seriously though don't bottle everything up",
                "look after yourself properly yeah?",
                "you deserve rest too"
            );
        }

        return chat.RandomChoice(
            "you've seemed a bit distant lately",
            "something definitely feels off",
            "i'm just making sure you're alright",
            "you don't always have to pretend you're fine",
            "you can actually talk to me properly you know",
            "sometimes people hit their limit without realising"
        );
    }
}