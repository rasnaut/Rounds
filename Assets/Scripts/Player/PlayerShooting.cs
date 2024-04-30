using UnityEngine;

public class PlayerShooting : CharacterShooting
{
  [SerializeField] protected bool _autoReloading = true; // Флаг автоматической перезарядки
protected override void OnInit()  { // Заполняем ссылки на компоненты
    base.OnInit(); // Инициализируем объект базового класса То есть CharacterShooting
  }
  protected override void Shooting() { // Начинаем стрелять
    if (Input.GetMouseButton(0)) {     // Если нажата левая кнопка мыши
      Shoot();                         // Вызываем метод Shoot()
      AutoReloading();                 // Вызываем метод AutoReloading()
    }
  }
  protected override void Reloading() { // Перезаряжаем оружие
    if ((!CheckHasBulletsInRow()        // Если нет пуль
      && Input.GetMouseButton(0))       // и нажата левая кнопка мыши
      || Input.GetKeyDown(KeyCode.R)) { // Или нажата клавиша R
      Reload();                         // Вызываем метод Reload()
    }
  }
  private void AutoReloading() { // Происходит перезарядка
    if (!_autoReloading) {       // Если перезарядка не нужна
      return;                    // Выходим из метода
    }
    if (!CheckHasBulletsInRow()) { // Если закончились пули
      Reload();                    // Вызываем метод Reload()
    }
  }
}