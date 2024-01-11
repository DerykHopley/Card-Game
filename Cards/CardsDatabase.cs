// Unitinfo = [Type, Power, Cost, Name, Effect]
// Eventinfo = [Type, Effect]

using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Cards {
	public static class Cards
	{
		private static List<Card> _units;
		public static List<Card> GetCards()
		{
			string json = System.IO.File.ReadAllText("Cards/cards.json");
			_units = JsonConvert.DeserializeObject<List<Card>>(json);
			return _units;
		}

		public static Card JSONtoCard(string json)
		{
			return JsonConvert.DeserializeObject<Card>(json);
		}

		public static void Shuffle<Card>(this List<Card> list)
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			int n = list.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do provider.GetBytes(box);
				while (!(box[0] < n * (System.Byte.MaxValue / n)));
				int k = (box[0] % n);
				n--;
				Card value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static Card GetCard(string id)
		{	
			return GetCards().First(item => item.Id.Equals(id));
		}

		public static String GetJSON<Card>(this Card card)
		{
			return JsonConvert.SerializeObject(card);
		}
	}

	public class Events
	{
		public static List<Event> GetEvents()
		{

			JsonSerializer serializer = new JsonSerializer();

			string json = System.IO.File.ReadAllText("Cards/events.json");
			return JsonConvert.DeserializeObject<List<Event>>(json);
		}
	}

	public class CardPanel : Panel {
		public Card Card {get; set;}
	}

	public class Card {
		public string Id {get; set;}
		public string Type {get; set;}
		public string Name {get; set;}
		public int Cost { get; set; }
		public bool InPlay { get; set; }
		public bool IsOpponentCard { get; set; }
		public bool CostReduced { get; set; }
		public bool CostIncreased { get; set; }
		public int Power { get; set; }
		public int PowerReduced { get; set; }
		public int PowerIncreased { get; set; }
		public string Effect { get; set; }
		public string EffectRemoved { get; set; }
	}

	public class Event
	{
		public string Name { get; set; }
		public string Effect { get; set; }
	}
}
