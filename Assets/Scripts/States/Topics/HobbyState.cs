using UnityEngine;

public class HobbyState : ChatState
{
    int exchanges = 0;

    public HobbyState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.lastTopic = "hobby";
    }

    public override string HandleInput(string input)
    {
        exchanges++;

        string lower =
            input.ToLower();

        // remembers recurring hobbies
        if (
            lower.Contains("reading")
        )
        {
            chat.Remember(
                "favoriteHobby",
                "reading"
            );
        }

        if (
            lower.Contains("reading")
            || lower.Contains("book")
            || lower.Contains("novel")
            || lower.Contains("manga")
        )
        {
            if (
                chat.HasMemory("favoriteBook")
            )
            {
                return chat.RandomChoice(
                    "you still into " + chat.Recall("favoriteBook") + " stuff?",
                    "you finding anything that tops " + chat.Recall("favoriteBook") + " yet?",
                    "you always end up finding strange books"
                );
            }

            return chat.RandomChoice(
                "anything good at least?",
                "you reading horror stuff again?",
                "physical books or kindle?",
                "you one of those people that reads ten books at once?",
                "sounds relaxing",
                "i used to fall asleep holding books",
                "you ever read something so good you ignore everyone for hours?",
                "half the time i pretend i'll read and then immediately get distracted"
            );
        }

        if (
            lower.Contains("drawing")
            || lower.Contains("art")
            || lower.Contains("painting")
            || lower.Contains("sketch")
        )
        {
            return chat.RandomChoice(
                "you actually getting good at it now?",
                "that takes patience",
                "you drawing anything specific?",
                "better than staring at screens all day",
                "sounds relaxing at least",
                "i genuinely can't draw anything except wonky stickmen",
                "art people scare me you all have too much talent",
                "you ever spend ages on something and then hate it immediately afterwards?"
            );
        }

        if (
            lower.Contains("music")
            || lower.Contains("song")
            || lower.Contains("band")
            || lower.Contains("playlist")
            || lower.Contains("spotify")
        )
        {
            return chat.RandomChoice(
                "you still listening to weird music?",
                "anything decent or just depressing playlists?",
                "music fixes most problems",
                "what've you been listening to lately?",
                "you always find the strangest songs",
                "your generation listens to songs with the saddest sounding titles",
                "i swear music hits different at stupid hours of the night",
                "some songs instantly teleport you back to random memories"
            );
        }

        if (
            lower.Contains("gaming")
            || lower.Contains("game")
            || lower.Contains("games")
        )
        {
            chat.ChangeState(
                new GamingState(chat)
            );

            return chat.RandomChoice(
                "still grinding games?",
                "you've definitely lost sleep gaming before",
                "what've you been playing lately then?",
                "gaming communities genuinely frighten me sometimes"
            );
        }

        if (
            lower.Contains("collect")
            || lower.Contains("collection")
            || lower.Contains("figures")
            || lower.Contains("plush")
        )
        {
            return chat.RandomChoice(
                "collecting things becomes addictive weirdly fast",
                "you running out of shelf space yet?",
                "everyone ends up collecting something random",
                "i respect the dedication",
                "you ever buy something and immediately think 'where am i even putting this'"
            );
        }

        if (
            lower.Contains("writing")
            || lower.Contains("story")
            || lower.Contains("fanfic")
        )
        {
            return chat.RandomChoice(
                "you still writing dramatic emotional stuff?",
                "writing seems exhausting mentally",
                "you ever reread old writing and physically recoil?",
                "creative people have too much power",
                "you writing for fun or emotionally processing things secretly?"
            );
        }

        if (
            chat.HasMemory("favoriteHobby")
            && Random.value < 0.1f
        )
        {
            return
                "you really do spend a lot of time with "
                + chat.Recall("favoriteHobby");
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
                "sounds relaxing at least",
                "better than doomscrolling all night",
                "you been doing that a lot lately?",
                "that's probably good for you",
                "nice having hobbies that don't involve stress",
                "everyone needs something that switches their brain off",
                "could be worse",
                "fair enough"
            );
        }

        if (
            Random.value < 0.22f
        )
        {
            return chat.RandomChoice(
                "i always wanted a hobby that made me look interesting",
                "i tried gardening once and nearly killed everything",
                "your aunt keeps trying to get me into random hobbies",
                "half my hobbies nowadays are accidentally falling asleep",
                "i swear hobbies get more expensive the older you get"
            );
        }

        if (exchanges >= 6)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "anyway what else have you been up to?",
                "so what's new outside of that?",
                "you been keeping busy otherwise?",
                "sounds like you've had a lot going on lately",
                "fair enough. what else is new?"
            );
        }

        return chat.RandomChoice(
            "that sounds pretty chill",
            "nice having something relaxing to do",
            "you seem really into that lately",
            "sounds like you've been keeping busy",
            "better than being bored all day",
            "that's probably healthy for your brain",
            "you always end up finding interesting stuff to do",
            "fair enough"
        );
    }
}