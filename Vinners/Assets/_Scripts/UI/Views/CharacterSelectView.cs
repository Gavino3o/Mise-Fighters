using UnityEngine;
using UnityEngine.UI;
using FishNet;

public class CharacterSelectView : View
{
    [SerializeField] private Button redButton;
    [SerializeField] private Button greenButton;

    public override void Initialise()
    {
        redButton.onClick.AddListener(() =>
        {
            
        });

        greenButton.onClick.AddListener(() =>
        {
            
        });

        base.Initialise();

    }

    public override void Show(object args = null)
    {
        if (args is string message)
        {
            Debug.Log(message);
        }
        base.Show(args);
    }
}
