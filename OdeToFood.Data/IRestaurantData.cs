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
        Restaurant Update(Restaurant updateRestaurant);
        Restaurant Add(Restaurant newRestaurant);
        Restaurant Delete(int id);
        int Commit();
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
        public Restaurant Add(Restaurant newRestaurant)
        {
            Restaurants.Add(newRestaurant);
            newRestaurant.Id = Restaurants.Max(r => r.Id) + 1;
            return newRestaurant;
        }
        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var restaurant = Restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);
            if (restaurant != null)
            {
                restaurant.Name = updatedRestaurant.Name;
                restaurant.Location = updatedRestaurant.Location;
                restaurant.Cuisine = updatedRestaurant.Cuisine;
            }
            return restaurant;
        }
        public Restaurant Delete(int id)
        {
            var restaurant = Restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant != null)
            {
                Restaurants.Remove(restaurant);
            }
            return null;
        }
        public int Commit()
        {
            return 0;
        }
    }

}
