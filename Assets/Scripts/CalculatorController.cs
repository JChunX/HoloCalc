using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CalculatorController : MonoBehaviour
{
    [SerializeField] private CalculatorDisplay _display;    // Encapsulate event generating class
    private Stack<string> _opStack;
    private Stack<int> _outStack;

    //public delegate void DisplayEventHandler(string output);
    //public event DisplayEventHandler SignaledOutput;

    // Start is called before the first frame update
    void Start()
    {
        _display.ReceivedOperand += HandleOperand;   // Add HandleOperand as listener to ReceivedOperand event
        _display.ReceivedOperator += HandleOperator; // Similar
        _display.ReceivedReset += Reset;
        _opStack = new Stack<string>();
        _outStack = new Stack<int>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Reset()
    {
        _opStack = new Stack<string>();
        _outStack = new Stack<int>();
    }

    void OnDestory()
    {
        _display.ReceivedOperand -= HandleOperand;  // Remove listener on destroy
        _display.ReceivedOperator -= HandleOperator;
    }

    void HandleOperand(string input)    // Operand event handler
    {
        //Debug.LogFormat("Operand: {0}", input);
        _outStack.Push(Int32.Parse(input));
    }

    void HandleOperator(string input)
    {
        //Debug.LogFormat("Operator: {0}", input);

        if (input.Equals("("))
        {
            _opStack.Push(input);
            return;
        }

        if (input.Equals("="))
        {
            while (_opStack.Count != 0)
            {
                ToOut(_opStack.Pop());
            }
            return;
        }

        _display.Out("0");

        if (input.Equals(")") && _opStack.Contains("("))
        {
            while (!(_opStack.Peek().Equals("(")))
            {
                ToOut(_opStack.Pop());
            }
            _opStack.Pop(); // Pop left parenthesis
        }

        if (input.Equals("*") || input.Equals("/"))
        {
            while(_opStack.Count != 0 
                && !(_opStack.Peek().Equals("+") || _opStack.Peek().Equals("-") || _opStack.Peek().Equals("(")))
            {
                ToOut(_opStack.Pop());
            }
            _opStack.Push(input);
        }

        if (input.Equals("+") || input.Equals("-"))
        {
            while(_opStack.Count != 0 && !(_opStack.Peek().Equals("(")))
            {
                ToOut(_opStack.Pop());
            }
            _opStack.Push(input);
        }

        string operands = "";
        foreach(int operand in _outStack)
        {
            operands += operand.ToString();
            operands += ", ";
        }
        Debug.Log("----------------------------------------------------------------");
        Debug.Log(operands);

        string ops = "";
        foreach (string op in _opStack)
        {
            ops += op;
            ops += ", ";
        }
        Debug.Log(ops);
    }

    void ToOut(string op)
    {
        int x = _outStack.Pop();
        int y = _outStack.Pop();
        int z = 0;

        switch (op)
        {
            case "+":
                z = y + x;
                break;
            case "-":
                z = y - x;
                break;
            case "*":
                z = y * x;
                break;
            case "/":
                z = y / x;
                break;
        }
        //Debug.LogFormat("z: {0}", z);
        _display.Out(z.ToString());
        _outStack.Push(z);
    }
}
