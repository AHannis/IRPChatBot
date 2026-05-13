using UnityEngine;

public class GamingState : ChatState
{
    int gamingExchanges = 0;

    public GamingState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.lastTopic = "gaming";

        chat.currentMood =
            ChatManager.Mood.Playful;

        if (
            chat.exchangesSinceGamingQuestion
            < 5
        )
        {
            return;
        }

        chat.exchangesSinceGamingQuestion =
            0;

        if (
            chat.relationshipLevel >= 25
        )
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "you're definitely the type to stay awake gaming until sunrise",
                    "let me guess another game has completely consumed your life",
                    "you vanish off the planet whenever you find a good game",
                    "you've absolutely been neglecting sleep for games again",
                    "go on then what game's stolen your soul this time?",
                    "i already know your sleep schedule's destroyed again"
                )
            );

            return;
        }

        chat.SendAIImmediate(
            chat.RandomChoice(
                "what've you been playing lately?",
                "still glued to games then?",
                "let me guess another weird game i've never heard of",
                "you still staying awake until 3am gaming?",
                "go on then what game's taken over your life this time?",
                "you found another game to obsess over?"
            )
        );
    }

    public override string HandleInput(
        string input
    )
    {
        gamingExchanges++;

        string lower =
            input.ToLower();

        string gameGuess = input;

        gameGuess = gameGuess
            .Replace("i've been", "")
            .Replace("ive been", "")
            .Replace("playing", "")
            .Replace("been on", "")
            .Replace("started", "")
            .Replace("gaming", "")
            .Replace("just", "")
            .Trim();

        if (
            lower.Contains("horror")
            || lower.Contains("resident evil")
            || lower.Contains("silent hill")
            || lower.Contains("phasmophobia")
            || lower.Contains("scary")
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "absolutely not",
                "you're braver than me",
                "i'd throw the controller and leave immediately",
                "those games are stress simulators",
                "see this is why your sleep schedule is destroyed",
                "you enjoy terrifying yourself for fun apparently"
            )
            + ". "
            + chat.RandomChoice(
                "you screamed yet?",
                "you playing that alone or with people?",
                "bet the sound design alone would finish me off",
                "why do horror fans always play in the dark too?",
                "you still jump at everything?"
            );
        }

        if (
            lower.Contains("multiplayer")
            || lower.Contains("online")
            || lower.Contains("ranked")
            || lower.Contains("competitive")
        )
        {
            return chat.RandomChoice(
                "online games bring out the worst in people",
                "competitive games age people rapidly",
                "that's where all the angry shouting comes from",
                "you definitely argue with strangers online",
                "i refuse to believe anyone actually stays calm in ranked games",
                "ranked games sound like emotional warfare"
            )
            + ". "
            + chat.RandomChoice(
                "you winning at least?",
                "how many controllers nearly got launched?",
                "you playing with friends or randoms?",
                "people online are terrifying",
                "bet somebody called you trash within five minutes"
            );
        }

        if (
            lower.Contains("story")
            || lower.Contains("single player")
            || lower.Contains("campaign")
        )
        {
            return chat.RandomChoice(
                "see that's more my kind of thing",
                "story games are way less stressful",
                "i can respect a game with an actual story",
                "single player games feel less emotionally damaging",
                "those games always end up taking over your life though",
                "story games always leave people emotionally destroyed somehow"
            )
            + ". "
            + chat.RandomChoice(
                "good story at least?",
                "you nearly finished it?",
                "you gotten attached to fictional characters again?",
                "how many hours deep are you now?",
                "you gonna replay it after finishing it too?"
            );
        }

        if (
            lower.Contains("fun")
            || lower.Contains("good")
            || lower.Contains("love")
            || lower.Contains("amazing")
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "good. at least you're enjoying yourself",
                "you get way too invested in games",
                "i swear you live online sometimes",
                "you always disappear when a game's good",
                "that's your entire week gone then",
                "yeah you're definitely obsessed already"
            )
            + ". "
            + chat.RandomChoice(
                "how many hours already?",
                "you recommending it then?",
                "you been playing it nonstop?",
                "worth buying or not?",
                "you obsessed with it yet?"
            );
        }

        if (
            lower.Contains("bad")
            || lower.Contains("boring")
            || lower.Contains("awful")
            || lower.Contains("hate")
        )
        {
            return chat.RandomChoice(
                "sounds tragic",
                "money well wasted then",
                "you always find strange games",
                "game companies really charge money for anything now",
                "nothing worse than buying a disappointing game",
                "that's painful honestly"
            )
            + ". "
            + chat.RandomChoice(
                "you at least finish bad games?",
                "you uninstall it immediately?",
                "see this is why i don't trust reviews",
                "how bad are we talking?",
                "you gonna complain about it for the next month now?"
            );
        }

        if (
            lower.Contains("friends")
            || lower.Contains("co-op")
            || lower.Contains("coop")
        )
        {
            return chat.RandomChoice(
                "games are always funnier with friends",
                "co-op games usually end in arguments",
                "you definitely blame each other constantly",
                "nothing destroys friendships faster than co-op games",
                "i already know somebody's getting yelled at",
                "co-op games somehow become survival situations"
            )
            + ". "
            + chat.RandomChoice(
                "who've you been playing with?",
                "you carrying them or what?",
                "you all coordinated or just chaos?",
                "bet nobody knows what's happening half the time",
                "somebody definitely keeps causing problems"
            );
        }

        if (
            lower.Contains("tired")
            || lower.Contains("late")
            || lower.Contains("sleep")
        )
        {
            chat.ChangeState(
                new TiredState(chat)
            );

            return chat.RandomChoice(
                "there it is",
                "knew the gaming sleep deprivation would appear eventually",
                "your sleep schedule never stood a chance",
                "you need actual rest",
                "gaming until sunrise is not self care",
                "your body's begging for sleep at this point"
            );
        }

        if (
            lower.Contains("minecraft")
            || lower.Contains("fortnite")
            || lower.Contains("roblox")
            || lower.Contains("cod")
            || lower.Contains("valorant")
        )
        {
            return chat.RandomChoice(
                "those games genuinely consume people",
                "i swear everybody on earth plays that",
                "that community sounds terrifying",
                "you've definitely rage quit that before",
                "i can already hear the shouting through the screen",
                "those games permanently alter people's personalities"
            )
            + ". "
            + chat.RandomChoice(
                "you actually good at it?",
                "how many hours have vanished into that game?",
                "you playing ranked on that too?",
                "you still enjoying it or just addicted?",
                "you got a favourite game mode or what?"
            );
        }

        if (
            lower.Contains("fnaf")
            || lower.Contains("five nights")
        )
        {
            return chat.RandomChoice(
                "those animatronics are horrifying",
                "people are way too calm about haunted robot bears",
                "that series gave an entire generation trust issues",
                "i still don't understand the lore at all",
                "those games feel stressful on purpose"
            )
            + ". "
            + chat.RandomChoice(
                "you into the lore side too?",
                "which game's your favourite?",
                "you watching theory videos at 2am too?",
                "those jumpscares would've finished me off",
                "you playing them or just watching stuff about them?"
            );
        }

        if (
            chat.HasMemory("favoriteGame")
            && Random.value < 0.12f
        )
        {
            return
                "you're still obsessed with "
                + chat.Recall(
                    "favoriteGame"
                )
                + "?";
        }

        if (
            !string.IsNullOrWhiteSpace(
                gameGuess
            )
        )
        {
            if (gameGuess.Length <= 30)
            {
                chat.Remember(
                    "favoriteGame",
                    gameGuess
                );
            }
        }

        if (gamingExchanges >= 4)
        {
            gamingExchanges = 0;

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "anyway you've definitely been gaming too much lately",
                "right i'm convinced games consume your entire life now",
                "fair enough sounds like you've been busy then",
                "you always end up finding the strangest games",
                "i still don't understand half the things you talk about",
                "your brain's basically powered by games at this point"
            )
            + ". "
            + chat.RandomChoice(
                "what else have you been up to lately?",
                "anything besides gaming?",
                "you been doing much outside of that?",
                "life been alright otherwise?",
                "what's been keeping you busy?"
            );
        }

        if (
            lower == "yeah"
            || lower == "nah"
            || lower == "kinda"
            || lower == "a bit"
        )
        {
            return chat.RandomChoice(
                "sounds about right honestly",
                "you always end up addicted to something",
                "i can already tell you've been playing nonstop",
                "your free time's vanished again then",
                "gaming really does consume your life"
            );
        }

        return chat.RandomChoice(
            "you and your games",
            "sounds chaotic",
            "fair enough",
            "i've no idea what any of that means",
            "modern gaming sounds exhausting",
            "you definitely take games too seriously",
            "i still can't keep up with game names anymore"
        );
    }
}