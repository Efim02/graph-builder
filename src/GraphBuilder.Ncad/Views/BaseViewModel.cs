namespace GraphBuilder.Ncad.Views
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Базовый класс для вью-моделей.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Указывает, правильно ли введены значения.
        /// </summary>
        public virtual bool IsDataValid => true;

        /// <summary>
        /// Указывает, сохраненны ли данные.
        /// </summary>
        public virtual bool IsSaved => true;

        /// <summary>
        /// Событие, генерируемое при изменении свойств.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Метод генерации события при изменении определенного свойства.
        /// </summary>
        /// <param name="propertyName"> Имя свойства. </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}