using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameState : BaseGameState
{
    public override string StateName => "Game";

    public override async Task EnterAsync()
    {
        await base.EnterAsync();
        
        //await AdaptiveAudioManager.Instance.SetMoodAsync(AudioMood.Calm, 0.5f);
        await MenuManager.Instance.OpenMenuAsync("MainMenu");
        // Здесь можно добавить логику для начала игрового процесса, например, запуск таймеров, настройку игровых объектов.
        Debug.Log("Game state has started!");

}

    public override async Task ExitAsync()
    {
        await base.ExitAsync();
        // Здесь можно добавить логику для завершения игрового процесса, например, сохранение состояния игры.
        Debug.Log("Game state has ended!");

    }

    public override async void Update()
    {
        base.Update();


        // Здесь можно добавлять логику обновления игрового процесса, например, проверку на паузу.

    }
}
