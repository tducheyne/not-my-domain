using System;
using System.Collections.Generic;
using System.Linq;

namespace NotMyDomain.Interface
{
    internal abstract class ConsoleOptionSelector<T> : IConsoleOptionSelector<T>
    {
        private readonly string _title;
        private readonly IList<T> _options;
        private readonly Func<T, string> _optionName;

        private int _selectedOptionIndex;
        private int? _chosenOptionIndex;

        protected ConsoleOptionSelector(string title, IEnumerable<T> options, Func<T, string> optionName)
        {
            _title = title;
            _optionName = optionName;
            _options = options.ToList();
        }

        public T Execute(bool defaultFirstOption = true)
        {
            _selectedOptionIndex = defaultFirstOption ? 0 : -1;

            while (!_chosenOptionIndex.HasValue)
            {
                Clear();
                PrintMenu();
                ProcessInput();
            }

            return _options[_chosenOptionIndex.Value];
        }

        private void Clear()
        {
            Console.SetCursorPosition(0, 0);
        }

        private void PrintMenu()
        {
            Console.WriteLine($"{_title}. Use arrow keys to change option. Select with Enter or Space");

            const string uncheckedCheckbox = "[ ]";
            const string checkedCheckbox = "[*]";

            for (int i = 0; i < _options.Count; i++)
            {
                var option = _options[i];
                var checkbox = _selectedOptionIndex == i ? checkedCheckbox : uncheckedCheckbox;
                var optionName = _optionName(option);

                Console.WriteLine($"{checkbox} {optionName}");
            }
        }

        private void ProcessInput()
        {
            var consoleKey = Console.ReadKey().Key;

            if (consoleKey == ConsoleKey.DownArrow) _selectedOptionIndex++;
            if (consoleKey == ConsoleKey.UpArrow) _selectedOptionIndex--;

            if (consoleKey == ConsoleKey.Enter || consoleKey == ConsoleKey.Spacebar)
            {
                if (_selectedOptionIndex < 0) return;

                _chosenOptionIndex = _selectedOptionIndex;
            }

            if (_selectedOptionIndex >= _options.Count) _selectedOptionIndex = 0;
            if (_selectedOptionIndex < 0) _selectedOptionIndex = _options.Count - 1;
        }
    }
}