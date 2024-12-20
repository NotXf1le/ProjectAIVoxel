using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseGameState : IGameState
{
    public abstract string StateName { get; }
    protected GameStateManager gameStateManager;

    public BaseGameState()
    {
        gameStateManager = GameStateManager.Instance;
    }

    // Вход в состояние (асинхронно)
    public virtual async Task EnterAsync()
    {
        Debug.Log($"{StateName} entered.");
        await Task.CompletedTask; // Здесь можно добавлять асинхронную логику, например, загрузку ресурсов.
    }

    // Выход из состояния (асинхронно)
    public virtual async Task ExitAsync()
    {
        Debug.Log($"{StateName} exited.");
        await Task.CompletedTask; // Здесь можно добавлять асинхронную логику, например, очистку ресурсов.
    }

    // Логика обновления
    public virtual void Update()
    {
        // Можно переопределить в дочерних классах для обновления конкретного состояния
    }

    // Пауза состояния
    public virtual void OnPause()
    {
        Debug.Log($"{StateName} paused.");
    }

    // Возвращение из паузы
    public virtual void OnResume()
    {
        Debug.Log($"{StateName} resumed.");
    }
}
