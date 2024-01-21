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
        public static void StartTurn(GameManager _gm, Card _card)
		{
			if (!_card.IsCopy) 
            {
                // Get cards in dropzone
                Dictionary<Enum, Card> cards = CardsInDropZone.PlayerLeft.GetCards();
                var _c = _gm.GetChildren();
                // Get original card
                Card original = cards
                    .Where(item => item.Value != null && item.Value.Name == "VoidFish" && !item.Value.IsCopy)
                    .Select(item => item.Value)
                    .First();
                
                // get zone/subzone where card is played
                CardZone zone = original.Zone.Zone;
                CardSubZone subZone = (CardSubZone)original.Zone.SubZone;
                // Get all empty spaces in this zone
                // Make dynamic for field where card is played
                var emptyFields = CardsInDropZone.PlayerLeft.GetCards()
                    .Where(
                        item => item.Value == null
                    ).Select(
                        item => item.Key
                    );
                if (emptyFields.Count() > 0)
                {
                    // select random empty subzone
                    Random rnd = new Random();
                    int r = rnd.Next(emptyFields.Count());
                    Enum e = emptyFields.ElementAt(r);
                    // duplicate this card
                    CardPanel card = _gm.CardScene.Instance<CardPanel>();
                    card.RectScale = new Vector2((float)0.7,(float)0.7);
                    card.Card = new Card
                    {
                        Id = original.Id + "-Copy",
                        Type = original.Type,
                        Name = original.Name,
                        Cost = 1,
                        Power = original.Power,
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
                    cards.Add((CardSubZone?)Enum.Parse(typeof(CardSubZone),e.ToString()), card.Card);
                }

                // Find other VoidFish copies merge with original and sum their power
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

                    foreach (CardPanel c in _gm.GetChildren())
                    {
                        if (!c.Card.IsCopy && c.Card.Name == "VoidFish")
                        {
                            c.Card.Power += otherPower;
                            c._Ready();
                        }
                    }
                }
            }
		}
    }
    public class Footman
    {
        public static void EndTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }
    }
    public class Archer
    {
        public static void EndTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }
    }
    public class Guardian
    {
        public static void EndTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }
    }

    public class Mentor
    {
        public static void EndTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }
    }
	public class Mercenary
    {
        public static void EndTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }
    }
    public class Knight
    {
        public static void EndTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }

        public static void StartTurn(GameManager _gm, Card _card)
        {
            GD.Print(_card);
        }
    }
}
