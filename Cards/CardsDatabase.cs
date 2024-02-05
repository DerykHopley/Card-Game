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
        private MoveCard _mc;
        private static Vector2 _start_position;
        public VoidFish()
        {
            _mc = new MoveCard();
        }
        
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
                
                foreach (CardPanel c in _gm.GetChildren())
                {
                    if (c.Card == original)
                    {
                        _start_position = (Vector2)c.Card.Zone.Position();
                    }
                }

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
                    // define new zone
                    CardPlayedZone _zone = new CardPlayedZone(){
                        Zone = CardZone.Left,
                        SubZone = (CardSubZone?)Enum.Parse(typeof(CardSubZone),e.ToString()),
                    };
                    
                    // duplicate this card
                    CardPanel card = _gm.CardScene.Instance<CardPanel>();
                    card.RectScale = new Vector2((float)0.7,(float)0.7);
                    card.RectPosition = _start_position;
                    card.Card = new Card
                    {
                        Id = original.Id + "-Copy",
                        Type = original.Type,
                        Name = original.Name,
                        Cost = 1,
                        Power = original.Power,
                        InPlay = true,
                        IsCopy = true,
                        Zone = _zone,
                        IsOpponentCard = false,
                        HasTweenMethod = "EndTurn"
                    };
                    card.GetNode<Label>("IsCopy").Text = card.Card.IsCopy ? "IsCopy" : "IsOriginal";

                    // add new card to empty subzone
                    _gm.AddChild(card);
                    // ?? If cards are run in an order this duplicate isn't needed
                    //_gm.AddCardtoZone(card.Card);
                    cards.Add((CardSubZone?)Enum.Parse(typeof(CardSubZone),e.ToString()), card.Card);
                }
            }
		}

        public static void EndTurn(GameManager _gm, Card _card)
        {

            Dictionary<Enum, Card> cards = CardsInDropZone.PlayerLeft.GetCards();
            // Get original card
            Card original = cards
                .Where(item => item.Value != null && item.Value.Name == "VoidFish" && !item.Value.IsCopy)
                .Select(item => item.Value)
                .First();

            foreach (CardPanel c in _gm.GetChildren())
            {
                if (c.Card == _card)
                {
                    c.Card.Zone = original.Zone;
                    c.Card.HasTweenMethod = "MergePower";
                    c._Ready();
                }
            }
        }

        public static void MergePower(GameManager _gm, Card _card)
        {
            foreach (CardPanel c in _gm.GetChildren())
            {
                if (c.Card == _card)
                {
                    _gm.RemoveChild(c);
                }
            }
            Dictionary<Enum, Card> cards = CardsInDropZone.PlayerLeft.GetCards();
            // Get original card
            Card original = cards
                .Where(item => item.Value != null && item.Value.Name == "VoidFish" && !item.Value.IsCopy)
                .Select(item => item.Value)
                .First();

            foreach (CardPanel c in _gm.GetChildren())
            {
                if (c.Card == original)
                {
                    c.Card.Power += _card.Power;
                    c.Card.HasTweenMethod = "none";
                    c._Ready();
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
