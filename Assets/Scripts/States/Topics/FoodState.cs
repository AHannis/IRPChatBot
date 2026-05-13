using UnityEngine;

public class FoodState : ChatState
{
    public FoodState(ChatManager manager) : base(manager)
    {
    }

    public override string HandleInput(string input)
    {
        string lower = input.ToLower();

        if (chat.analyser.ContainsFuzzy(lower, "cake", "crisps", "cookies"))
        {
            chat.ChangeState(new CasualState(chat));
            return "i'm sure i can get that in for you";
        }

        return "that sounds questionable honestly";
    }
}