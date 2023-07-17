using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    public EnemyAI enemy;

    private void Awake()
    {
        Setup(enemy.baseStats.maxHealth, enemy.currHealth);
    }

    private void Update()
    {
        SetHP(enemy.currHealth);
    }

    public void Setup(float max, float curr)
    {
        slider.maxValue = max;
        slider.value = curr;
    }

    public void SetHP(float health)
    {
        slider.value = health;
    }
}
