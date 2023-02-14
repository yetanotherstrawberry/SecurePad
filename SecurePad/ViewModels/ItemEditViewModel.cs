using SecurePad.Models;
using System;
using Xamarin.Forms;

namespace SecurePad.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemEditViewModel : BaseViewModel
    {

        private string itemId;
        private string text;
        private string description;

        public string Id { get; private set; }

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

        public Command SaveCommand { set; get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        public ItemEditViewModel()
        {
            SaveCommand = new Command(SaveItem);
            CancelCommand = new Command(OnCancel);
        }

        private async void SaveItem()
        {
            Item item = await DataStore.GetItemAsync(ItemId);
            item.Description = Description;
            item.Name = Text;
            await DataStore.UpdateItemAsync(item);
            await Shell.Current.GoToAsync("..");
        }

        public async void LoadItemId(string itemId)
        {
            var item = await DataStore.GetItemAsync(itemId);
            Id = item.Id;
            Text = item.Name;
            Description = item.Description;
        }

    }
}
