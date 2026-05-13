using UnityEngine;

public class SingingState : ChatState
{
    int singingExchanges = 0;

    public SingingState(ChatManager manager) : base(manager)
    {
    }

    public override string HandleInput(string input)
    {
        singingExchanges++;

        string lower = input.ToLower();

        if (
            lower.Contains("sing with me")
            || lower.Contains("sing me a song")
            || lower.Contains("lets sing")
            || lower.Contains("let's sing")
            || lower.Contains("write lyrics")
            || lower.Contains("write a song")
            || lower.Contains("lets write lyrics")
            || lower.Contains("let's write lyrics")
            || lower.Contains("make a song")
            || lower.Contains("karaoke")
        )
        {
            return chat.RandomChoice(
                "oh no this never ends well honestly",
                "go on then superstar",
                "this is already becoming chaotic honestly",
                "i already regret agreeing to karaoke",
                "if auntie janice joins in i'm leaving",
                "alright but i'm not hitting the high notes"
            );
        }

        if (
            lower.Contains("lyrics")
            || lower.Contains("verse")
            || lower.Contains("chorus")
            || lower.Contains("beat")
            || lower.Contains("music")
        )
        {
            return chat.RandomChoice(
                "this sounds like the start of a terrible band honestly",
                "we're absolutely getting booed off stage",
                "you better not make me rap honestly",
                "suddenly i'm emotionally invested in this song",
                "we'd either go viral or get banned honestly"
            );
        }

        if (singingExchanges >= 3)
        {
            chat.ChangeState(new CasualState(chat));

            return chat.RandomChoice(
                "right that's enough musical suffering honestly",
                "my imaginary ears need a break",
                "anyway before this becomes a concert",
                "we are absolutely not starting a band"
            );
        }

        return chat.brain.GetSingingReply();
    }
}