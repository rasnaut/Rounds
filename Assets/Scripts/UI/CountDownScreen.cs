using System;
using TMPro;
using UnityEngine;

public class CountDownScreen : BaseScreen
{
  private const int StartCountDownValue = 3;  // Константа начального значения отсчёта

  [SerializeField] private TextMeshProUGUI _countDownText;         // Надпись про обратный отсчёт
  [SerializeField] private float           _secondDelay    = 1.3f; // Задержка между числами в секундах
  [SerializeField] private float           _startTextDelay = 0.3f; // Задержка перед появлением текста

  private float _timer         ; // Таймер обратного отсчёта
  private bool  _isActive      ; // Флаг активности отсчёта
  private int   _countDownValue; // Текущее значение отсчёта
  private float _currentDelay  ; // Текущая задержка

  public Action OnCountDownEnd;  // Событие завершения отсчёта
  public void StartCountDown()
  {
    _isActive = true;                       // Активируем отсчёт
    SetCountDownValue(StartCountDownValue); // Вызываем метод SetCountDownValue()
  }
  private void SetCountDownValue(int value)
  {
    _countDownValue = value; // Запоминаем текущее значение отсчёта

    // Определяем текущую задержку
    _currentDelay = value > 0 ? _secondDelay : _startTextDelay; // В зависимости от значения value

    SetCountDownText(value); // Вызываем метод SetCountDownText()
    
    if (_countDownValue <= -1) { // Если отсчёт окончен
      EndCountDown();            // Вызываем метод EndCountDown()
    }
  }
  private void SetCountDownText(int value)
  {
    _countDownText.text = value > 0 ?  // Отображаем число 
                          $"{value}" : // (значение отсчёта)
                          "Начали!";   // Или слово «Начали!»
  }
  private void Update()
  {
    if (!_isActive) { // Если отсчёт не активен
      return;         // Выходим из метода
    }
    TimerTick();      // Вызываем метод TimerTick()
  }
  private void TimerTick()
  {
    // Увеличиваем значение таймера
    // На время, прошедшее между кадрами
    _timer += Time.deltaTime;

    if (_timer >= _currentDelay) {        // Если текущее время превысило время задержки
      _timer -= _currentDelay;            // Уменьшаем значение таймера На значение задержки
      _countDownValue--;                  // Уменьшаем значение отсчёта
      SetCountDownValue(_countDownValue); // Вызываем метод SetCountDownValue()
    }
  }
  private void EndCountDown() {
    OnCountDownEnd?.Invoke(); // Вызываем событие OnCountDownEnd
  }
}
