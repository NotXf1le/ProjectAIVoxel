using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveState : BaseGameState
{
    public override string StateName => "SaveState";

    public override async Task EnterAsync()
    {
        await base.EnterAsync();
        await MenuManager.Instance.OpenMenuAsync("SaveMenu");
        // Здесь можно добавить логику для начала игрового процесса, например, запуск таймеров, настройку игровых объектов.

    }

    public override async Task ExitAsync()
    {
        await base.ExitAsync();
        // Здесь можно добавить логику для завершения игрового процесса, например, сохранение состояния игры.
        await MenuManager.Instance.OpenMenuAsync("MainMenu");


    }

    public override async void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Escape))
            GameStateManager.Instance.ChangeState("Game");

        // Здесь можно добавлять логику обновления игрового процесса, например, проверку на паузу.

    }
}
