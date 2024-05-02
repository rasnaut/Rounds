using UnityEngine;

public class ExplosionBulletHit : BulletHit
{
  private const float HitRange = 1f;  // Константа со значением диапазона взрыва
  public ExplosionBulletHit(int finalHitType, GameObject hitPrefab) : base(finalHitType, hitPrefab) { }
  public override void Hit(Collision collision, Transform bulletTransform)
  {
    // Получаем массив коллайдеров объектов
    // Которые находятся в радиусе взрыва
    Collider[] collidersInRange = Physics.OverlapSphere(collision.contacts[0].point, HitRange * FinalHitType);

    CheckCharacterHit(collidersInRange); // Вызываем метод CheckCharacterHit() Передаём в него коллайдеры

    // Вызываем метод CheckPhysicObjectHit()
    // Передаём в него коллайдеры
    // А также трансформу пули и точку столкновения
    CheckPhysicObjectHit(collidersInRange, bulletTransform, collision.contacts[0].point);

    // Создаём экземпляр эффекта попадания
    // В точке столкновения без вращения
    GameObject hitSample = GameObject.Instantiate(HitPrefab, collision.contacts[0].point, Quaternion.identity);

    hitSample.transform.localScale = Vector3.one * FinalHitType; // Изменяем размер взрыва В зависимости от уровня бонуса
  }
  protected bool CheckCharacterHit(Collider[] colliders)
  {
    bool isHit = false;                          // Снимаем флаг попадания
    for (int i = 0; i < colliders.Length; i++) { // Проходим по коллайдерам
      if (CheckCharacterHit(colliders[i]))  {    // Если коллайдер на персонаже
        isHit = true;                            // Ставим флаг попадания
      }
    }
    return isHit;                                // Возвращаем значение флага
  }
  private bool CheckPhysicObjectHit(Collider[] colliders, Transform bulletTransform, Vector3 point)
  {
    bool isHit = false; // Снимаем флаг попадания
    for (int i = 0; i < colliders.Length; i++) { // Проходим по коллайдерам
      // Вычисляем направление от точки взрыва
      // До ближайшей точки на коллайдере
      Vector3 direction = (colliders[i].ClosestPoint(point) - point).normalized;
      if (CheckPhysicObjectHit(colliders[i], direction, point))  { // Если коллайдер на физическом объекте
        isHit = true;                                              // Ставим флаг попадания
      }
    }
    return isHit; // Возвращаем значение флага
  }
}