using System;
using UnityEngine;

// Атрибут [Serializable]
// Позволит классу отображаться в инспекторе Unity
[Serializable]
public class BonusData
{
  [SerializeField] private float     _chance; // Вероятность получения бонуса
  [SerializeField] private BonusType _type  ; // Тип бонуса из перечисления (enum) BonusType
  [SerializeField] private string    _title ; // НОВОЕ: Название (заголовок) бонуса
  public float     Chance => _chance; // Свойство, которое позволяет узнать вероятность
  public BonusType Type   => _type;   // Свойство, которое позволяет узнать тип бонуса
  public string    Title  => _title;  // НОВОЕ: Свойство для доступа к названию бонуса
}