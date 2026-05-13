using UnityEngine;

public class PostNameState : ChatState
{
    bool greeted = false;

    public PostNameState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        if (!greeted)
        {
            greeted = true;

            if (!chat.knowsUserAge)
            {
                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "anyway random question how old are you these days honestly?",
                        "time flies honestly how old are you now?",
                        "wait how old are you actually now?",
                        "you still school age or fully an adult now honestly?",
                        "i genuinely lose track of how old everyone is nowadays honestly"
                    )
                );

                chat.ChangeState(
                    new AgeState(chat)
                );

                return;
            }

            chat.SendAIImmediate(
                chat.RandomChoice(
                    "anyway how've you been?",
                    "feels like i haven't heard from you in ages honestly",
                    "so what's been going on with you then?",
                    "you been surviving at least?",
                    "what chaos have you been causing lately?",
                    "right now i've sorted your contact crisis how are you actually doing?"
                )
            );
        }
    }

    public override string HandleInput(string input)
    {
        string lower = input.ToLower();

        if (
            lower.Contains("call me")
            || lower.Contains("save me as")
            || lower.Contains("change my name")
            || lower.Contains("actually save me as")
            || lower.Contains("change it to")
        )
        {
            string newName =
                chat.analyser.CleanName(
                    chat.analyser.ExtractNameFlexible(input)
                );

            if (newName == chat.userName)
            {
                return chat.RandomChoice(
                    "that IS what i've got you saved as honestly",
                    "you are literally already " + newName,
                    "i know. that's already your name in my phone",
                    "i'm not completely useless honestly that's already your contact name"
                );
            }

            chat.userName = newName;

            PlayerPrefs.SetString(
                "UserName",
                chat.userName
            );

            chat.ChangeState(
                new NameLoopState(chat)
            );

            return chat.RandomChoice(
                "okay NOW you're " + chat.userName + "?",
                "right so we're changing it again?",
                "make your mind up honestly. now you're " + chat.userName,
                "you enjoy confusing me don't you",
                "your identity changes every five minutes honestly"
            );
        }

        if (
            lower.Contains("how are you")
            || lower.Contains("and you")
            || lower.Contains("you?")
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "i'm surviving honestly. nearly fell asleep earlier though. " + chat.GetConversationContinuation(),
                "i've had about three coffees today so mentally i'm vibrating. " + chat.GetConversationContinuation(),
                "i'm alright honestly. your aunt nearly destroyed the microwave earlier. " + chat.GetConversationContinuation(),
                "alive. barely honestly. " + chat.GetConversationContinuation(),
                "mentally i'm somewhere between relaxed and crashing honestly. " + chat.GetConversationContinuation()
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "good",
                "fine",
                "okay",
                "alright",
                "cool"
            )
        )
        {
            chat.relationshipLevel++;

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "look at you surviving and everything. " + chat.GetConversationContinuation(),
                "well that's vaguely reassuring honestly. " + chat.GetConversationContinuation(),
                "good. was starting to think you'd vanished honestly. " + chat.GetConversationContinuation(),
                "love that for you honestly. " + chat.GetConversationContinuation(),
                "living the dream then honestly? " + chat.GetConversationContinuation()
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "tired",
                "exhausted",
                "drained",
                "burnt out"
            )
        )
        {
            chat.ChangeState(
                new TiredState(chat)
            );

            if (chat.relationshipLevel >= 15)
            {
                return chat.RandomChoice(
                    "yeah you've sounded exhausted lately honestly",
                    "you seriously need proper sleep at some point",
                    "your sleep schedule is being held together with tape honestly",
                    "you're running yourself into the ground honestly"
                );
            }

            return chat.RandomChoice(
                "yeah you sound exhausted honestly",
                "you need sleep before you collapse dramatically somewhere",
                "your sleep schedule is fighting for its life honestly"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "busy",
                "working",
                "work",
                "stressed",
                "overwhelmed"
            )
        )
        {
            if (
                lower.Contains("stressed")
                || lower.Contains("overwhelmed")
            )
            {
                chat.ChangeState(
                    new ConcernState(chat)
                );

                return chat.RandomChoice(
                    "yeah that doesn't sound great honestly",
                    "you've been carrying a lot mentally huh?",
                    "life piling up on you lately?",
                    "come on then what's been going on properly?"
                );
            }

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "yeah you always sound busy honestly. you surviving work at least?",
                "you need one normal relaxing week honestly",
                "your life always sounds chaotic somehow. " + chat.GetConversationContinuation(),
                "honestly i think you attract stress psychologically",
                "you ever actually stop and relax honestly?"
            );
        }

        if (
            Random.value < 0.2f
            && chat.relationshipLevel > 5
        )
        {
            chat.ChangeState(
                new UncleStoryState(chat)
            );

            return chat.RandomChoice(
                "actually that reminds me of something honestly",
                "speaking of chaos",
                "you know what happened to me earlier?",
                "remind me to never leave the house on weekends again honestly",
                "wait i've gotta tell you what happened earlier honestly"
            );
        }

        if (
            Random.value < 0.15f
        )
        {
            return chat.RandomChoice(
                "random question what games are people obsessed with nowadays honestly?",
                "you still watching weird stuff online at 2am or have you matured yet?",
                "what even is popular with people your age now honestly?",
                "you still into the same hobbies or have they changed again?",
                "actually wait what've you mainly been doing lately?"
            );
        }

        chat.ChangeState(
            new CasualState(chat)
        );

        return chat.RandomChoice(
            "fair enough honestly. " + chat.GetConversationContinuation(),
            "sounds about right honestly. " + chat.GetConversationContinuation(),
            "look at you being social and everything. " + chat.GetConversationContinuation(),
            "honestly that's very you. " + chat.GetConversationContinuation(),
            "i respect the commitment to vague answers honestly. " + chat.GetConversationContinuation(),
            "you somehow always sound mildly chaotic honestly. " + chat.GetConversationContinuation()
        );
    }
}