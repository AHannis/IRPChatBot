using UnityEngine;

public class GoodbyeState : ChatState
{
    public GoodbyeState(ChatManager manager) : base(manager)
    {
    }

    public override string HandleInput(string input)
    {
        string lower = input.ToLower();

        chat.analyser.userSaidGoodbye = true;

        if (
            lower.Contains("bye")
            || lower.Contains("goodbye")
            || lower.Contains("see you")
            || lower.Contains("gn")
            || lower.Contains("goodnight")
            || lower.Contains("cya")
            || lower.Contains("goodbyeeee")
            || lower.Contains("byeeeee")
            || lower.Contains("ttyl")
        )
        {
            return chat.brain.GetGoodbyeReply();
        }

        chat.analyser.userSaidGoodbye = false;

        chat.ChangeState(new CasualState(chat));

        return chat.RandomChoice(
            "oh you're back already honestly",
            "that goodbye lasted about three seconds",
            "welcome back menace",
            "see i knew you'd return eventually"
        );
    }
}