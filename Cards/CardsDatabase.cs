// Unitinfo = [Type, Power, Cost, Name, Effect]
// Eventinfo = [Type, Effect]

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static GameManager;

namespace CardCollection {

	public class VoidFish
	{
        public static void StartTurn(GameManager _gm, Card _card, DropZones _cards)
		{
			if (!_card.IsCopy) 
            {
                Dictionary<Enum, Card> cards = CardsInDropZone.PlayerLeft.GetCards();
                var _c = _gm.GetChildren();
                // Find other VoidFish copies and sum their power
                IEnumerable<KeyValuePair<Enum, Card>> copies = cards
                    .Where(item => item.Value != null && item.Value.Name == "VoidFish" && item.Value.IsCopy);

                if (copies.Count() > 0)
                {
                    int otherPower = copies
                        .Select(item => item.Value.Power)
                        .Sum();
                    foreach (var copy in copies)
                    {
                        foreach (CardPanel c in _gm.GetChildren())
                        {
                            if (c.Card.IsCopy && c.Card.Name == "VoidFish")
                            {
                                _gm.RemoveChild(c);
                            }
                        }
                        _gm.RemoveCardfromZone(copy.Value);
                    }

                    Card original = cards
                        .Where(item => item.Value != null && item.Value.Name == "VoidFish" && !item.Value.IsCopy)
                        .Select(item => item.Value)
                        .First();
                    foreach (CardPanel c in _gm.GetChildren())
                    {
                        if (!c.Card.IsCopy && c.Card.Name == "VoidFish")
                        {
                            c.Card.Power = otherPower;
                            c._Ready();
                        }
                    }
                }
                GD.Print();
            }
		}

		public static void EndTurn(GameManager _gm, Card _card, DropZones _cards)
		{
			// get zone/subzone where card is played
			CardZone zone = _card.Zone.Zone;
			CardSubZone subZone = (CardSubZone)_card.Zone.SubZone;
			// Get all empty spaces in this zone
			// Make dynamic for field where card is played
            var playerPowerLeft = CardsInDropZone.PlayerLeft.GetCards()
                .Where(
					item => item.Value == null
				).Select(
					item => item.Key
				);
			if (playerPowerLeft.Count() > 0)
			{
				// select random empty subzone
				Random rnd = new Random();
				int r = rnd.Next(playerPowerLeft.Count());
				Enum e = playerPowerLeft.ElementAt(r);
				// duplicate this card
				CardPanel card = _gm.CardScene.Instance<CardPanel>();
				card.RectScale = new Vector2((float)0.7,(float)0.7);
                card.Card = new Card
                {
                    Id = _card.Id + "-Copy",
                    Type = _card.Type,
                    Name = _card.Name,
                    Cost = 1,
                    Power = 1,
                    InPlay = true,
                    IsCopy = true,
                    Zone = new CardPlayedZone(){
                        Zone = CardZone.Left,
                        SubZone = (CardSubZone?)Enum.Parse(typeof(CardSubZone),e.ToString()),
                    },
                    IsOpponentCard = false
                };
				card.RectPosition = (Vector2)card.Card.Zone.Position();
                card.GetNode<Label>("IsCopy").Text = card.Card.IsCopy ? "IsCopy" : "IsOriginal";
				// add new card to empty subzone
                _gm.AddChild(card);
                _gm.AddCardtoZone(card.Card);
			}
		}
    }
    public class Footman
    {
        public static void EndTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }
    }
    public class Archer
    {
        public static void EndTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }
    }
    public class Guardian
    {
        public static void EndTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }
    }

    public class Mentor
    {
        public static void EndTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }
    }
	public class Mercenary
    {
        public static void EndTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }
    }
    public class Knight
    {
        public static void EndTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card, DropZones _cards)
        {
            GD.Print(_card);
        }
    }
}
