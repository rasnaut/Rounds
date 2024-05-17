using UnityEngine;

// Наследуем DefaultBulletHit от BulletHit
public class DefaultBulletHit : BulletHit
{
  public DefaultBulletHit(int finalHitType, GameObject hitPrefab) : base(finalHitType, hitPrefab) { }

  // Переопределяем метод Hit()
  public override void Hit(Collision collision, Transform bulletTransform)
  {
    bool isCharacterHit = CheckCharacterHit(collision.collider);                                    // Проверяем попадание в персонажа
    CheckPhysicObjectHit(collision.collider, bulletTransform.forward, collision.contacts[0].point); // Проверяем попадание в физический объект

    if (!isCharacterHit) { // Если попали не в персонажа
      Quaternion hitRotation = Quaternion.LookRotation(-bulletTransform.up, -bulletTransform.forward); // Создаём объект с нужным нам поворотом

      // Создаём экземпляр эффекта попадания
      // В точке столкновения с нужным вращением
      GameObject.Instantiate(HitPrefab, collision.contacts[0].point, hitRotation);
    }
  }
}
