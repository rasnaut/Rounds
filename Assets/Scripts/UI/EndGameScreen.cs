using UnityEngine;
using UnityEngine.UI;
using System;
public class EndGameScreen : BaseScreen
{
  // Кнопка выхода из игры
  [SerializeField] private Button _leaveButton;

  // Событие нажатия на кнопку выхода
  public Action OnLeaveButtonClick;

  // Вызывается при запуске игры
  private void Start()
  {
    // Обрабатываем нажатие на кнопку выхода
    _leaveButton.onClick.AddListener(LeaveButtonClick);
  }
  // Вызывается при нажатии на кнопку выхода
  private void LeaveButtonClick()
  {
    // Вызываем событие OnLeaveButtonClick
    OnLeaveButtonClick?.Invoke();
  }
}