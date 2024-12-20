using System;
using System.Threading.Tasks;

public interface IGameState
{
    string StateName { get; }  // Имя состояния
    Task EnterAsync();          // Вход в состояние (асинхронно)
    Task ExitAsync();           // Выход из состояния (асинхронно)
    void Update();              // Логика обновления состояния
    void OnPause();             // Обработка паузы
    void OnResume();            // Обработка восстановления
}