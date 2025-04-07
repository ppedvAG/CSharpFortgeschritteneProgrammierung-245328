namespace HelloGenerics.SampleISP
{
    // Super-Interface was mehrere Interfaces vereint
    // *** Bitte nicht nachmachen! ***
    // Warum? Weil unendlich viele Kombinationsmoeglichkeiten denkbar sind
    public interface IPerson : IEat, IChef, ISwim
    {
    }
}