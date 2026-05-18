using UnityEngine;

public class WorkState : ChatState
{
    int workExchanges = 0;

    public WorkState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.lastTopic = "work";

        chat.currentMood =
            ChatManager.Mood.Concerned;

        // higher relationship = more personal openings
        if (chat.relationshipLevel >= 20)
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "work still draining the life out of you?",
                    "please tell me your coworkers are behaving themselves",
                    "adult life treating you terribly again?",
                    "how close are you to launching yourself out a window because of work?",
                    "you still surviving work somehow?",
                    "you still mentally recovering from shifts?"
                )
            );

            return;
        }

        chat.SendAIImmediate(
            chat.RandomChoice(
                "how's work been lately then?",
                "work still chaotic?",
                "people still testing your patience at work?",
                "you been surviving your shifts alright?",
                "work been alright lately?",
                "customers still emotionally exhausting you?"
            )
        );
    }

    public override string HandleInput(string input)
    {
        workExchanges++;

        string lower =
            input.ToLower();

        if (
            chat.relationshipLevel >= 15
            && chat.ContainsAny(
                lower,
                "burnout",
                "can't cope",
                "too much"
            )
        )
        {
            chat.ChangeState(
                new ComfortState(chat)
            );

            return chat.RandomChoice(
                "yeah you've been under pressure for a while",
                "sounds like everything's piling up on you",
                "work burnout genuinely messes people up mentally",
                "you need proper recovery time",
                "you can't keep running on stress forever"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "yeah",
                "yep",
                "exactly",
                "i know",
                "got that right",
                "tell me about it"
            )
        )
        {
            return chat.RandomChoice(
                "some people drain your entire battery",
                "working with the public should qualify as combat",
                "i need recovery time after talking to some people",
                "people are emotionally exhausting",
                "that's why i stay indoors",
                "customers genuinely unlock new levels of frustration"
            )
            + " "
            + chat.Emoji("laugh");
        }

        if (
            chat.ContainsAny(
                lower,
                "holiday",
                "booked",
                "trip",
                "vacation",
                "going away"
            )
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "good",
                "about time where you going?",
                "your brain was begging for a break",
                "nice when are you off?",
                "good. you seriously needed one",
                "finally escaping reality for a bit then"
            )
            + " "
            + chat.Emoji("thumbsup");
        }

        if (
            chat.ContainsAny(
                lower,
                "stress",
                "busy",
                "awful",
                "hate",
                "manager",
                "customers",
                "coworkers"
            )
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "yeah work has a way of sucking the soul out of people",
                "you deserve paying double",
                "sounds miserable",
                "i would've snapped by now",
                "people really test your patience at work huh?",
                "work stress builds up ridiculously fast"
            )
            + ". "
            + chat.RandomChoice(
                "you getting any actual rest outside of work?",
                "how many shifts have you done this week now?",
                "you surviving it alright at least?",
                "adult life is genuinely exhausting",
                "you sleeping properly at least?"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "good",
                "promotion",
                "fine",
                "raise",
                "better"
            )
        )
        {
            chat.currentMood =
                ChatManager.Mood.Happy;

            chat.relationshipLevel += 2;

            return chat.RandomChoice(
                "look at you becoming successful",
                "that's actually really good to hear",
                "proud of you",
                "well someone's thriving",
                "finally some good news from work",
                "see? all the suffering paid off"
            )
            + " "
            + chat.Emoji("thumbsup");
        }

        if (
            chat.ContainsAny(
                lower,
                "quit",
                "left",
                "leaving",
                "resigned"
            )
        )
        {
            chat.currentMood =
                ChatManager.Mood.Playful;

            return chat.RandomChoice(
                "maybe escaping was the right choice",
                "can't blame you there",
                "freedom at last",
                "bet your stress levels dropped instantly",
                "workplaces really push people to the limit",
                "i'd celebrate"
            )
            + " "
            + chat.Emoji("laugh");
        }

        if (
            chat.ContainsAny(
                lower,
                "tired",
                "exhausted",
                "drained"
            )
        )
        {
            chat.ChangeState(
                new TiredState(chat)
            );

            return chat.RandomChoice(
                "you sound completely exhausted",
                "work's been destroying your energy huh?",
                "you seriously need proper rest",
                "you've been running on fumes lately",
                "your brain sounds overloaded",
                "shifts really wipe people out"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "money",
                "pay",
                "bills",
                "rent"
            )
        )
        {
            return chat.RandomChoice(
                "adult life is basically paying bills",
                "everything costs ridiculous amounts now",
                "money stress ages people rapidly",
                "working just to survive is emotionally offensive",
                "life's unbelievably expensive now",
                "payday disappears in about twelve seconds somehow"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "fryer",
                "kitchen",
                "retail",
                "shop",
                "shift"
            )
        )
        {
            return chat.RandomChoice(
                "retail and food jobs genuinely test human patience",
                "people who survive customer service deserve medals",
                "shift work destroys your sense of time",
                "shops somehow become chaotic instantly",
                "kitchen stress looks terrifying"
            );
        }

        // random uncle observations
        if (
            Random.value < 0.18f
        )
        {
            return chat.RandomChoice(
                "half of adulthood is pretending you're not tired",
                "everyone at work always looks slightly exhausted",
                "i swear coworkers become strange after enough shifts together",
                "working life really changes people",
                "people in uniforms always look emotionally drained"
            );
        }

        if (workExchanges >= 4)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "adulthood feels like a scam sometimes",
                "work never really ends does it?",
                "people are exhausting",
                "you seriously need more time to relax",
                "life gets ridiculously busy",
                "you deserve a proper break"
            )
            + ". "
            + chat.RandomChoice(
                "what else have you been up to lately?",
                "anything good outside of work at least?",
                "you been doing anything fun lately?",
                "life alright outside of work chaos?",
                "what's been keeping you busy then?"
            );
        }

        return chat.RandomChoice(
            "sounds exhausting",
            "people are hard work",
            "you seriously need a holiday",
            "work never ends does it",
            "adult life is ridiculous",
            "you deserve a break",
            "jobs seem emotionally draining"
        );
    }
}