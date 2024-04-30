using UnityEngine;

public abstract class Character : MonoBehaviour
{
  private CharacterPart[] _parts; // Массив частей персонажа

  // Вызываем иницилизацию при старте
  private void Start() { Init(); }
  
  // Заполняем ссылки на компоненты
  private void Init()
  {
    _parts = GetComponents<CharacterPart>(); // Получаем компоненты частей персонажа

    // Проходим по всем частям
    for (int i = 0; i < _parts.Length; i++) { _parts[i].Init(); } // Вызываем у каждой метод Init()
    
    InitDeath(); // Вызываем метод InitDeath()
  }

  // Инициализируем гибель персонажа
  private void InitDeath()
  {
    /*for (int i = 0; i < _parts.Length; i++) {    // Проходим по всем частям
      if (_parts[i] is CharacterHealth health) { // Если это часть здоровья персонажа
        health.OnDie += Stop;
      }
    }*/
    var health = GetPart<CharacterHealth>();  // Получаем компонент здоровья персонажа 
    health.OnDie += Stop;                     // Обрабатываем событие OnDie без цикла
  }

  // Получаем часть персонажа по типу T
  // То есть ограничиваем метод так, что он работает
  // Только с объектами, которые наследуются от CharacterPart
  private T GetPart<T>() where T : CharacterPart
  {
    for (int i = 0; i < _parts.Length; i++) { // Проходим по всем частям
      if (_parts[i] is T part) {              // Если текущая часть соответствует типу T
        return part;                          // Возвращаем эту часть
      }
    }
    return null; // Возвращаем пустое значение
  }

  // Останавливаем персонажа
  private void Stop()
  {
    for (int i = 0; i < _parts.Length; i++) { // Проходим по всем частям 
      _parts[i].Stop();                       // Вызываем у каждой метод Stop()
    }
  }
}