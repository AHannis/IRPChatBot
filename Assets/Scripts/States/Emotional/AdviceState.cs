using UnityEngine;

public class AdviceState : ChatState
{
    int adviceDepth = 0;

    public AdviceState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood = ChatManager.Mood.Concerned;

        chat.SendAIImmediate(
            chat.RandomChoice(
                "alright talk to me properly then",
                "come on what's actually bothering you?",
                "you've clearly got something on your mind honestly"
            )
        );
    }

    public override string HandleInput(string input)
    {
        adviceDepth++;

        string lower = input.ToLower();

        if (chat.ContainsAny(lower, "stress", "anxious", "overwhelmed"))
        {
            if (adviceDepth >= 2)
            {
                chat.ChangeState(new CasualState(chat));

                return "one thing at a time alright? don't try carrying everything at once";
            }

            return chat.RandomChoice(
                "you've been under pressure for too long honestly",
                "your brain needs a break at some point",
                "you can't solve everything in one night"
            );
        }

        if (chat.ContainsAny(lower, "relationship", "friend", "partner"))
        {
            chat.ChangeState(new CasualState(chat));

            return chat.RandomChoice(
                "people get complicated honestly",
                "sometimes you just need to talk properly with people",
                "communication fixes more than people realise"
            );
        }

        if (adviceDepth >= 3)
        {
            chat.ChangeState(new CasualState(chat));

            return "anyway enough of my life coaching session honestly";
        }

        return chat.RandomChoice(
            "you'll figure it out honestly",
            "just don't be too hard on yourself",
            "life gets messy sometimes",
            "you've handled worse before"
        );
    }
}