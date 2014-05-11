using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace pewpewmobileapp
{
    class GetReliefGoodImg: Dictionary<string, string>
    {
        public const string cannedgoods = "Canned Goods";
        public const string readytoeat = "Ready-to-eat Foods";
        public const string noodles = "Noodles";
        public const string rice = "Rice";
        public const string water = "Water";
        public const string drinks = "Milk, Coffee, Drinks";
        public const string med = "Medical Supplies";
        public const string clothes = "Clothes";
        public const string utensils = "Utensils";
        public const string soap = "Hygiene Supplies";
        public const string conssupplies = "Building Materials";
        public const string vehicles = "Vehicles/Equipment";
        
        public GetReliefGoodImg()
        {
            this.Add(cannedgoods, "/Assets/icons_goods/cannedGood.png");
            this.Add(readytoeat, "/Assets/icons_goods/readytoeat.png");
            this.Add(noodles, "/Assets/icons_goods/noodle.png");
            this.Add(rice, "/Assets/icons_goods/rice.png");
            this.Add(water, "/Assets/icons_goods/water.png");
            this.Add(drinks, "/Assets/icons_goods/milk.png");
            this.Add(med, "/Assets/icons_goods/medicalSupplies.png");
            this.Add(clothes, "/Assets/icons_goods/shirt.png");
            this.Add(utensils, "/Assets/icons_goods/utensils.png");
            this.Add(soap, "/Assets/icons_goods/soap.png");
            this.Add(conssupplies, "/Assets/icons_goods/tools.png");
            this.Add(vehicles, "/Assets/icons_goods/vehicle.png");
        }

    }
}