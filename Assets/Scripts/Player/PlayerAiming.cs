using UnityEngine;

public class PlayerAiming : CharacterAiming
{
  
  private Camera _mainCamera; // Главная камера

  // Инициализируем переменные
  protected override void OnInit() {
     base.OnInit();            // Инициализируем объект базового класса  То есть CharacterAiming
    _mainCamera = Camera.main; // Присваиваем _mainCamera объект камеры
  }
  // Вызывается каждый кадр
  private void Update()
  {
    if (!IsActive) { // Если игрок не активен
      return; // Выходим из метода
    }
    Aiming(); // Вызываем метод Aiming()
  }
  // Управляем прицеливанием игрока
  private void Aiming()
  {
    // Вычисляем разницу в координате Z
    float characterZDelta = transform.position.z - _mainCamera.transform.position.z; // Между персонажем и камерой

    // Преобразуем позицию курсора мыши из экранных координат в мировые
    Vector3 mouseInWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * characterZDelta);

    Weapon.transform.LookAt(mouseInWorldPosition); // Поворачиваем оружие в сторону курсора 
  }
}