using System;

namespace ToDoApp.Models
{
    public class TodoModel : BaseModel
    {
        public event EventHandler NameChangedEvent;
        public event EventHandler DescriptionChangedEvent;
        public event EventHandler DoneChangedEvent;

        #region Properties
        public long Id { get; set; }

        private string _name;
        /// <summary>
        /// Kind of title. Not really in use
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged();

                NameChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private string _description;
        /// <summary>
        /// Description of to do
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                {
                    return;
                }

                _description = value;
                OnPropertyChanged();

                DescriptionChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Time todo created
        /// </summary>
        public DateTime CreatedTime { get; set; }

        private bool _isDone = false;
        /// <summary>
        /// true if todo completed
        /// </summary>
        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                OnPropertyChanged();

                if (_isDone)
                {
                    _bgColorString = "#DADCDD";
                    DoneTime = DateTime.Now;
                }
                else
                {
                    _bgColorString = "#EEF0F2";
                    DoneTime = null;
                }
                OnPropertyChanged(nameof(BgColorString));
                DoneChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private string _bgColorString;
        /// <summary>
        /// only for UI helper
        /// </summary>
        public string BgColorString
        {
            get => _bgColorString;
            set
            {
                if (_bgColorString == value)
                {
                    return;
                }

                _bgColorString = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _doneTime;
        /// <summary>
        /// Time when <see cref="IsDone"/> becomes true
        /// </summary>
        public DateTime? DoneTime
        {
            get => _doneTime;
            set
            {
                _doneTime = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
