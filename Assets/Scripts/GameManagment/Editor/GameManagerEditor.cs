using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private bool isApplicationPlaying;
    public override async void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        isApplicationPlaying = Application.isPlaying;
        GameManager gameManager = (GameManager)target;

        string header = isApplicationPlaying ? $"Сейчас мы на {GameStateManager.CurrentState.StateName}, переключиться на другой state:" : "Игра еще не запустилась";

        GUILayout.Label(header, EditorStyles.foldoutHeader);

        if(isApplicationPlaying)
        {

            if (GUILayout.Button("Go to Main Menu"))
                await GameStateManager.Instance.ChangeStateAsync("MainMenu");

            if (GUILayout.Button("Go to Game State"))
                await GameStateManager.Instance.ChangeStateAsync("Game");
            

        }


    }
}
