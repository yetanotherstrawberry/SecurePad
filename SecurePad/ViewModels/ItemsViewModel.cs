using SecurePad.Models;
using SecurePad.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace SecurePad.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {

        private Item _selectedItem;

        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command PasswordCommand { get; }
        public Command<Item> ItemTapped { get; }
        public Command<string> ItemDelete { get; }

        public Command DeleteEverythingCommand { get; }

        public ItemsViewModel()
        {
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            ItemTapped = new Command<Item>(OnItemSelected);
            ItemDelete = new Command<string>(DeleteItem);
            AddItemCommand = new Command(OnAddItem);
            DeleteEverythingCommand = new Command(ExecuteDeleteEverything);
            PasswordCommand = new Command(OnPasswordCommand);
        }

        private async void ExecuteDeleteEverything()
        {
            IsBusy = true;
            await DataStore.DeleteEverything();
            IsBusy = false;

            ExecuteLoadItemsCommand();
        }

        private async void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync();
                foreach (var item in items) Items.Add(item);
            }
            catch (Exception ex)
            {
                Toast.ShowToast("Error: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        private async void OnPasswordCommand()
        {
            await Shell.Current.GoToAsync(nameof(NewPasswordPage));
        }

        private async void DeleteItem(string id)
        {
            await DataStore.DeleteItemAsync(id);
            ExecuteLoadItemsCommand();
        }

        private async void OnItemSelected(Item item)
        {
            if (item == null) return;
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

    }
}
