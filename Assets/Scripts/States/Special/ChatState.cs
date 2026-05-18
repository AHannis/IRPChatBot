using UnityEngine;

// base class all conversation states inherit from
public abstract class ChatState
{
    protected ChatManager chat;

    public ChatState(
        ChatManager manager
    )
    {
        chat = manager;
    }

    // optional enter hook
    public virtual void Enter()
    {
    }

    // every state must handle user input
    public abstract string HandleInput(
        string input
    );

    // optional exit hook
    public virtual void Exit()
    {
    }
}