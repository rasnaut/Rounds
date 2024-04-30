using System.Collections.Generic;
using UnityEngine;

public class ShootCountBonusApplier : BonusApplier
{
// Переопределяем метод ApplyBonus()
  public void ApplyBonus(List<BonusType> existingBonusTypes, GameObject root)
  {
    int finalShootCount = 1;                             // Задаём начальное количество выстрелов
    for (int i = 0; i < existingBonusTypes.Count; i++) { // Проходим по активированным бонусам
      int shootCount = 1;                                // Задаём переменную для подсчёта выстрелов

      switch (existingBonusTypes[i]) // Выбираем действие в зависимости от типа бонуса
      { 
        case BonusType.DoubleShoot   : shootCount = 2; break; // Если бонус — двойной выстрел
        case BonusType.TripleShoot   : shootCount = 3; break; // Если бонус — тройной выстрел
        case BonusType.QuadrupleShoot: shootCount = 4; break; // Если бонус — 4й выстрел
      }

      // Добавлено на случай, если будет несколько бонусов И при этом в порядке не по возрастанию
      if (finalShootCount < shootCount) { // Если начальное число выстрелов < текущего
        finalShootCount = shootCount;     // Приравниваем начальное значение к текущему
      }
    }
    // Получаем компонент, который реализует интерфейс IShootCountBonusDependent
    IShootCountBonusDependent dependent = root.GetComponentInChildren<IShootCountBonusDependent>();

    dependent.SetShootCount(finalShootCount); // Вызываем у него метод SetShootCount()
  }
}