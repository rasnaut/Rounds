using UnityEngine;    // Используем команды Unity
using TMPro;          // Работаем с элементами TMPro
using UnityEngine.UI; // Используем элементы UI
using System;         // Работаем с событиями

public class RoomListElement : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _roomNameText; // Надпись с названием комнаты
  [SerializeField] private Button          _joinButton;   // Кнопка входа в комнату

  public Action<string> OnJoinButtonClick; // Событие входа в комнату
  
  private string _roomName; // Название комнаты
  public void SetRoomName(string value) // Задаём название комнаты
  {
    _roomName          = value; // Делаем его равным value
    _roomNameText.text = value; // Задаём текст надписи
  }
  private void Start() // Вызывается при запуске игры
  {
    _joinButton.onClick.AddListener(JoinButtonClick); // Обрабатываем нажатие на кнопку входа в комнату
  }
  private void JoinButtonClick() // Вызывается при нажатии на кнопку входа в комнату
  {
    // Вызываем событие OnJoinButtonClick
    OnJoinButtonClick?.Invoke(_roomName); // Передаём в него название комнаты
  }
}