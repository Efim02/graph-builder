namespace GraphBuilder.Ncad.Views.Common
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Базовая команда.
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        /// <summary>
        /// Указывает, доступна ли возможность вызова команды.
        /// </summary>
        /// <param name="parameter"> Параметр для команды. </param>
        /// <returns> True - доступна. </returns>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Событие изменения возможности вызывать команду.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Выполнение команды.
        /// </summary>
        /// <param name="parameter"> Параметр для команды. </param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// Вызывает событие изменения возможности вызвать команду.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}