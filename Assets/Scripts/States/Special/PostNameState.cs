using UnityEngine;

public class PostNameState : ChatState
{
    bool greeted = false;

    public PostNameState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override void Enter()
    {
        if (!greeted)
        {
            greeted = true;

            // fake familiarity creates stronger conversational continuity
            if (!chat.knowsUserAge)
            {
                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "anyway random question how old are you these days?",
                        "time flies. how old are you now?",
                        "wait how old are you actually now?",
                        "you still school age or fully an adult now?",
                        "i genuinely lose track of how old everyone is nowadays"
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
                    "feels like i haven't heard from you in ages",
                    "so what's been going on with you then?",
                    "you been surviving at least?",
                    "what chaos have you been causing lately?",
                    "right now i've sorted your contact crisis how are you actually doing?"
                )
            );
        }
    }

    public override string HandleInput(
        string input
    )
    {
        string lower =
            input.ToLower();

        if (
            chat.ContainsAny(
                lower,
                "call me",
                "save me as",
                "change my name",
                "change it to"
            )
        )
        {
            string newName =
                chat.analyser.CleanName(
                    chat.analyser.ExtractNameFlexible(
                        input
                    )
                );

            if (
                newName == chat.userName
            )
            {
                return chat.RandomChoice(
                    "that IS what i've got you saved as",
                    "you are literally already " + newName,
                    "i know. that's already your name in my phone",
                    "i'm not completely useless that's already your contact name"
                );
            }

            chat.userName =
                newName;

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
                "make your mind up. now you're " + chat.userName,
                "you enjoy confusing me don't you",
                "your identity changes every five minutes"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "how are you",
                "and you",
                "you?"
            )
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "i'm surviving. nearly fell asleep earlier though. " + chat.GetConversationContinuation(),
                "i've had about three coffees today so mentally i'm vibrating. " + chat.GetConversationContinuation(),
                "i'm alright. your aunt nearly destroyed the microwave earlier. " + chat.GetConversationContinuation(),
                "alive. barely. " + chat.GetConversationContinuation(),
                "mentally i'm somewhere between relaxed and crashing. " + chat.GetConversationContinuation()
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
                "well that's vaguely reassuring. " + chat.GetConversationContinuation(),
                "good. was starting to think you'd vanished. " + chat.GetConversationContinuation(),
                "love that for you. " + chat.GetConversationContinuation(),
                "living the dream then? " + chat.GetConversationContinuation()
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
                "actually that reminds me of something",
                "speaking of chaos",
                "you know what happened to me earlier?",
                "remind me to never leave the house on weekends again",
                "wait i've gotta tell you what happened earlier"
            );
        }

        chat.ChangeState(
            new CasualState(chat)
        );

        return chat.RandomChoice(
            "fair enough. " + chat.GetConversationContinuation(),
            "sounds about right. " + chat.GetConversationContinuation(),
            "look at you being social and everything. " + chat.GetConversationContinuation(),
            "that's very you. " + chat.GetConversationContinuation(),
            "i respect the commitment to vague answers. " + chat.GetConversationContinuation(),
            "you somehow always sound mildly chaotic. " + chat.GetConversationContinuation()
        );
    }
}