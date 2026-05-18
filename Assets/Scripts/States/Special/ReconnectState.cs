using UnityEngine;

public class ReconnectState : ChatState
{
    bool askedAge = false;

    public ReconnectState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override void Enter()
    {
        // relationship level changes how familiar the uncle sounds
        if (
            chat.relationshipLevel >= 30
        )
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "look who finally returned",
                    "you vanished off the planet again",
                    "i thought you'd been abducted",
                    "there you are. i was starting to think you'd disappeared forever",
                    "you always reappear like a mysterious side character",
                    "the lost child returns at last"
                )
            );

            return;
        }

        if (
            chat.relationshipLevel >= 15
        )
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "haven't heard from you in a while",
                    "you been alright lately?",
                    "thought you'd disappeared",
                    "you've been quiet lately huh?",
                    "good to see you're still alive",
                    "you still surviving over there?"
                )
            );

            return;
        }

        chat.SendAIImmediate(
            chat.RandomChoice(
                "haven't heard from you in a while you okay?",
                "hey kid. you been alright?",
                "you disappeared for a bit there",
                "thought i'd lost your number again",
                "there you are",
                "look who decided to reappear"
            )
        );
    }

    public override string HandleInput(
        string input
    )
    {
        string lower =
            input.ToLower();

        // reconnect conversations naturally shift back into learning details
        if (
            !chat.knowsUserAge
            && !askedAge
            && chat.ContainsAny(
                lower,
                "good",
                "fine",
                "okay",
                "alright"
            )
        )
        {
            askedAge = true;

            chat.ChangeState(
                new AgeState(chat)
            );

            return chat.RandomChoice(
                "good. christ time flies though how old are you now?",
                "glad you're alright. how old are you these days anyway?",
                "good to hear. you must be getting older now how old are you?",
                "nice. feels like everyone's growing up ridiculously fast now",
                "good. wait how old are you now actually?",
                "mad how fast time moves. how old are you these days?"
            );
        }

        chat.ChangeState(
            new CasualState(chat)
        );

        if (
            chat.ContainsAny(
                lower,
                "good",
                "fine",
                "okay",
                "alright"
            )
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "good. what've you been up to then?",
                "glad you're surviving",
                "good. life been treating you alright?",
                "nice. you keeping busy lately?",
                "good to hear",
                "love that for you"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "tired",
                "meh",
                "bored",
                "drained"
            )
        )
        {
            return chat.RandomChoice(
                "yeah you sound exhausted",
                "life catching up with you huh?",
                "you've sounded drained lately",
                "your energy levels sound tragic",
                "you seriously need proper rest",
                "you sound mentally cooked"
            )
            + ". "
            + chat.RandomChoice(
                "you been sleeping properly?",
                "what's been stressing you out then?",
                "life just busy or what?",
                "you taking care of yourself at least?",
                "everything piling up lately?"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "stress",
                "overwhelmed",
                "bad",
                "awful"
            )
        )
        {
            chat.ChangeState(
                new ComfortState(chat)
            );

            return chat.RandomChoice(
                "yeah i figured something was off",
                "come on then what's been going on?",
                "you've been carrying a lot mentally huh?",
                "life hitting hard lately?",
                "talk to me properly for a second",
                "you've seemed overwhelmed lately"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "gaming",
                "game",
                "playing"
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
            chat.ContainsAny(
                lower,
                "busy",
                "work",
                "uni",
                "school"
            )
        )
        {
            return chat.RandomChoice(
                "yeah life's relentless",
                "everything piles up unbelievably fast",
                "sounds like you've had a lot going on",
                "adult life is basically organised chaos",
                "you've definitely been busy lately huh?",
                "your schedule sounds horrifying"
            )
            + ". "
            + chat.RandomChoice(
                "you managing alright at least?",
                "you getting any actual rest?",
                "what's been taking up most of your time?",
                "you surviving it all somehow?",
                "you still finding time for yourself?"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "missed you",
                "miss you"
            )
        )
        {
            return chat.RandomChoice(
                "aww look at you getting emotional",
                "don't make this weird now",
                "yeah yeah i missed you too probably",
                "that's suspiciously wholesome from you",
                "you're lucky i tolerate you"
            )
            + " "
            + chat.Emoji("smile");
        }

        if (
            chat.ContainsAny(
                lower,
                "nothing",
                "not much"
            )
        )
        {
            return chat.RandomChoice(
                "there's no way that's true",
                "you definitely sound like somebody avoiding details",
                "you always say that and then reveal something ridiculous later",
                "fair enough",
                "living mysteriously i see"
            );
        }

        return chat.RandomChoice(
            "alright just checking in on you",
            "good to hear from you again",
            "you always vanish and then reappear randomly",
            "i swear you disappear into another dimension sometimes",
            "anyway what've you been up to lately?",
            "so what's been happening with you lately then?"
        );
    }
}