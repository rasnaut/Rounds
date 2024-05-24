using TMPro;
using UnityEngine;

public class WaitScreen : BaseScreen
{
  [SerializeField] private TextMeshProUGUI _countText;  // ??????? ??? ?????????? ???????
  public void SetCountText(int current, int max)    // ?????? ????? ???????
  {
    _countText.text = $"?????? {current} ?? {max}"; // ?????? ??? ?????? ???????? ??????
  }
}
