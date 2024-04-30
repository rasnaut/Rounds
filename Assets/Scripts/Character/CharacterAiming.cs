using UnityEngine;

public abstract class CharacterAiming : CharacterPart
{
  public Weapon _weapon;             // Оружие персонажа
  protected Weapon Weapon => _weapon; // Свойство с данными о текущем оружии

  // Заполняем ссылки на компоненты
  protected override void OnInit() {
    _weapon = GetComponentInChildren<Weapon>(); // Получаем Weapon из дочерних объектов
  }
}