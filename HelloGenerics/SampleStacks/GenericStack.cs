namespace HelloGenerics.Sample1
{
    // T steht fuer Typeargument
    // und ist Platzhalter fuer beliebige Datentypen
    public class GenericStack<T>
    {
        private int _currentIndex = 0;
        private T[] _dynamicArray;

        public GenericStack(int startSize = 4)
        {
            _dynamicArray = new T[startSize];
        }

        public void Push(T item)
        {
            if (_currentIndex == _dynamicArray.Length)
            {
                T[] newData = new T[_dynamicArray.Length * 2];
                Array.Copy(_dynamicArray, newData, _dynamicArray.Length);
                _dynamicArray = newData;
            }
            _dynamicArray[_currentIndex++] = item;
        }

        public T Pop()
        {
            if (_currentIndex == 0)
                throw new InvalidOperationException("Der Stack ist leer");

            var result = _dynamicArray[--_currentIndex];
            _dynamicArray[_currentIndex] = default; // Standartwert fuer den Datentyp hinter T
            return result;
        }
    }
}
