using SecurePad.Models;
using System;
using Xamarin.Forms;

namespace SecurePad.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {

        private string description;
        private string name;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(description);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            IsBusy = true;

            Item newItem = new Item()
            {
                Id = Guid.NewGuid().ToString(),
                Description = Description,
                Name = Name,
                DateAdded = DateTime.Now,
            };

            await DataStore.AddItemAsync(newItem);

            IsBusy = false;

            await Shell.Current.GoToAsync("..");
        }

    }
}
