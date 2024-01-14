using Godot;
using System;
using System.Collections.Generic;
using CardCollection;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
public class GameManager : Node
{
	//expose card and card back in editor
	[Export]
	public PackedScene CardScene;

	[Export]
	public PackedScene CardBackScene;

	[Export]
	public PackedScene FieldScene;

	public class SubDropZones
	{
		private enum SubZones {TopLeft,TopRight,Mid,BottomLeft,BottomRight}
		public Card TopLeft;
		public Card TopRight;
		public Card Mid;
		public Card BottomLeft;
		public Card BottomRight;
		public Dictionary<Enum, Card> GetCards()
		{
			return new Dictionary<Enum,Card>(){
				{SubZones.TopLeft, TopLeft},
				{SubZones.TopRight, TopRight},
				{SubZones.Mid, Mid},
				{SubZones.BottomLeft, BottomLeft},
				{SubZones.BottomRight, BottomRight},
			};
		}
		public bool HasCards()
		{
			if (TopLeft != null || TopRight != null || Mid != null || BottomLeft != null || BottomRight != null)
				return true;
			return false;
		}
	}
	public class DropZones
	{
		public SubDropZones PlayerLeft;
		public SubDropZones PlayerMid;
		public SubDropZones PlayerRight;
		public List<Card> PlayerHand;
		public SubDropZones OpponentLeft;
		public SubDropZones OpponentMid;
		public SubDropZones OpponentRight;
		
	}

	public Dictionary<string, FieldZone> FieldZones = new Dictionary<string, FieldZone>
	{
		{"Ocean", new FieldZone(){
				Region = new Rect2(0,0,192,192)
			}
		},
		{"Grass", new FieldZone(){Region = new Rect2(192,0,192,192)}},
		{"Arctic", new FieldZone(){Region = new Rect2(384,0,192,192)}},
		{"Desert", new FieldZone(){Region = new Rect2(576,0,192,192)}},
		{"Hole", new FieldZone(){Region = new Rect2(0,192,192,192)}},
		{"Void", new FieldZone(){Region = new Rect2(192,192,192,192)}},
		{"Lava", new FieldZone(){Region = new Rect2(384,192,192,192)}},
		{"Forest", new FieldZone(){Region = new Rect2(576,192,192,192)}},
		{"Blackhole", new FieldZone(){Region = new Rect2(768,192,192,192)}},
	};

	public class FieldZone {
		public Rect2 Region;
	}

	//keep track of number of cards in dropzone
	public static DropZones CardsInDropZone;

	//keep track of number of opponent card backs to render
	private List<Panel> opponentCards = new List<Panel>();

	private List<Card> _player_deck;
	public int round;
	public int energy;

	public override void _Ready()
	{
        CardsInDropZone = new DropZones
        {
			PlayerLeft = new SubDropZones(),
			PlayerMid = new SubDropZones(),
			PlayerRight = new SubDropZones(),
            PlayerHand = new List<Card>(),
			OpponentLeft = new SubDropZones(),
			OpponentMid = new SubDropZones(),
			OpponentRight = new SubDropZones()
        };

        _player_deck = Cards.GetCards();
		//Cards.Shuffle(_player_deck);

		round = 0;
		energy = 0;

		base._Ready();
	}

