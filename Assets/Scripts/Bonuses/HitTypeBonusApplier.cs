using System.Collections.Generic;
using UnityEngine;

// Добавили наследование от BonusApplier
public class HitTypeBonusApplier : MonoBehaviour, BonusApplier
{
  [SerializeField] private GameObject _defaultHitPrefab  ; // Префаб эффекта обычного попадания
  [SerializeField] private GameObject _explosionHitPrefab; // Префаб эффекта попадания со взрывом
  public void ApplyBonus(List<BonusType> existingBonusTypes, GameObject root) // Применяем бонусы к пуле
  {
    // Получаем объект типа попадания
    // С учётом собранных игроком бонусов
    BulletHit hit = GetHit(existingBonusTypes);
    Apply(hit, root);  // Применяем объект типа попадания к исходному объекту
  }
  private BulletHit GetHit(List<BonusType> existingBonusTypes) // Получаем объект типа попадания
  {
    int finalHitType = 0;                                // Задаём начальную силу взрыва
    for (int i = 0; i < existingBonusTypes.Count; i++) { // Проходим по собранным игроком бонусам
      int hitType = 0;                                   // Задаём переменную для типа попадания
      switch (existingBonusTypes[i])                     // Выбираем действие в зависимости от типа бонуса
      {
        case BonusType.SmallExplosionHit : hitType = 1; break; // Если бонус — слабый взрыв
        case BonusType.MediumExplosionHit: hitType = 2; break; // Если бонус — средний взрыв
        case BonusType.LargeExplosionHit : hitType = 3; break; // Если бонус — сильный взрыв
      }

      if (finalHitType < hitType)  { // Если начальная силы взрыва < текущей
        finalHitType = hitType; // Приравниваем начальное значение к текущему
      }
    }
    
    BulletHit hit = null; // Создаём объект типа попадания
    if (finalHitType == 0) {                                           // Если взрывной бонус отсутствует
      hit = new DefaultBulletHit(finalHitType, _defaultHitPrefab);     // Создаём обычное попадание
    } else {                                                           // Иначе
      hit = new ExplosionBulletHit(finalHitType, _explosionHitPrefab); // Создаём попадание со взрывом
    }
    return hit; // Возвращаем объект типа попадания
  }
  private void Apply(BulletHit hit, GameObject root) // Применяем объект типа попадания к исходному объекту
  {
    // Ищем все компоненты в дочерних объектах root
    // Которые зависят от типа попадания
    IHitTypeBonusDependent[] dependents = root.GetComponentsInChildren<IHitTypeBonusDependent>();
    for (int i = 0; i < dependents.Length; i++) { // Проходим по найденным компонентам
      dependents[i].SetHit(hit);                  // Вызываем для каждого метод SetHit()
    }
  }
}