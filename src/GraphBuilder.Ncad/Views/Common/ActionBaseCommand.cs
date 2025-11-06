namespace GraphBuilder.Ncad.Views.Common
{
    using System;
    using System.Windows;

    /// <summary>
    /// Базовая команда для реализации Action действия.
    /// </summary>
    public sealed class ActionBaseCommand : BaseCommand
    {
        private readonly Action _action;

        /// <summary>
        /// Базовая команда делегат.
        /// </summary>
        /// <param name="action"> Действие. </param>
        public ActionBaseCommand(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// Выполнение команды.
        /// </summary>
        /// <param name="parameter"> Параметр для команды. </param>
        public override void Execute(object parameter)
        {
            try
            {
                _action.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}