	private void GameStart(string left, string mid, string right)
	{
		//TODO: Make method
		//TODO: Make Scene / Scene state
		//TODO: Set same for Opponent/Player (Server choice? Wait for server initialization)
		foreach (int i in Enum.GetValues(typeof(CardSubZone))) 
		{
			((Sprite) GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/Land"+Enum.GetName(typeof(CardSubZone), i))).RegionRect = FieldZones[left].Region;
			((Sprite) GetParent().GetNode<Sprite>("OpponentZone/OpponentZoneLeft/Lands/Land"+Enum.GetName(typeof(CardSubZone), i))).RegionRect = FieldZones[left].Region;

			((Sprite) GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/Land"+Enum.GetName(typeof(CardSubZone), i))).RegionRect = FieldZones[mid].Region;
			((Sprite) GetParent().GetNode<Sprite>("OpponentZone/OpponentZoneMid/Lands/Land"+Enum.GetName(typeof(CardSubZone), i))).RegionRect = FieldZones[mid].Region;

			((Sprite) GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/Land"+Enum.GetName(typeof(CardSubZone), i))).RegionRect = FieldZones[right].Region;
			((Sprite) GetParent().GetNode<Sprite>("OpponentZone/OpponentZoneRight/Lands/Land"+Enum.GetName(typeof(CardSubZone), i))).RegionRect = FieldZones[right].Region;
		}
		//TODO: Make scene listen to signal on energy change
		round = 1;
		energy = 1;
		((Label) GetParent().GetNode("PlayerZone/PlayerTurnPanel/PlayerTurn")).Text = round.ToString();
		((Label) GetParent().GetNode("PlayerZone/PlayerEnergyPanel/PlayerEnergy")).Text = energy.ToString();
	}

	private void EndRound()
	{
		GD.Print("End Turn:");
		if (CardsInDropZone.PlayerLeft.HasCards())
		{		
			foreach (var card in CardsInDropZone.PlayerLeft.GetCards().Where(item => item.Value != null).ToList())
			{
				string name = card.Value.Name;
				Type type = Type.GetType("CardCollection." + name);
				//Type type = Type.GetType("CardCollection.VoidFish");
				if (type != null)
				{
					type.InvokeMember("EndTurn", 
						BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
						null,
						null,
						new object[] { this, card.Value, CardsInDropZone }
					);
				}
			}
			int? playerPowerLeft = CardsInDropZone.PlayerLeft.GetCards()
				.Where(item => item.Value != null)
				.Select(item => (int?)item.Value.Power)
				.Sum();
            GetParent().GetNode<Label>("FieldZone/FieldZoneLeft/PlayerPowerPanelLeft/PlayerTotalPowerLeft").Text = playerPowerLeft.ToString();
        }
		// Trigger actions of cards in dropped order
		// trigger update of field power
		// trigger
		round++;
		((Label) GetParent().GetNode("PlayerZone/PlayerTurnPanel/PlayerTurn")).Text = round.ToString();
		EmitSignal(nameof(NextRound));
	}

	private void StartRound()
	{
		GD.Print("Start Round:");
		DrawCards(1);
		energy++;
		((Label) GetParent().GetNode("PlayerZone/PlayerEnergyPanel/PlayerEnergy")).Text = energy.ToString();

		if (CardsInDropZone.PlayerLeft.HasCards())
		{		
			foreach (var card in CardsInDropZone.PlayerLeft.GetCards().Where(item => item.Value != null).ToList())
			{
				string name = card.Value.Name;
				//Type type = Type.GetType("CardCollection." + name);
				Type type = Type.GetType("CardCollection.VoidFish");
				if (type != null)
				{
					GD.Print("Call Method: CardCollection." + name + ".StartTurn()");
					type.InvokeMember("StartTurn", 
						BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
						null,
						null,
						new object[] { this, card.Value, CardsInDropZone }
					);
				}
			}
			int? playerPowerLeft = CardsInDropZone.PlayerLeft.GetCards()
				.Where(item => item.Value != null)
				.Select(item => (int?)item.Value.Power)
				.Sum();
            GetParent().GetNode<Label>("FieldZone/FieldZoneLeft/PlayerPowerPanelLeft/PlayerTotalPowerLeft").Text = playerPowerLeft.ToString();
        }
	}

	private void EvaluateScoreTurn()
    {
		GD.Print("Evaluate Turn Score:");
        if (CardsInDropZone.OpponentLeft.HasCards()){
            var opponentPowerLeft = CardsInDropZone.OpponentLeft.GetCards().Values;
			//.Sum(item => item.Power)
            ((Label) GetParent().GetNode<Label>("FieldZone/FieldZoneLeft/OpponentPowerPanelLeft/OpponentTotalPowerLeft")).Text = opponentPowerLeft.ToString();
        }
		if (CardsInDropZone.PlayerLeft.HasCards())
		{
            int? playerPowerLeft = CardsInDropZone.PlayerLeft.GetCards()
				.Where(item => item.Value != null)
				.Select(item => (int?)item.Value.Power)
				.Sum();
            GetParent().GetNode<Label>("FieldZone/FieldZoneLeft/PlayerPowerPanelLeft/PlayerTotalPowerLeft").Text = playerPowerLeft.ToString();
		
			foreach (var card in CardsInDropZone.PlayerLeft.GetCards().Where(item => item.Value != null).ToList())
			{
				string name = card.Value.Name;
				//Type type = Type.GetType("CardCollection." + name);
				Type type = Type.GetType("CardCollection.VoidFish");
				if (type != null)
				{
					GD.Print("Call Method: CardCollection." + name + ".StartTurn()");
					type.InvokeMember("StartTurn", 
						BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
						null,
						null,
						new object[] { this, card.Value, CardsInDropZone }
					);

					type.InvokeMember("EndTurn", 
						BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
						null,
						null,
						new object[] { this, card.Value, CardsInDropZone }
					);
				}
			}
        }
        if (CardsInDropZone.OpponentMid.HasCards()){
            int opponentPowerMid = CardsInDropZone.OpponentMid.GetCards().Values.Sum(item => item.Power);
            ((Label) GetParent().GetNode<Label>("FieldZone/FieldZoneMid/OpponentPowerPanelMid/OpponentTotalPowerMid")).Text = opponentPowerMid.ToString();
        }
		if (CardsInDropZone.PlayerMid.HasCards()){
            int playerPowerMid = CardsInDropZone.PlayerMid.GetCards().Values.Sum(item => item.Power);
            ((Label) GetParent().GetNode<Label>("FieldZone/FieldZoneMid/PlayerPowerPanelMid/PlayerTotalPowerMid")).Text = playerPowerMid.ToString();
        }
		if (CardsInDropZone.OpponentRight.HasCards()){
            int opponentPowerRight = CardsInDropZone.OpponentRight.GetCards().Values.Sum(item => item.Power);
            ((Label) GetParent().GetNode<Label>("FieldZone/FieldZoneRight/OpponentPowerPanelRight/OpponentTotalPowerRight")).Text = opponentPowerRight.ToString();
        }
		if (CardsInDropZone.PlayerRight.HasCards()){
            int playerPowerRight = CardsInDropZone.PlayerRight.GetCards().Values.Sum(item => item.Power);
            ((Label) GetParent().GetNode<Label>("FieldZone/FieldZoneRight/PlayerPowerPanelRight/PlayerTotalPowerRight")).Text = playerPowerRight.ToString();
        }
		GD.Print("Evaluate Turn Score:");
    }

	//draw cards and emit signal to client object that cards have been drawn
	private void DrawCards(int cardCount)
	{
		// check if _player_deck has cards
		if (_player_deck != null && _player_deck.Any())
		{
			if (_player_deck.Count < cardCount)
			{
				cardCount = _player_deck.Count;
			}
			//get range, add to player hand and remove from deck
			foreach (Card card in _player_deck.GetRange(0,cardCount).ToList())
			{
				drawCard(card);
				_player_deck.Remove(card);
			}
			EmitSignal(nameof(CardsDrawn), CardsInDropZone.PlayerHand.Count);
		} else {
			GD.Print("Deck Empty");
			EmitSignal(nameof(DeckEmpty));
		}
	}

	private void drawCard(Card _card)
	{
		//Create new card
		CardPanel card = CardScene.Instance<CardPanel>();
		GD.Print(CardsInDropZone.PlayerHand.Count);
		card.RectPosition = new Vector2((CardsInDropZone.PlayerHand.Count * 130) + 96, 1082);
		card.Card = _card;
		card.Card.Zone = new CardPlayedZone(){
			Zone = CardZone.Hand,
			Order = CardsInDropZone.PlayerHand.Count
		};

		AddChild(card);
		AddCardtoZone(card.Card);
	}

	public void MoveCardtoZone(Card card)
	{
		RemoveCardfromZone(card);
		AddCardtoZone(card);
	}

	public void AddCardtoZone(Card card)
	{
		switch (card.Zone.Zone)
		{
			case CardZone.Left:
				switch (card.Zone.SubZone)
				{
					case CardSubZone.TopLeft:
						CardsInDropZone.PlayerLeft.TopLeft = card;
						break;
					case CardSubZone.TopRight:
						CardsInDropZone.PlayerLeft.TopRight = card;
						break;
					case CardSubZone.Mid:
						CardsInDropZone.PlayerLeft.Mid = card;
						break;
					case CardSubZone.BottomLeft:
						CardsInDropZone.PlayerLeft.BottomLeft = card;
						break;
					case CardSubZone.BottomRight:
						CardsInDropZone.PlayerLeft.BottomRight = card;
						break;
				}
			break;
			case CardZone.Mid:
				switch (card.Zone.SubZone)
				{
					case CardSubZone.TopLeft:
						CardsInDropZone.PlayerMid.TopLeft = card;
						break;
					case CardSubZone.TopRight:
						CardsInDropZone.PlayerMid.TopRight = card;
						break;
					case CardSubZone.Mid:
						CardsInDropZone.PlayerMid.Mid = card;
						break;
					case CardSubZone.BottomLeft:
						CardsInDropZone.PlayerMid.BottomLeft = card;
						break;
					case CardSubZone.BottomRight:
						CardsInDropZone.PlayerMid.BottomRight = card;
						break;
				}
			break;
			case CardZone.Right:
				switch (card.Zone.SubZone)
				{
					case CardSubZone.TopLeft:
						CardsInDropZone.PlayerRight.TopLeft = card;
						break;
					case CardSubZone.TopRight:
						CardsInDropZone.PlayerRight.TopRight = card;
						break;
					case CardSubZone.Mid:
						CardsInDropZone.PlayerRight.Mid = card;
						break;
					case CardSubZone.BottomLeft:
						CardsInDropZone.PlayerRight.BottomLeft = card;
						break;
					case CardSubZone.BottomRight:
						CardsInDropZone.PlayerRight.BottomRight = card;
						break;
				}
			break;
			default:
				CardsInDropZone.PlayerHand.Add(card);
			break;
		}
	}

	public void RemoveCardfromZone(Card card)
	{
		switch (card.Zone.Zone)
		{
			case CardZone.Left:
				switch (card.Zone.SubZone)
				{
					case CardSubZone.TopLeft:
						CardsInDropZone.PlayerLeft.TopLeft = null;
						break;
					case CardSubZone.TopRight:
						if (CardsInDropZone.PlayerLeft.TopRight != null)
							CardsInDropZone.PlayerLeft.TopRight = null;
						break;
					case CardSubZone.Mid:
						CardsInDropZone.PlayerLeft.Mid = null;
						break;
					case CardSubZone.BottomLeft:
						CardsInDropZone.PlayerLeft.BottomLeft = null;
						break;
					case CardSubZone.BottomRight:
						CardsInDropZone.PlayerLeft.BottomRight = null;
						break;
				}
			break;
			case CardZone.Mid:
				switch (card.Zone.SubZone)
				{
					case CardSubZone.TopLeft:
						CardsInDropZone.PlayerMid.TopLeft = null;
						break;
					case CardSubZone.TopRight:
						CardsInDropZone.PlayerMid.TopRight = null;
						break;
					case CardSubZone.Mid:
						CardsInDropZone.PlayerMid.Mid = null;
						break;
					case CardSubZone.BottomLeft:
						CardsInDropZone.PlayerMid.BottomLeft = null;
						break;
					case CardSubZone.BottomRight:
						CardsInDropZone.PlayerMid.BottomRight = null;
						break;
				}
			break;
			case CardZone.Right:
				switch (card.Zone.SubZone)
				{
					case CardSubZone.TopLeft:
						CardsInDropZone.PlayerRight.TopLeft = null;
						break;
					case CardSubZone.TopRight:
						CardsInDropZone.PlayerRight.TopRight = null;
						break;
					case CardSubZone.Mid:
						CardsInDropZone.PlayerRight.Mid = null;
						break;
					case CardSubZone.BottomLeft:
						CardsInDropZone.PlayerRight.BottomLeft = null;
						break;
					case CardSubZone.BottomRight:
						CardsInDropZone.PlayerRight.BottomRight = null;
						break;
				}
			break;
			default:
				CardsInDropZone.PlayerHand.Add(card);
			break;
		}
	}

	//handle drop signal from client object on opponent view
	private void RenderDrop(string zone, string sub_zone, string json)
	{
		Card _card = Cards.JSONtoCard(json);
		_card.IsOpponentCard = true;

		CardPanel card = CardScene.Instance<CardPanel>();
		card.Card = _card;

		GD.Print(zone);
		if (zone == CardZone.Left.ToString()) 
		{
			if (sub_zone == CardSubZone.TopLeft.ToString())
			{
				//Top left
				card.RectPosition = new Vector2(34, 382);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentLeft.TopLeft = card.Card;
			} 
			else if (sub_zone == CardSubZone.TopRight.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(188, 382);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentLeft.TopRight = card.Card;
			}
			else if (sub_zone == CardSubZone.Mid.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(112, 302);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentLeft.Mid = card.Card;
			}
			else if (sub_zone == CardSubZone.BottomLeft.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(34, 252);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentLeft.BottomLeft = card.Card;
			}
			else if (sub_zone == CardSubZone.BottomRight.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(188, 252);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentLeft.BottomRight = card.Card;
			}	
		} 
		else if (zone == CardZone.Mid.ToString()) 
		{
			if (sub_zone == CardSubZone.TopLeft.ToString())
			{
				//Top left
				card.RectPosition = new Vector2(644, 382);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentMid.TopLeft = card.Card;
			} 
			else if (sub_zone == CardSubZone.TopRight.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(798, 382);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentMid.TopRight = card.Card;
			}
			else if (sub_zone == CardSubZone.Mid.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(722, 302);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentMid.Mid = card.Card;
			}
			else if (sub_zone == CardSubZone.BottomLeft.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(644, 252);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentMid.BottomLeft = card.Card;
			}
			else if (sub_zone == CardSubZone.BottomRight.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(798, 252);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentMid.BottomRight = card.Card;
			}
		} 
		else if (zone == CardZone.Right.ToString()) 
		{
			if (sub_zone == CardSubZone.TopLeft.ToString())
			{
				//Top left
				card.RectPosition = new Vector2(644, 382);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentRight.TopLeft = card.Card;
			} 
			else if (sub_zone == CardSubZone.TopRight.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(798, 382);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentRight.TopRight = card.Card;
			}
			else if (sub_zone == CardSubZone.Mid.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(722, 302);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentRight.Mid = card.Card;
			}
			else if (sub_zone == CardSubZone.BottomLeft.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(644, 252);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentRight.BottomLeft = card.Card;
			}
			else if (sub_zone == CardSubZone.BottomRight.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(798, 252);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
				CardsInDropZone.OpponentRight.BottomRight = card.Card;
			}
		}
		else if (zone == CardZone.Hand.ToString()) 
		{
		}
		AddChild(card);
	}

	//handle card being dropped
	//TODO: Remove opponents displayed cards when player removed/moved
	//TODO: Remove wierd 0.0 card on drop
	public void Drop(Card card, string old_area = null)
	{
		if (card.Zone.Zone == CardZone.Left)
		{
			MoveCardtoZone(card);
			//TODO: setup refresh of other cards
			//TODO: Autoload Singleton? https://docs.godotengine.org/en/stable/tutorials/scripting/singletons_autoload.html
			energy = energy-card.Cost;
			((Label) GetParent().GetNode("PlayerZone/PlayerEnergyPanel/PlayerEnergy")).Text = energy.ToString();
		} 
		else if (card.Zone.Zone == CardZone.Mid)
		{
			MoveCardtoZone(card);
		} 
		else if (card.Zone.Zone == CardZone.Right)
		{
			MoveCardtoZone(card);
		} 
		else {
			MoveCardtoZone(card);
		}

		if (old_area == "Left")
		{
			MoveCardtoZone(card);
		} 
		else if (old_area == "Mid")
		{
			MoveCardtoZone(card);
		} 
		else if (old_area == "Right")
		{
			MoveCardtoZone(card);
		}
		
		//Refresh cards
		EmitSignal(nameof(CardDropped), card.Zone.Zone.ToString(), card.Zone.SubZone.ToString(), card.GetJSON());
	}

	public static double ConvertDegreesToRadians (double degrees)
	{
		double radians = (Math.PI / 180) * degrees;
		return (radians);
	}

	public static double ConvertRadiansToDegrees(double radians)
	{
		double degrees = (180 / Math.PI) * radians;
		return degrees;
	}

	//signal to let client object know cards have been drawn
	[Signal]
	public delegate void CardsDrawn();

	//signal to let client object know cards have been drawn
	[Signal]
	public delegate void DeckEmpty();

	//signal to let client object know card has been dropped
	[Signal]
	public delegate void CardDropped();

	[Signal]
	public delegate void NextRound();
}
