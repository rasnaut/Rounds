using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
}

public enum BonusType // Перечисление типов бонусов
{
  OneShoot    = 1,    // Обычный выстрел
  DoubleShoot = 2,    // Двойной выстрел
  TripleShoot = 3,    // Тройной выстрел
  QuadrupleShoot = 4, // Четверной выстрел

  SmallExplosionHit,  // Слабый взрывной удар
  MediumExplosionHit, // Средний взрывной удар
  LargeExplosionHit,  // Сильный взрывной удар
}