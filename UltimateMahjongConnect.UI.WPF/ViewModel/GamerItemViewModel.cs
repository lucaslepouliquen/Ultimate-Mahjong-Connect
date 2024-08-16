using UltimateMahjongConnect.UI.WPF.Model;

namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class GamerItemViewModel : ValidationViewModelBase
    {
        private readonly GamerModel _model;

        public GamerItemViewModel(GamerModel model)
        {
            _model = model;
        }

        public int Id => _model.Id;

        public string? Pseudonyme
        {
            get => _model.Pseudonyme;
            set
            {
                _model.Pseudonyme = value;
                RaisePropertyChanged();
                if (string.IsNullOrEmpty(_model.Pseudonyme))
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
            get => _model.Password;
            set
            {
                _model.Password = value;
                RaisePropertyChanged();
                if (string.IsNullOrEmpty(_model.Password))
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
