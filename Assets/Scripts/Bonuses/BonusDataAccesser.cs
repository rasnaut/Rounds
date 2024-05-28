using System.Collections.Generic;
using UnityEngine;

public class BonusDataAccesser : MonoBehaviour
{
  [SerializeField] private BonusData[] _data; // Массив данных о доступных бонусах

  // Получаем список случайных бонусов
  public List<BonusData> GetRandomBonuses(List<BonusType> existingBonusTypes, int targetCount)
  {
    List<BonusData> possibleData  = new List<BonusData>(); // Создаём пустой список для хранения возможных бонусов
    List<BonusType> existingTypes = new List<BonusType>(existingBonusTypes); // Копируем типы собранных бонусов

    // НОВОЕ: Вызываем метод AddLowLevelBonuses()
    // Передаём в него список типов собранных бонусов
    AddLowLevelBonuses(existingTypes);

    for (int i = 0; i < targetCount; i++) {        // Проходим по циклу длиной в количество требуемых бонусов
      if (existingTypes.Count == _data.Length) {   // Если все бонусы уже используются
        break; // Выходим из цикла
      }

      // Получаем случайный бонус Который не входит в список уже собранных
      BonusData randomBonus = GetRandomBonus(existingTypes); 
      existingTypes.Add(randomBonus.Type);
      possibleData .Add(randomBonus); // Добавляем бонус в список
    }
    return possibleData; // Возвращаем список возможных бонусов
  }

  // Получаем случайный бонус Проверив, какие бонусы игрок уже собрал
  private BonusData GetRandomBonus(List<BonusType> existingBonusTypes)
  {
    List<BonusData> possibleData = GetPossibleData(existingBonusTypes); // Получаем возможные бонусы для выбора
    float           sumChance    = GetSumChance   (possibleData);       // Суммируем шансы всех возможных бонусов
    float           rand         = Random.Range   (0, sumChance);       // Выбираем случайное число от 0 до суммы шансов

    for (int i = 0; i < possibleData.Count; i++) { // Проходим по возможным бонусам
      if (rand <= possibleData[i].Chance) {        // Если случайное число <= шансу бонуса
        return possibleData[i];                    // Возвращаем этот бонус
      }
      rand -= possibleData[i].Chance;              // Уменьшаем случайное число на шанс бонуса
    }
    return null; // Возвращаем пустое значение
  }
  // Получаем список ещё не активированных бонусов
  private List<BonusData> GetPossibleData(List<BonusType> existingBonusTypes)
  {
    List<BonusData> possibleData = new List<BonusData>(); // Создаём новый список ещё не активированных бонусов
    for (int i = 0; i < _data.Length; i++) {              // Проходим по всему списку бонусов
      if (!existingBonusTypes.Contains(_data[i].Type)) {  // Если в списке уже активированных бонусов Ещё нет выбранного типа
        possibleData.Add(_data[i]);                       // Добавляем этот бонус в список возможных
      }
    }
    return possibleData; // Возвращаем список возможных бонусов
  }
  // Вычисляем суммарный шанс получения бонусов из списка
  private float GetSumChance(List<BonusData> data) 
  {
    float sumChance = 0;                   // Начинаем с шанса 0
    for (int i = 0; i < data.Count; i++) { // Проходим по всему списку бонусов
      sumChance += data[i].Chance;         // Добавляем шанс каждого бонуса к общему шансу
    }
    return sumChance; // Возвращаем сумму шансов всех бонусов
  }
  private void AddLowLevelBonuses(List<BonusType> bonuses) // Добавляем бонусы низшего уровня
  {
    // Объявляем переменную типа BonusData
    // Для хранения данных о бонусах
    BonusData data;

    // Проходим по всем бонусам в обратном порядке
    for (int i = bonuses.Count - 1; i >= 0; i--)
    {
      // Получаем данные о каждом бонусе по его типу
      data = GetBonusDataByType(bonuses[i]);

      // Проходим по массиву всех данных
      for (int j = 0; j < _data.Length; j++)
      {
        // Если этого типа бонуса нет в списке
        // И он принадлежит той же группе
        // И у него меньший уровень
        if (!bonuses.Contains(_data[j].Type) && _data[j].Group == data.Group && _data[j].Level < data.Level)
        {
          // Добавляем этот тип бонуса в список
          bonuses.Add(_data[j].Type);
        }
      }
    }
  }
  private BonusData GetBonusDataByType(BonusType type) // Получаем данные о бонусе по его типу
  {
    for (int i = 0; i < _data.Length; i++) // Проходим по всем бонусам
    {
      // Если тип очередного бонуса совпадает с указанным
      if (_data[i].Type == type) {
        return _data[i]; // Возвращаем данные этого бонуса
      }
    }
    return null; // Возвращаем пустое значение
  }
}
