using UnityEngine;

public class AgeState : ChatState
{
    public AgeState(
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

        int detectedAge =
            ExtractAge(lower);

        // exaggerated replies make the character feel reactive and human
        if (
            lower.Contains("999")
            || lower.Contains("1000")
            || lower.Contains("immortal")
            || detectedAge >= 80
        )
        {
            return chat.RandomChoice(
                "right so you're either immortal or lying",
                "i'm not saving fossil records into my contacts",
                "you absolutely made that age up",
                "right okay ancient one",
                "i refuse to believe you're older than civilisation",
                "you belong in a museum at that age"
            )
            + ". "
            + chat.RandomChoice(
                "seriously how old are you actually?",
                "come on give me a real age",
                "be honest this time",
                "right actual answer now"
            );
        }

        if (
            detectedAge <= 0
        )
        {
            return chat.RandomChoice(
                "wait i still don't actually know how old you are",
                "you completely dodged the question there",
                "right but you never answered me",
                "you're being suspiciously secretive about your age",
                "so we're pretending i didn't ask then?"
            );
        }

        chat.userAge =
            detectedAge;

        chat.knowsUserAge =
            true;

        PlayerPrefs.SetInt(
            "UserAge",
            detectedAge
        );

        PlayerPrefs.SetInt(
            "KnowsUserAge",
            1
        );

        if (
            detectedAge < 18
        )
        {
            chat.lifeStage =
                "school";

            chat.ChangeState(
                new SchoolState(chat)
            );

            return chat.RandomChoice(
                "christ you're still young",
                "you're basically still a child",
                "where's the time gone",
                "secondary school age then huh?",
                "you're still in the school trenches"
            )
            + ". "
            + chat.RandomChoice(
                "school treating you alright?",
                "you drowning in homework yet?",
                "school still chaotic?",
                "you surviving exams and all that?"
            );
        }

        if (
            detectedAge >= 18
            && detectedAge <= 25
        )
        {
            chat.lifeStage =
                "youngAdult";

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "mad. proper adult now then",
                "christ time flies",
                "you're at that weird life stage now huh",
                "look at you becoming an actual adult"
            )
            + ". "
            + chat.RandomChoice(
                "you doing uni or working nowadays?",
                "what's taking up most of your time lately?",
                "life treating you alright at least?",
                "you figuring life out or just winging it?"
            );
        }

        chat.lifeStage =
            "adult";

        chat.ChangeState(
            new CasualState(chat)
        );

        return chat.RandomChoice(
            "look at us getting old",
            "mad how fast time moves",
            "proper adult life now then huh",
            "christ where did the years go"
        )
        + ". "
        + chat.RandomChoice(
            "what's life looking like for you lately?",
            "you still finding time for yourself?",
            "life treating you alright these days?"
        );
    }

    int ExtractAge(
        string input
    )
    {
        string cleaned =
            input.Replace(",", " ")
            .Replace(".", " ")
            .Replace("!", " ")
            .Replace("?", " ");

        string[] words =
            cleaned.Split(' ');

        foreach (
            string word
            in words
        )
        {
            int number;

            if (
                int.TryParse(
                    word,
                    out number
                )
            )
            {
                if (
                    number > 5
                    && number < 80
                )
                {
                    return number;
                }
            }
        }

        return -1;
    }
}