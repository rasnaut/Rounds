using UnityEngine;    // Используем команды Unity
using TMPro;          // Работаем с элементами TMPro
using UnityEngine.UI; // Используем элементы UI
using System;         // Работаем с событиями

public class RoomScreen : BaseScreen
{
  [SerializeField] private TextMeshProUGUI _roomNameText;    // Надпись с названием комнаты
  [SerializeField] private Button          _leaveRoomButton; // Кнопка выхода из комнаты
  [SerializeField] private Button          _playButton;      // Кнопка начала игры
  [SerializeField] private PlayerListView  _playerListView;  // Отображение списка игроков
  public PlayerListView PlayerListView => _playerListView; // Свойство для доступа к списку игроков

  public Action OnPlayButtonClick;   // Событие нажатия на кнопку начала
  public Action OnLeaveButtonClick;  // Событие выхода из комнаты
  public void SetRoomNameText(string value) { // Задаём название комнаты
    _roomNameText.text = value;               // Делаем его равным value
  }
  public void SetActivePlayButton(bool value) { // Задаём активность кнопки начала игры
    _playButton.gameObject.SetActive(value);    // Делаем её равной value
  }
  
  private void Start() {
    _leaveRoomButton.onClick.AddListener(LeaveButtonClick); // Обрабатываем нажатие на кнопку выхода из комнаты
    _playButton     .onClick.AddListener(PlayButtonClick);  // НОВОЕ: Обрабатываем нажатие на кнопку начала игры
  }
  private void LeaveButtonClick() { // Вызывается при нажатии на кнопку выхода из комнаты
    OnLeaveButtonClick?.Invoke();   // Вызываем событие OnLeaveButtonClick
  }
  private void PlayButtonClick() { // Вызывается при нажатии на кнопку начала игры
    OnPlayButtonClick?.Invoke();   // Вызываем событие OnPlayButtonClick
  }
}