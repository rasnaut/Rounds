using UnityEngine;

public abstract class BulletHit
{
  protected GameObject HitPrefab;    // Префаб, который будет создан при попадании пули
  protected int        FinalHitType; // Тип попадания
  protected int        Damage;       // Урон от пули
  protected float      Impulse;      // Импульс пули

  // Конструктор класса BulletHit
  // Принимает тип попадания и префаб
  public BulletHit(int finalHitType, GameObject hitPrefab)
  {
    FinalHitType = finalHitType; // Задаём тип попадания
    HitPrefab    = hitPrefab;    // Задаём префаб
  }
  // Инициализируем переменные
  public void Init(int damage, float impulse) {
    Damage  = damage;  // Задаём урон
    Impulse = impulse; // Задаём импульс
  }

  public abstract void Hit(Collision collision, Transform bulletTransform);
  protected bool CheckCharacterHit(Collider collider)
  {
    CharacterHealth hitedHealth = collider.GetComponentInParent<CharacterHealth>(); // Получаем CharacterHealth у объекта столкновения
    if (hitedHealth) {                      // Если у объекта есть компонент здоровья
      hitedHealth.AddHealthPoints(-Damage); // Наносим урон персонажу
      return true; // Возвращаем true
    }
    return false; // Возвращаем false
  }
  protected bool CheckPhysicObjectHit(Collider collider, Vector3 direction, Vector3 point)
  {
    // Получаем IPhysicHittable у объекта столкновения
    IPhysicHittable hittedPhysicObject = collider.GetComponentInParent<IPhysicHittable>();
    if (hittedPhysicObject != null) {     // Если пуля столкнулась с физическим объектом
      // Вызываем у объекта метод Hit()
      // Передаём в него вектор направления и точку столкновения
      hittedPhysicObject.Hit(direction * Impulse, point);

      return true; // Возвращаем true
    }
    return false; // Возвращаем false
  }
}