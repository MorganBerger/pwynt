using UnityEngine;

public class ExpAbility {

    private ExpAbility(string value) { Value = value; }

    public string Value { get; private set; }

    public static ExpAbility Trace   { get { return new ExpAbility("Trace"); } }
    public static ExpAbility Debug   { get { return new ExpAbility("Debug"); } }
    public static ExpAbility Info    { get { return new ExpAbility("Info"); } }
    public static ExpAbility Warning { get { return new ExpAbility("Warning"); } }
    public static ExpAbility Error   { get { return new ExpAbility("Error"); } }

    public override string ToString()
    {
        return Value;
    }
}

public class ExperimentalObj: MonoBehaviour {

}