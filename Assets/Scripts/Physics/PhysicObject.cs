using UnityEngine;

public class PhysicObject : MonoBehaviour, IPhysicHittable
{
  private Rigidbody _rigidbody = null; // Компонент Rigidbody на физическом объекте

  // Реализуем метод Hit() из интерфейса
  public void Hit(Vector3 force, Vector3 position)
  {
    CheckRigidbody();                                                  // Вызываем метод CheckRigidbody()
    _rigidbody.AddForceAtPosition(force, position, ForceMode.Impulse); // Применяем к Rigidbody силу force в позиции position
  }

  // Проверяем наличие Rigidbody
  private void CheckRigidbody()
  {
    if (!_rigidbody) {                        // Если переменная _rigidbody не задана
      _rigidbody = GetComponent<Rigidbody>(); // Присваиваем ей Rigidbody текущего объекта
    }
  }
}