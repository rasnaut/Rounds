using System.Collections.Generic;
using UnityEngine;

// Сделали класс абстрактным
// Наследуем его от CharacterPart
public abstract class CharacterBonuses : CharacterPart
{
  [SerializeField] private List<BonusType> _existingBonusTypes; // Список активированных бонусов

  private List<BonusApplier> _bonusAppliers = new List<BonusApplier>() {   // Массив объектов применения бонусов
    new ShootCountBonusApplier() // Создаём экземпляр объекта Для подсчёта количества выстрелов
  };
  // Добавляем бонус в список активированных
  public void AddBonus(BonusType type) {
    _existingBonusTypes.Add(type); // Вызываем у списка метод Add()
  }
  protected override void OnInit() {
    // Добавляем в список компоненты типа BonusApplier
    // Из дочерних объектов текущего объекта бонусов персонажа
    _bonusAppliers.AddRange(GetComponentsInChildren<BonusApplier>());

    for (int i = 0; i < _bonusAppliers.Count; i++) {                 // Проходим по объектам применения бонусов
      _bonusAppliers[i].ApplyBonus(_existingBonusTypes, gameObject); // Вызываем у каждого метод ApplyBonus()
    }
  }
}