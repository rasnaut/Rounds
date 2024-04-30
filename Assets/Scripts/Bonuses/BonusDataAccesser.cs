using System.Collections.Generic;
using UnityEngine;

public class BonusDataAccesser : MonoBehaviour
{
  [SerializeField] private BonusData[] _data; // Массив данных о доступных бонусах

  // Получаем случайный бонус Проверив, какие бонусы игрок уже собрал
  public BonusData GetRandomBonus(List<BonusType> existingBonusTypes)
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
}
