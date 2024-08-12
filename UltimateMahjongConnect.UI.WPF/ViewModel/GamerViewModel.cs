using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class GamerViewModel : ValidationViewModelBase
    {
        private readonly Gamer _gamer;

        public GamerViewModel(Gamer gamer)
        {
            _gamer = gamer; 
        }

        public string? Pseudonyme
        {
            get => _gamer.Pseudonyme;
            set
            {
                _gamer.Pseudonyme = value;
                RaisePropertyChanged();
                if(string.IsNullOrEmpty(_gamer.Pseudonyme))
                {
                    AddError("Pseudonyme", "Pseudonyme cannot be empty");
                }
                else
                {
                    ClearErrors("Pseudonyme");
                }
            }
        }
        
        public string? Password
        {
            get => _gamer.Password; 
            set
            {
                _gamer.Password = value;
                RaisePropertyChanged();
                if (string.IsNullOrEmpty(_gamer.Password))
                {
                    AddError("Password", "Password cannot be empty");
                }
                else
                {
                    ClearErrors("Password");
                }
            }
        }
    }
}
