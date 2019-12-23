using OdeToFood.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OdeToFood.Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetRestaurantsByName(string name);
        Restaurant GetById(int id);
    }

    public class InMomoryRestaurantData : IRestaurantData
    {
        public List<Restaurant> Restaurants { get; }
      
        public InMomoryRestaurantData()
        {
            Restaurants = new List<Restaurant>()
            {
                new Restaurant { Id=1, Name="Pizzea",Location="aaaaa", Cuisine=CuisineType.Italian },
                new Restaurant { Id=2, Name="Cinnamon",Location="bbbbb", Cuisine=CuisineType.Indian },
                new Restaurant { Id=3, Name="Costa",Location="cccccc", Cuisine=CuisineType.Mexican }
            };
        }
        public IEnumerable<Restaurant> GetRestaurantsByName(string name = null)
        {
            return from r in Restaurants
                   where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                   orderby r.Name
                   select r;
        }

        public Restaurant GetById(int id)
        {
            return Restaurants.SingleOrDefault(x => x.Id == id);
        }
    }

}
