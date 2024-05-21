using TMPro;
using UnityEngine;

public class PlayerListElement : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI        _playerNameText; // Надпись с именем игрока
                   private Photon.Realtime.Player _player;         // Информация об игроке от Photon

  public void SetPlayer(Photon.Realtime.Player value) // Задаём данные игрока
  {
    _player              = value;          // Делаем информацию об игроке равной value
    _playerNameText.text = value.NickName; // Делаем имя игрока равным value.NickName
  }
  public bool CheckPlayer(Photon.Realtime.Player player) // Проверяем данные игрока
  {
    // Возвращаем true, если заданный игрок равен _player
    // Иначе возвращаем false
    return player == _player;
  }
}
