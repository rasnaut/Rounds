public abstract class CharacterShooting : CharacterPart
{
  private Weapon _weapon; // Оружие персонажа
  protected abstract void Shooting (); // Защищённый абстрактный метод Shooting()
  protected abstract void Reloading(); // Защищённый абстрактный метод Reloading()
  protected override void OnInit() { // Заполняем ссылки на компоненты
    _weapon = GetComponentInChildren<Weapon>(); // Получаем Weapon из дочерних объектов
    _weapon.Init(); // Вызываем у оружия метод Init()
  }
  protected void Shoot() { _weapon.Shoot(); } // Вызываем у оружия метод Shoot()
  protected bool CheckHasBulletsInRow() {  // Проверяем, есть ли пули в магазине
    return _weapon.CheckHasBulletsInRow(); // Возвращаем результат проверки После вызова одноимённого метода у оружия
  }
  protected void Reload() { // Запускаем перезарядку оружия
    _weapon.Reload();       // Вызываем у оружия метод Reload()
  }
  private void Update() {
    // Если персонаж не активен
    if (!IsActive) {
      return; // Выходим из метода
    }
    Shooting();  // Вызываем метод Shooting()
    Reloading(); // Вызываем метод Reloading()
  }
}