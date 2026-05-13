using UnityEngine;

public class SchoolState : ChatState
{
    int schoolExchanges = 0;

    public SchoolState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.lastTopic = "school";

        chat.currentMood =
            ChatManager.Mood.Playful;

        if (chat.relationshipLevel >= 20)
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "secondary school still functioning as emotional warfare?",
                    "you surviving the chaos over there somehow?",
                    "school still full of drama and sleep deprivation?",
                    "tell me people are behaving themselves for once",
                    "school life still absolute madness?",
                    "you still mentally surviving school?"
                )
            );

            return;
        }

        chat.SendAIImmediate(
            chat.RandomChoice(
                "school still surviving without burning down then?",
                "how's school been lately?",
                "you drowning in homework yet?",
                "secondary school still chaotic?",
                "let me guess somebody being dramatic again",
                "you surviving lessons alright?"
            )
        );
    }

    public override string HandleInput(string input)
    {
        schoolExchanges++;

        string lower =
            input.ToLower();

        if (chat.lastEmotion == "socialStress")
        {
            if (
                lower.Contains("girls")
                || lower.Contains("friends")
                || lower.Contains("people")
                || lower.Contains("someone")
            )
            {
                return chat.RandomChoice(
                    "people at school can be horrible sometimes",
                    "teenagers genuinely scare me sometimes",
                    "some people just enjoy causing problems",
                    "school drama spreads faster than viruses somehow",
                    "school friendships become chaos unbelievably fast"
                );
            }

            if (
                lower.Contains("mean")
                || lower.Contains("bully")
                || lower.Contains("rude")
            )
            {
                return chat.RandomChoice(
                    "that's rough",
                    "you don't deserve that",
                    "people can be genuinely cruel at that age",
                    "school can be exhausting socially",
                    "some people enjoy being awful for no reason"
                );
            }

            return chat.RandomChoice(
                "school drama is exhausting",
                "secondary school politics sound like warfare",
                "some people never grow out of acting awful",
                "half of school is emotional survival",
                "teenage social situations sound terrifying"
            );
        }

        if (
            lower.Contains("homework")
            || lower.Contains("exam")
            || lower.Contains("revision")
            || lower.Contains("test")
            || lower.Contains("coursework")
            || lower.Contains("gcse")
        )
        {
            return chat.RandomChoice(
                "school really tries consuming your entire life",
                "revision sounds miserable",
                "i don't miss exams at all",
                "half of school is just stress management",
                "teachers act like their subject is the only one you have",
                "revision season destroys people mentally"
            )
            + ". "
            + chat.RandomChoice(
                "you coping alright with it?",
                "what subject's causing the suffering?",
                "you revising properly or pretending?",
                "bet everyone's panicking already",
                "you leaving everything until the last minute again?"
            );
        }

        if (
            lower.Contains("friend")
            || lower.Contains("mate")
            || lower.Contains("people")
            || lower.Contains("class")
        )
        {
            return chat.RandomChoice(
                "school friend groups are basically reality tv shows",
                "secondary school drama is terrifying",
                "every week somebody falls out with somebody",
                "you lot somehow make everything dramatic",
                "school gossip spreads unbelievably fast",
                "friend groups change every three business days somehow"
            )
            + ". "
            + chat.RandomChoice(
                "your friends behaving themselves at least?",
                "who's causing chaos this week then?",
                "you staying out of drama or joining in?",
                "bet somebody's arguing again",
                "school friendships change every five minutes"
            );
        }

        if (
            lower.Contains("boyfriend")
            || lower.Contains("girlfriend")
            || lower.Contains("crush")
            || lower.Contains("dating")
            || lower.Contains("relationship")
            || lower.Contains("like someone")
        )
        {
            chat.currentMood =
                ChatManager.Mood.Playful;

            return chat.RandomChoice(
                "OH here we go",
                "teenage drama detected immediately",
                "i knew we'd reach this topic eventually",
                "look at you becoming all romantic",
                "right spill everything immediately",
                "you smiling at your phone every five minutes now?"
            )
            + ". "
            + chat.RandomChoice(
                "they nice at least?",
                "does your family know yet?",
                "you smiling at your phone constantly now or what?",
                "how long has this been happening then?",
                "school relationships are emotional warfare"
            );
        }

        if (
            lower.Contains("teacher")
            || lower.Contains("lesson")
            || lower.Contains("detention")
        )
        {
            return chat.RandomChoice(
                "teachers can smell chaos from a mile away",
                "detention is basically school prison",
                "some teachers genuinely enjoy terrifying students",
                "there's always one teacher everybody fears",
                "school rules get stranger every year",
                "teachers somehow hear everything instantly"
            )
            + ". "
            + chat.RandomChoice(
                "what happened then?",
                "you in trouble again?",
                "please tell me you didn't argue with a teacher",
                "which lesson's the worst?",
                "you behaving most of the time at least?"
            );
        }

        if (
            lower.Contains("tired")
            || lower.Contains("stress")
            || lower.Contains("overwhelmed")
            || lower.Contains("pressure")
        )
        {
            chat.ChangeState(
                new AdviceState(chat)
            );

            return chat.RandomChoice(
                "school stress hits hard sometimes",
                "alright slow down for a second",
                "you're putting way too much pressure on yourself",
                "you actually need breaks sometimes",
                "can't spend every hour stressing over school",
                "your brain sounds overloaded lately"
            );
        }

        if (
            lower.Contains("good")
            || lower.Contains("fine")
            || lower.Contains("alright")
            || lower.Contains("okay")
        )
        {
            return chat.RandomChoice(
                "good. at least you're surviving",
                "sounds like school's not destroying you yet",
                "well that's better than total disaster",
                "look at you functioning properly",
                "i'll take that as a win",
                "honestly that's better than most people sound"
            )
            + ". "
            + chat.RandomChoice(
                "what subject do you actually like though?",
                "you still got loads of homework?",
                "your friends alright lately?",
                "you enjoying school at all?",
                "what've you been doing besides school?"
            );
        }

        if (
            lower.Contains("music")
            || lower.Contains("art")
            || lower.Contains("drama")
            || lower.Contains("creative")
        )
        {
            return chat.RandomChoice(
                "creative subjects always seem more fun",
                "finally a subject with actual personality",
                "those lessons always have the most chaotic people",
                "creative classes sound way less miserable",
                "i'd survive school better with subjects like that",
                "creative people always stay up too late somehow"
            )
            + ". "
            + chat.RandomChoice(
                "you good at it at least?",
                "you actually enjoy those lessons?",
                "better than maths at least",
                "you got projects for that too?",
                "you been making anything cool lately?"
            );
        }

        if (
            lower.Contains("maths")
            || lower.Contains("science")
            || lower.Contains("history")
        )
        {
            return chat.RandomChoice(
                "some subjects genuinely feel like psychological torture",
                "i'd fail immediately",
                "school expects people to remember ridiculous amounts",
                "your brain must be exhausted",
                "some lessons feel seventeen hours long",
                "teachers move through topics unbelievably fast"
            )
            + ". "
            + chat.RandomChoice(
                "you actually good at it?",
                "which subject's the worst?",
                "you revising loads for it?",
                "bet everyone's struggling with that subject",
                "you surviving it alright?"
            );
        }

        if (
            Random.value < 0.15f
        )
        {
            return chat.RandomChoice(
                "school corridors sound unbelievably loud",
                "teenagers somehow have infinite energy",
                "everyone at school always seems either stressed or chaotic",
                "school sleep schedules are genuinely awful",
                "half of school is just surviving social situations"
            );
        }

        if (schoolExchanges >= 4)
        {
            schoolExchanges = 0;

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "secondary school sounds exhausting these days",
                "you lot deal with way too much drama",
                "school really is its own universe",
                "i don't miss school at all",
                "you'll survive it somehow",
                "honestly being a teenager sounds tiring"
            )
            + ". "
            + chat.RandomChoice(
                "what else have you been up to lately?",
                "you still gaming loads too?",
                "anything interesting happening outside school?",
                "you been keeping busy?",
                "what've you been doing after school?"
            );
        }

        return chat.RandomChoice(
            "school sounds chaotic",
            "fair enough",
            "you lot have way too much drama",
            "secondary school genuinely sounds exhausting",
            "i'm too old for school problems now",
            "sounds about right",
            "teenage life sounds stressful honestly"
        );
    }
}