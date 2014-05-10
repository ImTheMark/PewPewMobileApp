using pewpewmobileapp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pewpewmobileapp
{
    class ApprovalRequest
    {

        public async Task<List<Asset>> GetListOfAsset()
        {
            List<Asset> listAsset = await App.MobileService.GetTable<Asset>().ToListAsync();
            return listAsset;
        }

        public async Task<Asset> FindAvailableAsset(string name)
        {
            List<Asset> assets = await GetListOfAsset();
            foreach (Asset asset in assets)
            {
                if (asset.status.Equals("AVAILABLE") && asset.name.Equals(name))
                {
                    return asset;
                }
            }

            return null;
        }


        public async void RequestAsset(int quantity, string assetName, Town requester)
        {
            List<Asset> assets = await GetListOfAsset();
            for (int i = 0; i < quantity; i++)
            {
                Asset foundAsset = await FindAvailableAsset(assetName);
                if (foundAsset != null)
                {
                    foundAsset.borrower = requester.contactperson;
                    foundAsset.status = "ON THE WAY";
                    await App.MobileService.GetTable<Asset>().UpdateAsync(foundAsset);
                }
                else
                {
                    Pending pendingticket = new Pending();
                    pendingticket.assetName = assetName;
                    pendingticket.townID = requester.id;
                    await App.MobileService.GetTable<Pending>().InsertAsync(pendingticket);
                }
            }

        }

        
    }
}
