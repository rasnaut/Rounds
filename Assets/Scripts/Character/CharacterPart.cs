using System;
using UnityEngine;

public abstract class CharacterPart : MonoBehaviour
{
  protected bool IsActive; // Флаг активности части

  // Инициализируем переменные
  public void Init() {
    IsActive = true; // Делаем часть активной
    OnInit();        // Вызываем метод OnInit()
  }

  // Останавливаем часть персонажа
  public void Stop() {
    IsActive = false; // Делаем часть неактивной
    OnStop();         // Вызываем метод OnStop()
  }

  public void Action()
  {
    if(IsActive)
    {
      OnAction();
    }
  }
  
  protected virtual void OnInit() { } // Защищённый виртуальный метод OnInit()
  protected virtual void OnStop() { } // Защищённый виртуальный метод OnStop()
  protected virtual void OnAction() { } // Защищённый виртуальный метод OnStop()
}
