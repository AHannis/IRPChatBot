using UnityEngine;

public class GoodbyeState : ChatState
{
    public GoodbyeState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override string HandleInput(
        string input
    )
    {
        string lower =
            input.ToLower();

        // tracks whether user actually ended conversation
        chat.analyser.userSaidGoodbye = true;

        if (
            chat.ContainsAny(
                lower,
                "bye",
                "goodbye",
                "see you",
                "gn",
                "goodnight",
                "cya",
                "ttyl"
            )
        )
        {
            return chat.brain
                .GetGoodbyeReply();
        }

        chat.analyser.userSaidGoodbye = false;

        chat.ChangeState(
            new CasualState(chat)
        );

        return chat.RandomChoice(
            "oh you're back already",
            "that goodbye lasted about three seconds",
            "welcome back menace",
            "see i knew you'd return eventually"
        );
    }
}