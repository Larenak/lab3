namespace Lab3;

public struct BaggageItem
{
    private string _name;
    private decimal _mass;

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    public decimal Mass
    {
        get
        {
            return _mass;
        }
        set
        {
            _mass = value;
        }
    }

    public BaggageItem(string name, decimal mass)
    {
        _name = name;
        _mass = mass;
    }
}
