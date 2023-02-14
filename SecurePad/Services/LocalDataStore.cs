using SecurePad.Helpers;
using SecurePad.Interfaces;
using SecurePad.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SecurePad.Services
{
    internal class LocalDataStore : IDataStore<Item>
    {

        private ICollection<Item> Items { get; set; }
        private PasswordService PasswordService { get; }

        private const string DataStoreName = "Data";

        public LocalDataStore()
        {
            PasswordService = DependencyService.Get<PasswordService>();

            string cryptoData = null;
            if (Application.Current.Properties.ContainsKey(DataStoreName))
                cryptoData = (string)Application.Current.Properties[DataStoreName];

            if (string.IsNullOrEmpty(cryptoData))
                Items = new List<Item>();
            else
                Items = JsonSerializer.Deserialize<List<Item>>(AesClass.Decrypt(cryptoData, PasswordService.EncryptionKey));
        }

        public async Task AddItemAsync(Item item)
        {
            Items.Add(item);
            await SaveAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            var oldItem = Items.Single(arg => arg.Id == item.Id);
            item.DateAdded = oldItem.DateAdded;
            Items.Remove(oldItem);
            Items.Add(item);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            var serializedData = JsonSerializer.Serialize(Items);
            var encryptedData = AesClass.Encrypt(serializedData, PasswordService.EncryptionKey);
            var serializedCryptoData = JsonSerializer.Serialize(encryptedData);
            Application.Current.Properties[DataStoreName] = serializedCryptoData;
            await Application.Current.SavePropertiesAsync();
        }

        public async Task DeleteEverything()
        {
            Items = new List<Item>();
            await SaveAsync();
        }

        public async Task DeleteItemAsync(string id, bool save = true)
        {
            var oldItem = Items.Single(arg => arg.Id == id);
            Items.Remove(oldItem);
            if (save) await SaveAsync();
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(Items.Single(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.FromResult(Items);
        }

    }
}
