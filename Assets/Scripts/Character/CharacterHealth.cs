using System;
using UnityEngine;

public abstract class CharacterHealth : CharacterPart
{
  [SerializeField] private int _startHealthPoints = 100; // Стартовое количество здоровья

  public Action                  OnDie;             // Событие при смерти
  public Action<CharacterHealth> OnDieWithObject;   // Доп. событие при смерти Со ссылкой на метод с параметром типа CharacterHealth
  public Action                  OnAddHealthPoints; // Событие при изменении очков здоровья

  private int  _healthPoints; // Очки здоровья персонажа
  private bool _isDead;       // Флаг смерти персонажа
  // Добавляем очки здоровья
  public void AddHealthPoints(int value)
  {
    // Если персонаж мёртв
    if (_isDead) { return; } // Выходим из метода
     
    _healthPoints += value;                            // Увеличиваем значение здоровья на value
    Mathf.Clamp(_healthPoints, 0, _startHealthPoints); // Проверяем, что здоровье в пределах от нуля до заданного изначально
    OnAddHealthPoints?.Invoke();                       // Вызываем событие OnAddHealthPoints

    // Если здоровье достигло нуля
    if (_healthPoints == 0) { Die(); } // Вызываем метод Die()
  }
  // Получаем стартовое количество здоровья
  public int GetStartHealthPoints() { return _startHealthPoints; }
  
  // Получаем текущее количество здоровья
  public int GetHealthPoints() { return _healthPoints; } 
  
  // Инициализируем переменные
  protected override void OnInit()
  {
    _healthPoints = _startHealthPoints; // Задаём начальное значение здоровья
    _isDead = false;                    // Ставим флаг в значение «живой»
  }
  
  // Обрабатываем смерть персонажа
  private void Die()
  {
    _isDead = true; // Ставим флаг в значение «мёртвый»
    OnDie?.Invoke(); // Вызываем событие OnDie

    // Вызываем событие OnDieWithObject
    // И передаём в него информацию о персонаже
    // То есть ссылку на объект типа CharacterHealth
    OnDieWithObject?.Invoke(this);
  }
}