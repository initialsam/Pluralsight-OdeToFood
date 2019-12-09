using OdeToFood.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OdeToFood.Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
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
        public IEnumerable<Restaurant> GetAll()
        {
            return from r in Restaurants
                   orderby r.Name
                   select r;
        }
    }

}
