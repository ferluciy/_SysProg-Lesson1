using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int _health;
    private bool _isHealing;

    private void Start()
    {
        ReceiveHealing();
    }

    public void ReceiveHealing()
    {
        StartCoroutine(Healing(3f, 0.1f, 5, 100));
    }

    private IEnumerator Healing(float time, float interval, int hp, int maxHp)
    {
        if (_isHealing) yield break;

        _isHealing = true;
        float currentTime = 0f;
        
        while ((_health < maxHp) && currentTime < time)
        {
            _health += hp;
            currentTime += interval;
            yield return new WaitForSeconds(interval);
        }
        if (_health >= maxHp) _health = maxHp;
        _isHealing = false;
        yield return null;
    }
}
