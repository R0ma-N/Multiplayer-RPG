using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    int _level = 1, _statPoints;
    float _exp, _nextLevelExp = 100;

    StatsManager _manager;
    public StatsManager manager
    {
        set
        {
            _manager = value;
            _manager.exp = _exp;
            _manager.nextLevelExp = _nextLevelExp;
            _manager.level = _level;
            _manager.statPoints = _statPoints;
        }
    }

    public bool RemoveStatPoint()
    {
        if (_statPoints > 0)
        {
            _statPoints--;
            if (_manager != null) _manager.statPoints = _statPoints;
            return true;
        }
        return false;
    }

    public void AddExp(float addExp)
    {
        _exp += addExp;
        while (_exp >= _nextLevelExp)
        {
            _exp -= _nextLevelExp;
            LevelUP();
        }
        if (_manager != null)
        {
            _manager.exp = _exp;
            _manager.level = _level;
            _manager.nextLevelExp = _nextLevelExp;
            _manager.statPoints = _statPoints;
        }
    }

    private void LevelUP()
    {
        _level++;
        _nextLevelExp += 100f;
        _statPoints += 3;
    }

}
