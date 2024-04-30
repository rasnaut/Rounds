using System.Collections.Generic;
using UnityEngine;

// Сделали класс абстрактным
// Наследуем его от CharacterPart
public abstract class CharacterBonuses : CharacterPart
{
  [SerializeField] private List<BonusType> _existingBonusTypes; // Список активированных бонусов
  
  private BonusApplier[] _bonusAppliers = new BonusApplier[] {   // Массив объектов применения бонусов
    new ShootCountBonusApplier() // Создаём экземпляр объекта Для подсчёта количества выстрелов
  };
  // Добавляем бонус в список активированных
  public void AddBonus(BonusType type) {
    _existingBonusTypes.Add(type); // Вызываем у списка метод Add()
  }
  protected override void OnInit() {
    for (int i = 0; i < _bonusAppliers.Length; i++) {                // Проходим по объектам применения бонусов
      _bonusAppliers[i].ApplyBonus(_existingBonusTypes, gameObject); // Вызываем у каждого метод ApplyBonus()
    }
  }
}