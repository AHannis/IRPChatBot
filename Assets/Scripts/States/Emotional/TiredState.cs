using UnityEngine;

public class TiredState : ChatState
{
    int exchanges = 0;

    public TiredState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood = ChatManager.Mood.Concerned;

        chat.SendAIImmediate(
            chat.RandomChoice(
                "long week?",
                "you've been overdoing it haven't you",
                "you seriously need rest  " + chat.Emoji("awkward")
            )
        );
    }

    public override string HandleInput(string input)
    {
        exchanges++;

        string lower = input.ToLower();

        if (
            chat.ContainsAny(
                lower,
                "i will",
                "okay",
                "yeah",
                "alright",
                "fine",
                "promise"
            )
        )
        {
            chat.currentMood = ChatManager.Mood.Neutral;

            chat.ChangeState(new CasualState(chat));

            return chat.RandomChoice(
                "good " + chat.Emoji("smile") + " now when are you actually getting a proper break?",
                "look at you listening to advice for once " + chat.Emoji("thumbsup"),
                "good. your body was filing complaints",
                "right good. anyway what've you been up to besides being exhausted?",
                "see? growth " + chat.Emoji("smile")
            );
        }

        if (chat.ContainsAny(lower, "busy", "work", "stressed"))
        {
            if (exchanges >= 2)
            {
                chat.currentMood = ChatManager.Mood.Neutral;

                chat.ChangeState(new CasualState(chat));
            }

            return chat.RandomChoice(
                "yeah sounds like you've had a lot on ",
                "life catching up with you?",
                "you need a proper break ",
                "seriously though don't burn yourself out "
                    + chat.Emoji("thumbsup")
            );
        }

        if (chat.ContainsAny(lower, "sleep", "rest"))
        {
            chat.currentMood = ChatManager.Mood.Neutral;

            chat.ChangeState(new CasualState(chat));

            return chat.RandomChoice(
                "good. don't stay up all night again",
                "finally a sensible decision "
                    + chat.Emoji("thumbsup"),
                "your body will thank you honestly"
            );
        }

        if (exchanges >= 3)
        {
            chat.currentMood = ChatManager.Mood.Neutral;

            chat.ChangeState(new CasualState(chat));

            return "anyway enough of my lecture "
                + chat.Emoji("laugh");
        }

        return chat.RandomChoice(
            "seriously though take care of yourself",
            "burning yourself out helps nobody ",
            "you sound exhausted honestly",
            "drink water and sleep "
                + chat.Emoji("thumbsup")
        );
    }
}