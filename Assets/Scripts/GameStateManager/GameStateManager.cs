using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private Dictionary<string, IGameState> states = new Dictionary<string, IGameState>();
    public static IGameState CurrentState {  get; private set; }
    private Stack<IGameState> previousStates = new Stack<IGameState>(); // Поддержка истории состояний

    private static GameStateManager _instance;
    public static GameStateManager Instance => _instance;

    private void Awake()
    {
        // Проверяем, существует ли уже экземпляр, и если нет, то создаем новый
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Объект не уничтожается при смене сцен
        }
        else
        {
            Destroy(gameObject); // Уничтожаем новый экземпляр, если он уже существует
        }
    }

    // Регистрация состояний игры
    public void RegisterState(IGameState gameState)
    {
        if (!states.ContainsKey(gameState.StateName))
        {
            states.Add(gameState.StateName, gameState);
        }
        else
        {
            Debug.LogWarning($"State {gameState.StateName} is already registered.");
        }
    }

    public void ChangeState(string newStateName)
    {
        if (CurrentState != null)
        {
            CurrentState.ExitAsync();
            previousStates.Push(CurrentState); // Сохранение предыдущего состояния
        }

        if (states.ContainsKey(newStateName))
        {
            CurrentState = states[newStateName];
            CurrentState.EnterAsync(); // Вход в новое состояние
        }
        else
        {
            Debug.LogError($"State {newStateName} not found.");
        }
    }
    // Переключение на новое состояние
    public async Task ChangeStateAsync(string newStateName)
    {
        if (CurrentState != null)
        {
            await CurrentState.ExitAsync();
            previousStates.Push(CurrentState); // Сохранение предыдущего состояния
        }

        if (states.ContainsKey(newStateName))
        {
            CurrentState = states[newStateName];
            await CurrentState.EnterAsync(); // Вход в новое состояние
        }
        else
        {
            Debug.LogError($"State {newStateName} not found.");
        }
    }

    // Возвращение к предыдущему состоянию
    public async Task GoBackToPreviousStateAsync()
    {
        if (previousStates.Count > 0)
        {
            var lastState = previousStates.Pop();
            await ChangeStateAsync(lastState.StateName);
        }
        else
        {
            Debug.LogWarning("No previous state to go back to.");
        }
    }

    // Обновление текущего состояния
    private void Update()
    {
        CurrentState?.Update();
    }

    // Пауза игры (приостановка текущего состояния)
    public void PauseCurrentState()
    {
        CurrentState?.OnPause();
    }

    // Восстановление игры (возвращение к текущему состоянию)
    public void ResumeCurrentState()
    {
        CurrentState?.OnResume();
    }
}
