using SecurePad.Views;
using Xamarin.Forms;

namespace SecurePad.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {

        private string itemId;
        private string text;
        private string description;

        public string Id { get; set; }
        public Command EditCommand { get; }
        public Command DeleteCommand { get; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public ItemDetailViewModel()
        {
            EditCommand = new Command(OnItemSelected);
            DeleteCommand = new Command(DeleteItem);
        }

        public async void LoadItemId(string itemId)
        {
            var item = await DataStore.GetItemAsync(itemId);
            Id = item.Id;
            Text = item.Name;
            Description = item.Description;
        }

        private async void DeleteItem()
        {
            await DataStore.DeleteItemAsync(ItemId);
            await Shell.Current.GoToAsync("..");
        }

        private async void OnItemSelected()
        {
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemEditPage)}?{nameof(ItemEditViewModel.ItemId)}={ItemId}");
            IsBusy = true;
            LoadItemId(Id);
            IsBusy = false;
        }

    }
}
