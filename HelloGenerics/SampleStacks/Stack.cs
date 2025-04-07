namespace HelloGenerics.Sample1
{
    public class Stack
    {
        private int _currentIndex = 0;
        private object[] _dynamicArray;

        public Stack(int startSize = 4)
        {
            _dynamicArray = new object[startSize];
        }

        public void Push(object item)
        {
            if (_currentIndex == _dynamicArray.Length)
            {
                object[] newData = new object[_dynamicArray.Length * 2];
                Array.Copy(_dynamicArray, newData, _dynamicArray.Length);
                _dynamicArray = newData;
            }
            _dynamicArray[_currentIndex++] = item;
        }

        public object Pop()
        {
            if (_currentIndex == 0)
                throw new InvalidOperationException("Der Stack ist leer");

            var result = _dynamicArray[--_currentIndex];
            _dynamicArray[_currentIndex] = null; // Eintrag "entfernen"
            return result;
        }
    }
}
