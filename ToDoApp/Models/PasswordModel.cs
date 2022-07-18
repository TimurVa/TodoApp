using System;

namespace ToDoApp.Models
{
    public class PasswordModel : BaseModel
    {
        public event EventHandler WhatForChanged;
        public event EventHandler UserNameChanged;
        public event EventHandler PasswordChanged;

        public long Id { get; set; }

        private string _whatFor;
        /// <summary>
        /// Site or program or anything else for
        /// </summary>
        public string WhatFor
        {
            get => _whatFor;
            set
            { 
                if (_whatFor == value)
                {
                    return;
                }

                _whatFor = value;
                OnPropertyChanged();

                WhatForChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private string _userName;
        /// <summary>
        /// Login or anything else
        /// </summary>
        public string UserName
        {
            get => _userName;
            set 
            {
                if (_userName == value)
                {
                    return;
                }

                _userName = value;
                OnPropertyChanged();
                
                UserNameChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        private string _password;
        /// <summary>
        /// Unhashed password
        /// </summary>
        public string Password
        {
            get => _password;
            set 
            {
                if (_password == value)
                {
                    return;
                }


                _password = value;
                OnPropertyChanged();

                PasswordChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
