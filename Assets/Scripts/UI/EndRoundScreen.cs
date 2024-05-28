using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EndRoundScreen : BaseScreen
{
  // Константа для текста, который отображается при победе
  private const string WinLabelText = "Победа!";

  // Константа для текста, который отображается при поражении
  private const string LoseLabelTaxt = "Проигрыш...";

  // Текстовый элемент для вывода результата
  [SerializeField] private TextMeshProUGUI _labelText;

  // Кнопка «Сдаться»
  [SerializeField] private Button _giveUpButton;

  // Кнопка «Начало новой игры»
  [SerializeField] private Button _playButton;

  // Событие нажатия на кнопку «Сдаться»
  public Action OnGiveUpButtonClick;

  // Событие нажатия на кнопку «Начало новой игры»
  public Action OnPlayButtonClick;

  // Устанавливаем результат
  public void SetResult(bool lose)
  {
    // Задаём текст в зависимости от исхода игры
    _labelText.text = lose ? LoseLabelTaxt : WinLabelText;
  }
  // Вызывается при запуске игры
  private void Start()
  {
    // Обрабатываем нажатие на кнопку «Сдаться»
    _giveUpButton.onClick.AddListener(GiveUpButtonClick);

    // Обрабатываем нажатие на кнопку «Начало новой игры»
    _playButton.onClick.AddListener(PlayButtonClick);
  }
  // Нажимаем на кнопку «Сдаться»
  private void GiveUpButtonClick()
  {
    // Вызываем событие OnGiveUpButtonClick
    OnGiveUpButtonClick?.Invoke();
  }
  // Нажимаем на кнопку «Начало новой игры»
  private void PlayButtonClick()
  {
    // Вызываем событие OnPlayButtonClick
    OnPlayButtonClick?.Invoke();
  }
}