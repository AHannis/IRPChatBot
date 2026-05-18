using UnityEngine;

public class RouterState : ChatState
{
    //router acts as conversational director
 
    public RouterState(
        ChatManager manager
    ) : base(manager)
    {
    }

    //helper function to reduce repeated routing code
    string RouteToState(
        ChatState state,
        string input
    )
    {
        chat.ChangeState(
            state
        );

        return chat.currentState
            .HandleInput(input);
    }

    public override string HandleInput(
        string input
    )
    {
        string lower =
            input.ToLower();

        //detects direct name corrections
        if (
            lower.StartsWith("my name is ")
            || lower.StartsWith("call me ")
            || (
                (
                    lower.StartsWith("im ")
                    || lower.StartsWith("i'm ")
                )
                && input.Split(' ').Length <= 3
            )
        )
        {
            string newName = input;

            newName = newName
                .Replace("my name is ", "")
                .Replace("My name is ", "")
                .Replace("im ", "")
                .Replace("Im ", "")
                .Replace("i'm ", "")
                .Replace("I'm ", "")
                .Replace("call me ", "")
                .Replace("Call me ", "")
                .Trim();

            //safer name validation
            if (
                newName.Length >= 2
                && newName.Length <= 20
                && char.IsLetter(
                    newName[0]
                )
            )
            {
                string cleaned =
                    char.ToUpper(
                        newName[0]
                    )
                    + newName.Substring(1);

                chat.userName =
                    cleaned;

                chat.Remember(
                    "userName",
                    cleaned
                );

                return chat.RandomChoice(
                    "yeah yeah i know "
                    + cleaned
                    + ", i'm messing with you",

                    "course i knew that already "
                    + cleaned,

                    "i know "
                    + cleaned
                    + " i was joking",

                    "alright alright "
                    + cleaned
                    + ", don't sound so offended",

                    "obviously i know your name's "
                    + cleaned,

                    "relax "
                    + cleaned
                    + " i was just winding you up"
                );
            }
        }

        //high priority emotional detection
        bool emotionalInput =
            chat.ContainsAny(
                lower,

                "sad",
                "upset",
                "crying",
                "stressed",
                "stress",
                "overwhelmed",
                "struggling",
                "depressed",
                "anxious",
                "panic",
                "alone",
                "lonely",
                "hurt",
                "hurting",
                "heartbroken",
                "pain",
                "tired",
                "exhausted",
                "drained",
                "burnt out",
                "burned out",
                "too much",
                "rough lately",
                "been hard",
                "hard lately",
                "can't cope",
                "cant cope",
                "mental health",
                "not okay",
                "not been okay",
                "life sucks",
                "everything sucks",
                "i'm struggling",
                "im struggling",
                "i feel awful",
                "feel terrible"
            );

        //hard conversational topic routing
        bool hardTopicShift =
            chat.ContainsAny(
                lower,

                "school",
                "college",
                "uni",
                "exam",
                "teacher",

                "work",
                "job",
                "boss",
                "manager",
                "office",
                "shift",

                "game",
                "gaming",
                "genshin",
                "honkai",

                "family",
                "mum",
                "dad",
                "brother",
                "sister",

                "food",
                "hungry",
                "pizza",
                "burger",
                "takeaway"
            );

        int detectedAge = -1;

        string[] ageParts =
            lower.Split(' ');

        foreach (string p in ageParts)
        {
            int num;

            if (
                int.TryParse(
                    p,
                    out num
                )
            )
            {
                if (
                    num > 5
                    && num < 100
                )
                {
                    detectedAge = num;

                    break;
                }
            }
        }

        //emotional routing always takes priority
        if (
            emotionalInput
        )
        {
            chat.lastEmotion =
                "emotional";

            Debug.Log(
                "EMOTIONAL OVERRIDE"
            );

            return RouteToState(
                new ComfortState(chat),
                input
            );
        }

        //random uncle interruptions for realism
        //helps conversation feel less scripted
        if (
            !(chat.currentState
            is ComfortState)
            && !(chat.currentState
            is ConcernState)
            && Random.value < 0.05f
            && !chat.storyActive
        )
        {
            Debug.Log(
                "UNCLE STORY INTERRUPTION"
            );

            chat.ChangeState(
                new UncleStoryState(chat)
            );

            return chat.RandomChoice(
                "actually speaking of chaos",
                "wait that reminds me",
                "right random story",
                "okay listen to this",
                "speaking of disasters",
                "you know what happened earlier?"
            );
        }

        //topic based routing
        if (
            hardTopicShift
            || Random.value < 0.05f
        )
        {
            if (
                chat.brain
                .ContainsGamingTerms(
                    lower
                )
            )
            {
                return RouteToState(
                    new GamingState(chat),
                    input
                );
            }

            if (
                chat.brain
                .ContainsSchoolTerms(
                    lower
                )
            )
            {
                return RouteToState(
                    new SchoolState(chat),
                    input
                );
            }

            if (
                chat.brain
                .ContainsUniTerms(
                    lower
                )
            )
            {
                return RouteToState(
                    new UniState(chat),
                    input
                );
            }

            if (
                chat.ContainsAny(
                    lower,
                    "work",
                    "job",
                    "manager",
                    "shift",
                    "boss",
                    "office"
                )
            )
            {
                return RouteToState(
                    new WorkState(chat),
                    input
                );
            }

            if (
                chat.ContainsAny(
                    lower,
                    "family",
                    "mum",
                    "dad",
                    "brother",
                    "sister",
                    "uncle",
                    "aunt"
                )
            )
            {
                return RouteToState(
                    new FamilyState(chat),
                    input
                );
            }

            if (
                chat.ContainsAny(
                    lower,
                    "food",
                    "hungry",
                    "pizza",
                    "burger",
                    "takeaway"
                )
            )
            {
                return RouteToState(
                    new FoodState(chat),
                    input
                );
            }

            if (
                chat.brain
                .ContainsHobbyTerms(
                    lower
                )
            )
            {
                return RouteToState(
                    new HobbyState(chat),
                    input
                );
            }
        }

        //age detection system
        if (
            detectedAge > 0
            && !chat.knowsUserAge
        )
        {
            chat.userAge =
                detectedAge;

            chat.knowsUserAge =
                true;

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
                    "secondary school age then huh?",
                    "mad. feels like everyone's younger than me now"
                )
                + ". "
                + chat.RandomChoice(
                    "school treating you alright?",
                    "you surviving exams?",
                    "school still chaotic?"
                );
            }

