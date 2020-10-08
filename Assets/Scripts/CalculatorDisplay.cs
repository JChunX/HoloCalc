using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalculatorDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _TextField;

    public delegate void OperandEventHandler(string input); // Delegate is a method prototype for handling events
    public event OperandEventHandler ReceivedOperand; // The event

    public delegate void OperatorEventHandler(string input);
    public event OperatorEventHandler ReceivedOperator;

    public delegate void ResetEventHandler();
    public event ResetEventHandler ReceivedReset;

    private string _curOperand;
    private string _prevInput;

    // Start is called before the first frame update
    void Start()
    {
        _TextField.SetText("0");
        _curOperand = "0";
        _prevInput = "start";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Put(string input) 
    {
        switch (input)
        {
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
            case "0":
                if (input == "0" && _curOperand == "0")
                {
                    break;
                }
                else if (input != "0" && _curOperand == "0")
                {
                    _curOperand = input;
                }
                else if (_curOperand.Length < 8)
                {
                    _curOperand = _curOperand + input;
                }

                _TextField.SetText(_curOperand);

                if (input == "0")
                {
                    _prevInput = input;
                }
                else
                {
                    _prevInput = "num";
                }
                break;
            case "+":
            case "-":
            case "*":
            case "/":
            case "=":
                if (_prevInput != "+" && _prevInput != "-" && _prevInput != "*" && _prevInput != "/" && _prevInput != "=")
                {
                    if (_prevInput == "num" || _prevInput == "0" || _prevInput == "start") // check if operating on user entered number, else do not push current operand (already on outstack)
                    {
                        ReceivedOperand?.Invoke(_curOperand);   // Event usage: invoke event handler
                    }
                    ReceivedOperator?.Invoke(input);
                    _curOperand = "0";
                    _prevInput = input;
                }

                else if (_prevInput.Equals("="))
                {
                    ReceivedOperator?.Invoke(input);
                    _prevInput = input;
                }
                break;
            case "(":
                if (_prevInput == "num" || _prevInput == "0")
                {
                    ReceivedOperand?.Invoke(_curOperand);
                    ReceivedOperator?.Invoke(input);
                    _prevInput = input;
                    break;
                }
                if (_prevInput != "+" || _prevInput != "-" || _prevInput != "*" || _prevInput != "/" || _prevInput == "start")
                {
                    ReceivedOperator?.Invoke(input);
                    _curOperand = "0";
                    _prevInput = input;
                }
                break;
            case ")":
                if (_prevInput == "num")
                {
                    ReceivedOperand?.Invoke(_curOperand);
                    ReceivedOperator?.Invoke(input);
                    _prevInput = input;
                    break;
                }
                if (_prevInput == ")")
                {
                    ReceivedOperator?.Invoke(input);
                    _prevInput = input;
                }
                break;
            case "+/-": // not implemented
                break;
            case "c":
                if (_prevInput == "num")
                {
                    Start();
                }
                break;
            case "ac":
                ReceivedReset?.Invoke();
                Start();
                break;
        }
    }

    public void Out(string output)
    {
        _curOperand = output;
        _TextField.SetText(output);
    }

    public void Clear()
    {
        _TextField.SetText("0");
    }

}
