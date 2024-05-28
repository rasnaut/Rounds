using System;
using UnityEngine;

// Атрибут [Serializable]
// Позволит классу отображаться в инспекторе Unity
[Serializable]

// Убрали наследование от MonoBehaviour
public class HitEffectProperties
{
  // Координаты точки попадания
  [SerializeField] private Vector3 _point;

  // Поворот эффекта попадания
  [SerializeField] private Quaternion _rotation;

  // Свойство для доступа к координатам точки
  public Vector3 Point => _point;

  // Свойство для доступа к повороту
  public Quaternion Rotation => _rotation;

  // Объявляем свойства при попадании
  public HitEffectProperties(Vector3 point, Quaternion rotation)
  {
    // Задаём координаты точки
    _point = point;

    // Задаём поворот
    _rotation = rotation;
  }
}