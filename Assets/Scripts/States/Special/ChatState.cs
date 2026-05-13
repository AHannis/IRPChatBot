using UnityEngine;

public abstract class ChatState
{
    protected ChatManager chat;

    public ChatState(ChatManager manager)
    {
        chat = manager;
    }

    public virtual void Enter() { }

    public abstract string HandleInput(string input);

    public virtual void Exit() { }
}