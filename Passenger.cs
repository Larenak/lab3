namespace Lab3;

public struct Passenger
{
    private BaggageItem[] _items;

    public BaggageItem[] Items
    {
        get
        {
            return _items;
        }
        set
        {
            _items = value;
        }
    }

    public Passenger(BaggageItem[] items)
    {
        _items = items;
    }
}
