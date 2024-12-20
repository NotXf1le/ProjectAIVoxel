using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : BaseGameState
{
    public override string StateName => "MainMenu";

    public override async Task EnterAsync()
    {
        await base.EnterAsync();

        // Логика загрузки главного меню (например, активация UI, начало анимаций).
        Debug.Log("Main Menu state entered.");
    }

    public override async Task ExitAsync()
    {
        await base.ExitAsync();
        // Логика очистки главного меню (например, деактивация UI, остановка анимаций).
        Debug.Log("Main Menu state exited.");
    }

    public override async void Update()
    {
        base.Update();



        // Логика для главного меню, например, обработка ввода от пользователя.
    }
}
