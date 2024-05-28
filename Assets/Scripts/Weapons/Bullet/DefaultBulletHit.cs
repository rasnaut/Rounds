using UnityEngine;

// Наследуем DefaultBulletHit от BulletHit
public class DefaultBulletHit : BulletHit
{
  public DefaultBulletHit(int finalHitType, GameObject hitPrefab) : base(finalHitType, hitPrefab) { }

  // Переопределяем метод Hit()
  public override HitEffectProperties Hit(Collision collision, Transform bulletTransform)
  {
    bool isCharacterHit = CheckCharacterHit(collision.collider);                                    // Проверяем попадание в персонажа
    CheckPhysicObjectHit(collision.collider, bulletTransform.forward, collision.contacts[0].point); // Проверяем попадание в физический объект

    if (!isCharacterHit) { // Если попали не в персонажа
      Quaternion hitRotation = Quaternion.LookRotation(-bulletTransform.up, -bulletTransform.forward); // Создаём объект с нужным нам поворотом

      // НОВОЕ: Возвращаем свойства эффекта попадания
      return new HitEffectProperties(collision.contacts[0].point, hitRotation);
    }
    return null;
  }
  public override void SpawnHitEffect(HitEffectProperties properties) // Переопределяем метод SpawnHitEffect()
  {
    // Если свойства для создания эффекта не переданы
    if (properties == null)
    {
      // Выходим из метода
      return;
    }
    // Создаём объект с визуальными эффектами попадания —
    // В заданной точке и с вращением
    GameObject.Instantiate(HitPrefab, properties.Point, properties.Rotation);
  }
}