            if (
                detectedAge >= 18
                && detectedAge <= 25
            )
            {
                chat.lifeStage =
                    "youngAdult";

                return chat.RandomChoice(
                    "mad. proper adult now then",
                    "christ time flies",
                    "look at you becoming an actual adult"
                )
                + ". "
                + chat.RandomChoice(
                    "you doing uni or working nowadays?",
                    "life treating you alright at least?",
                    "what's taking up your time lately?"
                );
            }

            if (
                detectedAge >= 26
            )
            {
                chat.lifeStage =
                    "adult";

                return chat.RandomChoice(
                    "mad how fast time moves",
                    "proper adult life now then huh",
                    "look at us getting old"
                )
                + ". "
                + chat.RandomChoice(
                    "work keeping you busy nowadays?",
                    "life treating you alright these days?",
                    "you still finding time for yourself?"
                );
            }
        }

        //asks age naturally after enough interaction
        if (
            chat.completedIntro
            && !chat.knowsUserAge
            && !chat.askedAgeRecently
            && chat.totalMessagesSent >= 4
        )
        {
            chat.askedAgeRecently =
                true;

            chat.ChangeState(
                new AgeState(chat)
            );

            return chat.RandomChoice(
                "christ time flies. how old are you now?",
                "wait how old are you these days actually?",
                "mad. how old are you now anyway?",
                "hold on how old are you now?",
                "feels like everyone's growing up ridiculously fast. how old are you?"
            );
        }

        if (
            chat.analyser
            .IsGoodbye(lower)
        )
        {
            return RouteToState(
                new GoodbyeState(chat),
                input
            );
        }

        if (
            chat.analyser
            .IsSingingRequest(lower)
        )
        {
            return RouteToState(
                new SingingState(chat),
                input
            );
        }

        if (
            chat.analyser
            .IsRoleplay(lower)
        )
        {
            return RouteToState(
                new RoleplayState(chat),
                input
            );
        }

        if (
            chat.analyser
            .IsTeasing(lower)
        )
        {
            return RouteToState(
                new JokeState(chat),
                input
            );
        }

        if (
            lower.Contains("awkward")
            || lower.Contains(
                "embarrassed"
            )
            || lower.Contains(
                "cringe"
            )
            || lower.Contains(
                "humiliating"
            )
            || lower.Contains(
                "mortifying"
            )
        )
        {
            return RouteToState(
                new EmbarrasedState(chat),
                input
            );
        }

        if (
            lower.Contains("rude")
            || lower.Contains("mean")
            || lower.Contains("bully")
            || lower.Contains(
                "attacked"
            )
            || lower.Contains(
                "offended"
            )
        )
        {
            return RouteToState(
                new DefensiveState(chat),
                input
            );
        }

        //intentional conversational repair behaviour
        //helps simulate human reading mistakes
        if (
            !(chat.currentState
            is ComfortState)
            && !(chat.currentState
            is ConcernState)
            && chat.ShouldMisread()
            && input.Length > 8
            && Random.value < 0.6f
            && chat.lastEmotion == ""
        )
        {
            string[] words =
                input.Split(' ');

            if (
                words.Length > 0
            )
            {
                return chat.RandomChoice(
                    "wait did you just say '"
                    + words[0]
                    + "'?",

                    "hang on i completely misread that for a second",

                    "thought you said something completely different there",

                    "my brain absolutely failed reading that message",

                    "wait no ignore me i read that completely wrong"
                );
            }
        }

        //long term chaos callback system
        if (
            !(chat.currentState
            is ComfortState)
            && chat.chaosLevel > 3f
            && Random.value < 0.12f
        )
        {
            return chat.RandomChoice(
                "your life genuinely sounds like a sitcom",

                "every time we talk something chaotic's happening",

                "you attract disasters unbelievably fast",

                "i swear chaos follows you specifically",

                "your existence is stressful"
            );
        }

        //contextual memory callbacks
        if (
            chat.HasMemory(
                "favoriteGame"
            )
            && chat.brain.ContainsGamingTerms(
                lower
            )
            && Random.value < 0.05f
        )
        {
            return chat.RandomChoice(
                "you still obsessed with "
                + chat.Recall(
                    "favoriteGame"
                )
                + "?",

                "wait are you still playing "
                + chat.Recall(
                    "favoriteGame"
                )
                + " lately?",

                "random thought but i still don't understand half of "
                + chat.Recall(
                    "favoriteGame"
                )
            );
        }

        //relationship based conversational callbacks
        if (
            !(chat.currentState
            is ComfortState)
            && Random.value < 0.07f
            && chat.relationshipLevel > 10
            && chat.totalMessagesSent > 15
            && chat.lastEmotion == ""
        )
        {
            return chat.RandomChoice(
                "wait no actually",
                "hang on random thought",
                "you know what actually",
                "right completely unrelated",
                "wait i just remembered something"
            )
            + ". "
            + chat.GetConversationContinuation();
        }

        //fallback casual conversation state
        return RouteToState(
            new CasualState(chat),
            input
        );
    }
}