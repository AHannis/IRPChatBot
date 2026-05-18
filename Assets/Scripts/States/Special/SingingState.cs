using UnityEngine;

public class SingingState : ChatState
{
    int singingExchanges = 0;

    public SingingState(ChatManager manager)
        : base(manager)
    {
    }

    public override string HandleInput(
        string input
    )
    {
        singingExchanges++;

        string lower =
            input.ToLower();

        // light playful state for joke music interactions
        if (
            chat.ContainsAny(
                lower,
                "sing with me",
                "sing me a song",
                "lets sing",
                "let's sing",
                "karaoke",
                "write lyrics",
                "write a song"
            )
        )
        {
            return chat.RandomChoice(
                "oh no this never ends well",
                "go on then superstar",
                "this is already becoming chaotic",
                "i already regret agreeing to karaoke",
                "if auntie janice joins in i'm leaving",
                "alright but i'm not hitting the high notes"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "lyrics",
                "chorus",
                "verse",
                "beat",
                "music"
            )
        )
        {
            return chat.RandomChoice(
                "this sounds like the start of a terrible band",
                "we're absolutely getting booed off stage",
                "you better not make me rap",
                "suddenly i'm emotionally invested in this song",
                "we'd either go viral or get banned"
            );
        }

        if (
            lower.Contains("rap")
        )
        {
            return chat.RandomChoice(
                "absolutely not i'm too old for rap battles",
                "the moment i start rapping society collapses",
                "you do NOT want to hear uncle freestyle"
            );
        }

        if (singingExchanges >= 3)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "right that's enough musical suffering",
                "my imaginary ears need a break",
                "before this becomes a full concert let's stop",
                "we are absolutely not starting a band"
            );
        }

        return chat.brain.GetSingingReply();
    }
}