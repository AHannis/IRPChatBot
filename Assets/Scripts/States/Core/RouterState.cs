using UnityEngine;

public class RouterState : ChatState
{
    public RouterState(
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
        

        if (
            chat.currentState
            is SchoolState
        )
        {
            if (
                !chat.ContainsAny(
                    lower,
                    "game",
                    "gaming",
                    "food",
                    "family",
                    "sleep",
                    "tired",
                    "work",
                    "job"
                )
            )
            {
                return chat.currentState
                    .HandleInput(input);
            }
        }

        if (
            chat.currentState
            is WorkState
        )
        {
            if (
                !chat.ContainsAny(
                    lower,
                    "school",
                    "game",
                    "gaming",
                    "family",
                    "food"
                )
            )
            {
                return chat.currentState
                    .HandleInput(input);
            }
        }
        if (
     chat.IsAbsurdInput(lower)
     && !chat.IsLikelyActivityResponse(lower)
 )
        {
            return chat.RandomChoice(
               
                "you're impossible to get a straight answer from honestly",
                "you sound like an npc side character",
                "i genuinely can't tell when you're joking anymore"
            );
        }
        if (
            chat.currentState
            is GamingState
        )
        {
            if (
                !chat.ContainsAny(
                    lower,
                    "school",
                    "work",
                    "job",
                    "family"
                )
            )
            {
                return chat.currentState
                    .HandleInput(input);
            }
        }

        if (
            chat.currentState
            is FamilyState
        )
        {
            if (
                !chat.ContainsAny(
                    lower,
                    "game",
                    "gaming",
                    "school",
                    "work"
                )
            )
            {
                return chat.currentState
                    .HandleInput(input);
            }
        }
        if (
            detectedAge > 0
            && !chat.knowsUserAge
        )
        {
            chat.userAge =
                detectedAge;

            chat.knowsUserAge = true;

            if (detectedAge < 18)
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

            if (detectedAge >= 26)
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

        if (
     chat.completedIntro
     && !chat.knowsUserAge
     && !chat.askedAgeRecently
     && chat.totalMessagesSent >= 4
 )
        {
            chat.askedAgeRecently = true;

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
            chat.analyser.IsGoodbye(
                lower
            )
        )
        {
            chat.ChangeState(
                new GoodbyeState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            chat.analyser
            .IsSingingRequest(lower)
        )
        {
            chat.ChangeState(
                new SingingState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            chat.analyser
            .IsRoleplay(lower)
        )
        {
            chat.ChangeState(
                new RoleplayState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            chat.analyser
            .IsTeasing(lower)
        )
        {
            chat.ChangeState(
                new JokeState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            lower.Contains("awkward")
            || lower.Contains(
                "embarrassed"
            )
            || lower.Contains("cringe")
            || lower.Contains(
                "humiliating"
            )
            || lower.Contains(
                "mortifying"
            )
        )
        {
            chat.ChangeState(
                new EmbarrasedState(chat)
            );

            return chat.currentState
                .HandleInput(input);
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
            chat.ChangeState(
                new DefensiveState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            lower.Contains("sad")
            || lower.Contains("upset")
            || lower.Contains("crying")
            || lower.Contains(
                "stressed"
            )
            || lower.Contains(
                "overwhelmed"
            )
            || lower.Contains(
                "struggling"
            )
            || lower.Contains(
                "depressed"
            )
            || lower.Contains(
                "anxious"
            )
        )
        {
            chat.ChangeState(
                new ConcernState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            chat.brain
            .ContainsGamingTerms(
                lower
            )
        )
        {
            chat.ChangeState(
                new GamingState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            chat.knowsUserAge
            && chat.lifeStage
            == "school"
        )
        {
            if (
                chat.brain
                .ContainsSchoolTerms(
                    lower
                )
            )
            {
                chat.ChangeState(
                    new SchoolState(chat)
                );

                return chat.currentState
                    .HandleInput(input);
            }
        }

        if (
            chat.knowsUserAge
            && (
                chat.lifeStage
                == "youngAdult"
                || chat.lifeStage
                == "uni"
            )
        )
        {
            if (
                chat.brain
                .ContainsUniTerms(
                    lower
                )
            )
            {
                chat.ChangeState(
                    new UniState(chat)
                );

                return chat.currentState
                    .HandleInput(input);
            }
        }

        if (
            chat.knowsUserAge
            && (
                chat.lifeStage
                == "adult"
                || chat.lifeStage
                == "youngAdult"
            )
        )
        {
            if (
                chat.ContainsAny(
                    lower,
                    "work",
                    "job",
                    "manager",
                    "shift",
                    "coworker",
                    "boss",
                    "salary",
                    "pay",
                    "office"
                )
            )
            {
                chat.ChangeState(
                    new WorkState(chat)
                );

                return chat.currentState
                    .HandleInput(input);
            }
        }

        if (
            chat.brain
            .ContainsHobbyTerms(
                lower
            )
        )
        {
            chat.ChangeState(
                new HobbyState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            chat.ContainsAny(
                lower,
                "food",
                "hungry",
                "eat",
                "takeaway",
                "snack",
                "meal",
                "pizza",
                "burger"
            )
        )
        {
            chat.ChangeState(
                new FoodState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            chat.ContainsAny(
                lower,
                "family",
                "mum",
                "mom",
                "dad",
                "sister",
                "brother",
                "nan",
                "grandad",
                "cousin",
                "uncle",
                "aunt"
            )
        )
        {
            chat.ChangeState(
                new FamilyState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            chat.ContainsAny(
                lower,
                "sleep",
                "tired",
                "exhausted",
                "drained"
            )
        )
        {
            chat.ChangeState(
                new TiredState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }
        if (
            chat.ShouldMisread()
            && input.Length > 8
            && Random.value < 0.6f
            && chat.lastEmotion == ""
        )
        {
            string[] words =
                input.Split(' ');

            if (words.Length > 0)
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

        if (
     Random.value < 0.04f
     && !chat.storyActive
     && chat.lastEmotion == ""
 )
        {
            chat.ChangeState(
                new UncleStoryState(chat)
            );

            return chat.RandomChoice(
                "actually speaking of chaos",
                "wait that reminds me actually",
                "you know what listen to this",
                "right random story",
                "speaking of disasters"
            );
        }

        if (
            chat.chaosLevel > 3f
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

        if (
            chat.HasMemory(
                "favoriteGame"
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

        if (
    Random.value < 0.07f
    && chat.relationshipLevel > 10
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

        chat.ChangeState(
            new CasualState(chat)
        );

        return chat.currentState
            .HandleInput(input);
    }
}