using UnityEngine;    // Используем команды Unity
using TMPro;          // Работаем с элементами TMPro
using UnityEngine.UI; // Используем элементы UI
using System;         // Работаем с событиями

public class RoomScreen : BaseScreen
{
  [SerializeField] private TextMeshProUGUI _roomNameText; // Надпись с названием комнаты
  [SerializeField] private Button _leaveRoomButton; // Кнопка выхода из комнаты

  public Action OnLeaveButtonClick;  // Событие выхода из комнаты
  public void SetRoomNameText(string value) { // Задаём название комнаты
    _roomNameText.text = value;               // Делаем его равным value
  }
  private void Start() {
    _leaveRoomButton.onClick.AddListener(LeaveButtonClick); // Обрабатываем нажатие на кнопку выхода из комнаты
  }
  private void LeaveButtonClick() { // Вызывается при нажатии на кнопку выхода из комнаты
    OnLeaveButtonClick?.Invoke();   // Вызываем событие OnLeaveButtonClick
  }
}