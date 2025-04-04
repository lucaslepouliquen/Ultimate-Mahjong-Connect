﻿using System.Windows.Input;
using UltimateMahjongConnect.UI.WPF.ViewModel;

namespace UltimateMahjongConnect.UI.WPF.Command
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute=null)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(_execute));
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(ViewModelBase selectedViewModel) { }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => _canExecute is null || _canExecute(parameter);
        public void Execute(object? parameter) => _execute(parameter);
    }
}