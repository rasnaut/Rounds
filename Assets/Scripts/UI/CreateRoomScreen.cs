using UnityEngine;    // Используем команды Unity
using TMPro;          // Работаем с элементами TMPro
using System;         // Используем события
using UnityEngine.UI; // Работаем с элементами UI

public class CreateRoomScreen : BaseScreen
{
  [SerializeField] private TMP_InputField _roomNameInputField;// Поле ввода названия комнаты
  [SerializeField] private Button         _createRoomButton;  // Кнопка создания комнаты
  [SerializeField] private Button         _backButton;        // Кнопка возврата назад

  public Action<string> OnCreateRoomButtonClick; // Событие создания комнаты
  private void Start()
  {
    _createRoomButton.onClick.AddListener(CreateRoomButtonClick); // Обрабатываем нажатие на кнопку создания комнаты
    _backButton.onClick.AddListener(BackButtonClick);             // Обрабатываем нажатие на кнопку возврата назад
  }
  private void CreateRoomButtonClick() // Вызывается при нажатии на кнопку создания комнаты
  {
    if (string.IsNullOrEmpty(_roomNameInputField.text)) {     // Если поле ввода названия комнаты пустое
      _roomNameInputField.text = "Добавьте название комнаты"; // Выводим сообщение об ошибке
      return; // Выходим из метода
    }
    OnCreateRoomButtonClick?.Invoke(_roomNameInputField.text); // Вызываем событие OnCreateRoomButtonClick Передаём в него название комнаты
  }
  private void BackButtonClick() {              // Вызывается при нажатии на кнопку возврата назад
    ScreensController.Current.ShowPrevScreen(); // Отображаем предыдущий экран
  }
}