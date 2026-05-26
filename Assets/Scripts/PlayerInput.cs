using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const string SelectButtonName = "Select";

    public event Action<Vector2> SelectClicked;

    private void Update()
    {
        if (Input.GetButtonDown(SelectButtonName))
        {
            SelectClicked?.Invoke(Input.mousePosition);
        }
    }
}