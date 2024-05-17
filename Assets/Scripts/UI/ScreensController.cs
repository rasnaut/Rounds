using System.Collections.Generic;
using UnityEngine;

public class ScreensController : MonoBehaviour
{
  private BaseScreen[] _screens;                                    // Массив экранов
  private Stack<BaseScreen> _prevScreens = new Stack<BaseScreen>(); // Стек предыдущих экранов
  private BaseScreen _currentScreen;                                // Текущий экран
  public static bool HasCurrent => Current; // Свойство для проверки, есть ли объект текущего экрана в игре
  public static ScreensController Current { get; private set; } // Свойство для доступа к текущему экрану

  public void Init()
  {
    Current  = this;                                      // Устанавливаем текущий экран
    _screens = GetComponentsInChildren<BaseScreen>(true); // Получаем все экраны из дочерних объектов
    HideAllScreens();                                     // Вызываем метод HideAllScreens()
  }
  public T ShowScreen<T>(bool insertToPrev = true) where T : BaseScreen
  {
    if (_currentScreen) {              // Если есть текущий экран
      _currentScreen.SetActive(false); // Скрываем его

      if (insertToPrev                           // Если стоит флаг insertToPrev
        && !(_currentScreen is LoadingScreen)) { // И это не экран загрузки
        _prevScreens.Push(_currentScreen);       // Добавляем текущий экран в стек предыдущих 
      }
    }
    
    _currentScreen = GetScreen<T>(); // Получаем экран нужного типа
    _currentScreen.SetActive(true);  // Показываем его
   
    return _currentScreen as T;  // Возвращаем экран как результат
  }
  public T GetScreen<T>() where T : BaseScreen
  {
    for (int i = 0; i < Current._screens.Length; i++) { // Проходим по всем экранам
      if (_screens[i] is T targetScreen) {              // Если найден экран нужного типа
        return targetScreen;                            // Возвращаем его
      }
    }
    return null; // Возвращаем пустое значение
  }

  public void ShowPrevScreen() { ShowScreen(_prevScreens.Pop()); } // Показываем экран из стека предыдущих
  private void HideAllScreens()
  {
    for (int i = 0; i < Current._screens.Length; i++)  { // Проходим по всем экранам
      _screens[i].SetActive(false);                      // Скрываем каждый
    }
  }
  private void ShowScreen(BaseScreen screen)
  {
    _currentScreen.SetActive(false); // Скрываем текущий экран
    _currentScreen = screen;         // Ставим новый текущий экран
    _currentScreen.SetActive(true);  // Показываем его
  }
}

