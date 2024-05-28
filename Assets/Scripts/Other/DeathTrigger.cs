using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
  // Вызывается при входе в триггерную зону
  private void OnTriggerEnter(Collider other)
  {
    // Вызываем метод AddDamage()
    // Передаём в него коллайдер объекта
    // То есть персонажа, который вошёл в зону
    AddDamage(other);
  }
  // Наносим урон персонажу
  private void AddDamage(Collider other)
  {
    // Получаем компонент CharacterHealth
    // Родительского объекта персонажа, который вошёл в триггер
    CharacterHealth health = other.GetComponentInParent<CharacterHealth>();

    // Если компонента здоровья нет
    if (!health)
    {
      // Выходим из метода
      return;
    }
    // Уменьшаем количество здоровья до нуля
    // Передавая отрицательное значение текущего здоровья
    health.AddHealthPoints(-health.GetHealthPoints());
  }
}