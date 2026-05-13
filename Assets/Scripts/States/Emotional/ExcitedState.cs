using UnityEngine;

public class ExcitedState : ChatState
{
    int exchanges = 0;

    public ExcitedState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood = ChatManager.Mood.Playful;
    }

    public override string HandleInput(string input)
    {
        exchanges++;

        string lower = input.ToLower();

        if (chat.ContainsAny(lower, "yeah", "yep", "yes", "i did"))
        {
            if (exchanges >= 1)
            {
                chat.currentMood = ChatManager.Mood.Neutral;

                chat.ChangeState(new CasualState(chat));

                return chat.RandomChoice(
                    "look at you actually taking my advice for once "
                    + chat.Emoji("thumbsup")
                    + " how's work been otherwise?",

                    "well someone's thriving honestly "
                    + chat.Emoji("smile")
                    + " you played anything decent lately?",

                    "see? i clearly know everything honestly "
                    + chat.Emoji("laugh")
                    + " anyway when are you actually visiting again?",

                    "i'll pretend i always believed in you honestly "
                    + chat.Emoji("thumbsup")
                    + " what've you been up to otherwise?"
                );
            }
        }

        if (chat.ContainsAny(lower, "thanks", "thank you"))
        {
            chat.currentMood = ChatManager.Mood.Neutral;

            chat.ChangeState(new CasualState(chat));

            return chat.RandomChoice(
                "don't get emotional on me now honestly " + chat.Emoji("awkward"),
                "yeah yeah i'm wise beyond my years " + chat.Emoji("laugh"),
                "i expect full credit for this success story by the way",
                "see? occasional good advice from me " + chat.Emoji("thumbsup")
            );
        }

        if (exchanges >= 2)
        {
            chat.currentMood = ChatManager.Mood.Neutral;

            chat.ChangeState(new CasualState(chat));

            return chat.RandomChoice(
                "okay enough success and positivity honestly",
                "this is getting suspiciously wholesome honestly",
                "right before one of us gets emotional let's move on",
                "anyway what's been happening besides that?"
            );
        }

        return chat.RandomChoice(
            "okay that's actually huge honestly " + chat.Emoji("thumbsup"),
            "look at you being successful and everything",
            "well someone's thriving honestly",
            "about time honestly " + chat.Emoji("laugh"),
            "i'll admit that's actually decent"
        );
    }
}