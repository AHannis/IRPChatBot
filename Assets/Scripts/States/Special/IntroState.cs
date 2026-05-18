using UnityEngine;

public class IntroState : ChatState
{
    public IntroState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override void Enter()
    {
        // intro designed to instantly establish character/personality
        chat.SendAIImmediate(
            "hey kid "
            + chat.Emoji("smile")
            + " it's uncle bob. i swapped sims earlier and accidentally wiped half my contacts so i'm re-adding everyone manually. what do you want me to save you as?"
        );
    }

    public override string HandleInput(
        string input
    )
    {
        string lower =
            input.ToLower().Trim();

        string extractedName =
            chat.analyser
            .ExtractNameFlexible(
                input
            );

        string cleanedName =
            chat.analyser
            .CleanName(
                extractedName
            );

        if (
            string.IsNullOrWhiteSpace(
                cleanedName
            )
        )
        {
            chat.ChangeState(
                new NameLoopState(chat)
            );

            return chat.RandomChoice(
                "you actually gotta give me something to work with here",
                "right that was absolutely not a name",
                "i'm trying to save your contact not solve a puzzle",
                "okay try that again properly"
            );
        }

        if (
            cleanedName.Length <= 2
        )
        {
            chat.ChangeState(
                new NameLoopState(chat)
            );

            return chat.RandomChoice(
                "that's suspiciously short",
                "what is that some secret agent codename?",
                "you giving me the full name or what?",
                "i feel like you're messing with me already"
            );
        }

        if (
            cleanedName.Length > 18
        )
        {
            chat.ChangeState(
                new NameLoopState(chat)
            );

            return chat.RandomChoice(
                "that cannot possibly be your actual name",
                "right i'm gonna need a shorter version of that",
                "your contact name isn't fitting on the screen at this rate",
                "that sounds fake"
            );
        }

        chat.playerData.userName =
            cleanedName;

        PlayerPrefs.SetString(
            "UserName",
            chat.playerData.userName
        );

        // occasional fake confusion creates more believable flow
        if (
            Random.value < 0.2f
        )
        {
            chat.ChangeState(
                new NameLoopState(chat)
            );

            return chat.RandomChoice(
                "wait actually are we sticking with "
                + chat.playerData.userName
                + " or are you gonna change it in five minutes?",
                "right before i save this permanently are you SURE you're "
                + chat.playerData.userName + "?",
                "okay just checking because people your age change names every six seconds",
                "you better not suddenly decide you're called something else in ten minutes"
            );
        }

        chat.completedIntro = true;

        chat.ChangeState(
            new PostNameState(chat)
        );

        return chat.RandomChoice(
            "alright "
            + chat.playerData.userName
            + ". saved.",
            "perfect. i've got you as "
            + chat.playerData.userName
            + " now.",
            "okay done. contact crisis resolved.",
            "right i've saved you as "
            + chat.playerData.userName + "."
        )
        + "\n\n"
        + chat.RandomChoice(
            "anyway how've you been?",
            "what've you been up to lately then?",
            "you surviving alright at least?",
            "so what's the latest chaos in your life?",
            "feels like i haven't heard from you in ages"
        );
    }
}