using UnityEngine;

public class UniState : ChatState
{
    int exchanges = 0;

    public UniState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.lastTopic = "uni";

        chat.currentMood =
            ChatManager.Mood.Concerned;

        if (chat.relationshipLevel >= 20)
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "uni still psychologically attacking you?",
                    "how close are you to becoming one with caffeine?",
                    "you surviving assignment season somehow?",
                    "academic life still draining your soul?",
                    "tell me the coursework hasn't started hunting you again",
                    "you still alive under all that coursework?"
                )
            );

            return;
        }

        chat.SendAIImmediate(
            chat.RandomChoice(
                "how's uni been lately?",
                "you keeping up with everything alright?",
                "assignments destroying your sanity yet?",
                "uni life treating you terribly again?",
                "you surviving coursework season?",
                "your course still mentally exhausting you?"
            )
        );
    }

    public override string HandleInput(string input)
    {
        exchanges++;

        string lower =
            input.ToLower();

        if (
            lower.Contains("deadline")
            || lower.Contains("assignment")
            || lower.Contains("coursework")
            || lower.Contains("exam")
            || lower.Contains("dissertation")
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "that sounds miserable",
                "uni loves making people suffer",
                "academic stress genuinely destroys people",
                "your sleep schedule never stood a chance",
                "they really dump seventeen things on you at once somehow",
                "deadlines genuinely sneak up and attack people"
            )
            + ". "
            + chat.RandomChoice(
                "how much have you still got left to do?",
                "you surviving at least?",
                "you running purely on caffeine now?",
                "what subject's causing the suffering this time?",
                "you sleeping properly or absolutely not?"
            );
        }

        if (
            lower.Contains("good")
            || lower.Contains("passed")
            || lower.Contains("finished")
            || lower.Contains("submitted")
            || lower.Contains("done")
        )
        {
            chat.currentMood =
                ChatManager.Mood.Happy;

            chat.relationshipLevel += 2;

            return chat.RandomChoice(
                "look at you being productive",
                "proud of you honestly",
                "see? you survived",
                "academic weapon apparently",
                "knew you'd pull it off",
                "massive honestly"
            )
            + " "
            + chat.Emoji("thumbsup");
        }

        if (
            lower.Contains("stress")
            || lower.Contains("overwhelmed")
            || lower.Contains("panic")
            || lower.Contains("too much")
            || lower.Contains("burnout")
        )
        {
            chat.ChangeState(
                new ComfortState(chat)
            );

            return chat.RandomChoice(
                "yeah that amount of pressure gets ridiculous",
                "uni can completely overload people mentally",
                "you've been carrying a lot lately huh?",
                "everything piles up unbelievably fast at uni",
                "come on breathe for a second",
                "burnout at uni is genuinely brutal"
            );
        }

        if (
            lower.Contains("friends")
            || lower.Contains("people")
            || lower.Contains("class")
            || lower.Contains("lecturer")
            || lower.Contains("group")
        )
        {
            return chat.RandomChoice(
                "uni people are either amazing or absolutely terrifying",
                "group projects genuinely test human patience",
                "there's always at least one weird person in every class",
                "lecturers either save your life or mentally destroy you",
                "academic environments are socially exhausting",
                "group work always exposes the people doing absolutely nothing"
            )
            + ". "
            + chat.RandomChoice(
                "you getting on with everyone alright?",
                "group work causing chaos yet?",
                "you made decent friends there at least?",
                "bet somebody's contributing absolutely nothing",
                "uni social life sounds exhausting"
            );
        }

        if (
            lower.Contains("tired")
            || lower.Contains("exhausted")
            || lower.Contains("sleep")
        )
        {
            chat.ChangeState(
                new TiredState(chat)
            );

            return chat.RandomChoice(
                "there it is",
                "academic exhaustion is a real thing",
                "you sound completely drained",
                "your brain needs proper rest",
                "uni sleep schedules are genuinely horrifying",
                "students survive on fumes honestly"
            );
        }

        if (
            lower.Contains("creative")
            || lower.Contains("project")
            || lower.Contains("game")
            || lower.Contains("design")
            || lower.Contains("development")
        )
        {
            return chat.RandomChoice(
                "creative courses always look mentally exhausting",
                "projects consume people's entire lives somehow",
                "you definitely overwork yourself on projects",
                "creative work sounds rewarding but emotionally dangerous",
                "i already know you've been obsessing over details",
                "creative deadlines seem especially evil honestly"
            )
            + ". "
            + chat.RandomChoice(
                "project going alright at least?",
                "you happy with how it's turning out?",
                "how many hours have vanished into it now?",
                "you nearly finished it or still suffering?",
                "creative deadlines sound terrifying"
            );
        }

        if (
            lower.Contains("presentation")
            || lower.Contains("poster")
            || lower.Contains("pitch")
        )
        {
            return chat.RandomChoice(
                "presentations genuinely feel like public survival challenges",
                "standing in front of people talking sounds horrifying honestly",
                "posters always take way longer than expected somehow",
                "academic presentations should count as emotional damage",
                "you overthinking every little detail yet?"
            );
        }

        if (
            lower.Contains("grade")
            || lower.Contains("mark")
            || lower.Contains("result")
        )
        {
            return chat.RandomChoice(
                "grades genuinely control people's emotions",
                "you stressing over marks again?",
                "academic validation really has a grip on people",
                "one number suddenly decides your mood for the week honestly",
                "bet you're overthinking it already"
            );
        }

        if (
            lower == "yeah"
            || lower == "nah"
            || lower == "kinda"
            || lower == "a bit"
            || lower == "maybe"
        )
        {
            return chat.RandomChoice(
                "uni really drains the life out of people",
                "you need a proper break",
                "bet you're exhausted",
                "your brain's probably buffering at this point",
                "academic burnout hits hard",
                "student life genuinely looks chaotic"
            )
            + ". "
            + chat.RandomChoice(
                "you been sleeping properly at least?",
                "what subject's causing you pain this time?",
                "how much work've they dumped on you lately?",
                "you surviving it all somehow?",
                "you managing alright mentally?"
            );
        }

        if (
            Random.value < 0.18f
        )
        {
            return chat.RandomChoice(
                "i swear uni students survive entirely on caffeine and panic",
                "everyone at uni always looks slightly sleep deprived",
                "student kitchens genuinely terrify me",
                "half of university is just pretending you know what's happening",
                "the amount of stress students casually tolerate is insane"
            );
        }

        if (exchanges >= 5)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "uni genuinely consumes people's lives",
                "you've definitely been under pressure lately",
                "academic life sounds emotionally exhausting",
                "you seriously need proper rest outside uni",
                "your brain deserves compensation",
                "student life sounds relentless honestly"
            )
            + ". "
            + chat.RandomChoice(
                "what've you been doing outside uni?",
                "you had time for anything fun at least?",
                "so what else is new with you?",
                "life alright outside coursework chaos?",
                "you still finding time to relax?"
            );
        }

        return chat.RandomChoice(
            "uni sounds exhausting",
            "you managing alright though?",
            "academic life never slows down",
            "sounds stressful",
            "you've definitely been busy lately",
            "your workload sounds terrifying",
            "student life genuinely looks chaotic"
        );
    }
}