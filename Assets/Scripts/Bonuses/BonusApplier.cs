using System.Collections.Generic;
using UnityEngine;

public interface BonusApplier
{
  void ApplyBonus(List<BonusType> existingBonusTypes, GameObject root);
}