using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private static Dictionary<string, Menu> menus = new Dictionary<string, Menu>();
    private static Menu currentMenu;

    private static MenuManager _instance;
    public static MenuManager Instance => _instance;

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

    public void RegisterMenu(string name, Menu menu)
    {
        if (!menus.ContainsKey(name))
            menus[name] = menu;
    }

    public void OpenMenu(string name)
    {
        _ = OpenMenuAsync(name);
    }
    
    public async Task OpenMenuAsync(string name)
    {
        if (currentMenu != null)
            await currentMenu.CloseMenuAsync();

        if (menus.TryGetValue(name, out var newMenu))
        {

            currentMenu = newMenu;
            await currentMenu.OpenMenuAsync();
        }
    }

    public async Task CloseCurrentMenuAsync()
    {
        if (currentMenu != null)
        {
            await currentMenu.CloseMenuAsync();
            currentMenu = null;
        }
    }
}
