using System;
using System.Diagnostics;
using Photon.Pun;

public abstract class CharacterPart : MonoBehaviourPunCallbacks
{
  protected bool IsActive; // Флаг активности части

  private PhotonView _photonView; // Переменная для работы с сетевым представлением объекта

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
    if(IsActive) {
      OnAction();
    }
  }
  public void Activate() {
    IsActive = true;      // Делаем часть активной
  }
  protected PhotonView PhotonView // Свойство для доступа к _photonView Из дочерних классов
  {
    get {
      if (!_photonView) {                         // Если _photonView не инициализирована
        _photonView = GetComponent<PhotonView>(); // Получаем компонент PhotonView
      }
      if (!_photonView)
        Debug.WriteLine("PhotonView not exist!");
      return _photonView; // Возвращаем полученное значение
    } 
  }
  protected virtual void OnInit() { } // Защищённый виртуальный метод OnInit()
  protected virtual void OnStop() { } // Защищённый виртуальный метод OnStop()
  protected virtual void OnAction() { } // Защищённый виртуальный метод OnStop()
}
