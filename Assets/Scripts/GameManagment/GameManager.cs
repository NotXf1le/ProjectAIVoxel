using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private async void Start()
    {
        // Регистрируем состояния
        GameStateManager.Instance.RegisterState(new SaveState());
        GameStateManager.Instance.RegisterState(new GameState());
        GameStateManager.Instance.RegisterState(new LoadState());

        // Переключаемся на главное меню
        await GameStateManager.Instance.ChangeStateAsync("Game");



        // Загрузка сцены главного меню (если она еще не загружена)
    }

}
