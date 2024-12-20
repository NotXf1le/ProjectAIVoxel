using System.Threading.Tasks;

public interface IUIElement
{
    Task ShowAsync();                  // Показать элемент с анимацией
    Task HideAsync();                  // Скрыть элемент с анимацией
    void SetText(string text);         // Установка текста, если элемент поддерживает его
    void UpdatePosition();             // Обновление позиции элемента
    void Reset();                      // Сброс всех параметров элемента
}
