using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _player1KeyUp;
    [SerializeField] private TMP_InputField _player1KeyDown;
    [SerializeField] private TMP_InputField _player2KeyUp;
    [SerializeField] private TMP_InputField _player2KeyDown;
    private List<TMP_InputField> ChangedInputs = new List<TMP_InputField>();

    void Start()
    {
        _player1KeyUp.text = PlayerInputs._axis[PlayerSide.LEFT].up;
        _player1KeyDown.text = PlayerInputs._axis[PlayerSide.LEFT].down;
        _player2KeyUp.text = PlayerInputs._axis[PlayerSide.RIGHT].up;
        _player2KeyDown.text = PlayerInputs._axis[PlayerSide.RIGHT].down;
    }

    public void Apply()
    {
        PlayerInputs.ModifyUpKey(PlayerSide.LEFT, _player1KeyUp.text);
        PlayerInputs.ModifyDownKey(PlayerSide.LEFT, _player1KeyDown.text);
        PlayerInputs.ModifyUpKey(PlayerSide.RIGHT, _player2KeyUp.text);
        PlayerInputs.ModifyDownKey(PlayerSide.RIGHT, _player2KeyDown.text);
    }
}
