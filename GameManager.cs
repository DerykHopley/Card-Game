using Godot;
using System;
using System.Collections.Generic;
using Cards;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
public class GameManager : Node
{
	//expose card and card back in editor
	[Export]
	public PackedScene CardScene;

	[Export]
	public PackedScene CardBackScene;

	[Export]
	public PackedScene FieldScene;

	public class DropZones
	{
		public List<Card> PlayerLeft;
		public List<Card> PlayerMid;
		public List<Card> PlayerRight;
		public List<Card> PlayerHand;
		public List<Card> OpponentLeft;
		public List<Card> OpponentMid;
		public List<Card> OpponentRight;
		
	}

	public enum CardZone {Hand, Left, Mid, Right}
	public enum CardSubZone {TopLeft,TopRight, Mid, BottomLeft, BottomRight}
	public class CardPlayedZone {
		public CardZone Zone;
		public CardSubZone? SubZone;
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
	private List<Card> _player_deck_org;
	private List<Card> _player_hand;
	private List<Event> _events;
	public int round;
	public int energy;

	public override void _Ready()
	{
        CardsInDropZone = new DropZones
        {
            PlayerLeft = new List<Card>(),
			PlayerMid = new List<Card>(),
            PlayerRight = new List<Card>(),
			PlayerHand = new List<Card>(),
			OpponentLeft = new List<Card>(),
			OpponentMid = new List<Card>(),
            OpponentRight = new List<Card>()
        };

		_player_deck = Cards.Cards.GetCards();
		Cards.Cards.Shuffle(_player_deck);
		//_events = Cards.Events.GetEvents();

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
		GD.Print(CardsInDropZone.PlayerLeft);
		GD.Print(CardsInDropZone.OpponentLeft);
		((Label) GetParent().GetNode("PlayerZone/PlayerTurnPanel/PlayerTurn")).Text = round.ToString();
		((Label) GetParent().GetNode("PlayerZone/PlayerEnergyPanel/PlayerEnergy")).Text = energy.ToString();
	}

	private void EvaluateTurn()
	{
		GD.Print("Evaluate Turn");
		round++;
		energy++;
		GD.Print(CardsInDropZone.PlayerLeft);
		GD.Print(CardsInDropZone.OpponentLeft);
		((Label) GetParent().GetNode("PlayerZone/PlayerTurnPanel/PlayerTurn")).Text = round.ToString();
		((Label) GetParent().GetNode("PlayerZone/PlayerEnergyPanel/PlayerEnergy")).Text = energy.ToString();
	}

	private void EvaluateTurnScore()
    {
		GD.Print("Evaluate Turn Score");
        GD.Print(CardsInDropZone.OpponentLeft.Count);
        if (CardsInDropZone.OpponentLeft.Count > 0){
            int opponentPowerLeft = CardsInDropZone.OpponentLeft.Sum(card=>card.Power);
            ((Label) GetParent().GetNode<Label>("FieldZone/FieldZoneLeft/OpponentPowerPanelLeft/OpponentTotalPowerLeft")).Text = opponentPowerLeft.ToString();
        }
		GD.Print(CardsInDropZone.PlayerLeft.Count);
		if (CardsInDropZone.PlayerLeft.Count > 0){
            int playerPowerLeft = CardsInDropZone.PlayerLeft.Sum(card=>card.Power);
            ((Label) GetParent().GetNode<Label>("FieldZone/FieldZoneLeft/PlayerPowerPanelLeft/PlayerTotalPowerLeft")).Text = playerPowerLeft.ToString();
        }
        
        /* 
        int opponentPowerMid = _gm.CardsInDropZone.OpponentMid.Sum(item => item.Power);
        ((Label) GetNode<Label>("FieldZone/FieldZoneMid/OpponentPowerPanelMid/OpponentTotalPowerMid")).Text = opponentPowerMid.ToString();
        int opponentPowerRight = _gm.CardsInDropZone.OpponentRight.Sum(item => item.Power);
        ((Label) GetNode<Label>("FieldZone/FieldZoneRight/OpponentPowerPanelRight/OpponentTotalPowerRight")).Text = opponentPowerRight.ToString(); */
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
		CardPanel card = CardScene.Instance<CardPanel>();
		card.RectPosition = new Vector2((CardsInDropZone.PlayerHand.Count * 130) + 96, 1082);
		//Random r = new Random();
		//card.RectRotation = r.Next(-3,3);

		card.Card = _card;

		AddChild(card);
		CardsInDropZone.PlayerHand.Add(_card);
	}

	//render opponent cards upon receiving signal from client object
	//private void RenderCards(int hand)
	//{
	//	for (int i = 0; i < hand; i++)
	//	{
	//		Panel card = CardBackScene.Instance<Panel>();
	//		card.RectPosition = new Vector2((i * 130) + 10, 25);
	//		AddChild(card);
	//		opponentCards.Add(card);
	//	}
	//}

	//handle drop signal from client object on opponent view
	private void RenderDrop(string zone, string sub_zone, string json)
	{
		Card _card = Cards.Cards.JSONtoCard(json);
		_card.IsOpponentCard = true;

		CardPanel card = CardScene.Instance<CardPanel>();
		card.Card = _card;

		GD.Print(zone);
		if (zone == CardZone.Left.ToString()) 
		{
			GD.Print(sub_zone);
			if (sub_zone == CardSubZone.TopLeft.ToString())
			{
				//Top left
				card.RectPosition = new Vector2(34, 382);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
			} 
			else if (sub_zone == CardSubZone.TopRight.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(188, 382);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
			}
			else if (sub_zone == CardSubZone.Mid.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(112, 302);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
			}
			else if (sub_zone == CardSubZone.BottomLeft.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(34, 252);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
			}
			else if (sub_zone == CardSubZone.BottomRight.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(188, 252);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
			}
				
			CardsInDropZone.OpponentLeft.Add(_card);
		} 
		else if (zone == CardZone.Mid.ToString()) 
		{
			if (CardsInDropZone.OpponentMid.Count < 2) {
				card.RectPosition = new Vector2((CardsInDropZone.OpponentMid.Count * 110) + 355, 374);
			} else if (CardsInDropZone.OpponentMid.Count < 4) {
				card.RectPosition = new Vector2(((CardsInDropZone.OpponentMid.Count-2) * 110) + 660, 247);
			}
			CardsInDropZone.OpponentMid.Add(_card);
		} 
		else if (zone == CardZone.Right.ToString()) 
		{
			GD.Print(sub_zone);
			if (sub_zone == CardSubZone.TopLeft.ToString())
			{
				//Top left
				card.RectPosition = new Vector2(644, 382);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
			} 
			else if (sub_zone == CardSubZone.TopRight.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(798, 382);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
			}
			else if (sub_zone == CardSubZone.Mid.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(722, 302);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
			}
			else if (sub_zone == CardSubZone.BottomLeft.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(644, 252);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
			}
			else if (sub_zone == CardSubZone.BottomRight.ToString())
			{
				//TODO: make method
				//Top left
				card.RectPosition = new Vector2(798, 252);
				card.RectScale = new Vector2((float)0.7,(float)0.7);
			}
			
			CardsInDropZone.OpponentRight.Add(_card);
		}
		else if (zone == CardZone.Hand.ToString()) 
		{
			if (CardsInDropZone.OpponentRight.Count < 2) {
				card.RectPosition = new Vector2((CardsInDropZone.OpponentRight.Count * 110) + 660, 382);
			} else if (CardsInDropZone.OpponentRight.Count < 4) {
				card.RectPosition = new Vector2(((CardsInDropZone.OpponentRight.Count-2) * 110) + 660, 257);
			}
			
			CardsInDropZone.OpponentRight.Add(_card);
		}
		AddChild(card);
	}

	//handle card being dropped
	//TODO: Remove opponents displayed cards when player removed/moved
	//TODO: Remove wierd 0.0 card on drop
	public void Drop(Card card, CardPlayedZone _played_zone, string old_area = null)
	{
		if (_played_zone.Zone == CardZone.Left)
		{
			CardsInDropZone.PlayerLeft.Add(card);
			CardsInDropZone.PlayerHand.Remove(card);
			//TODO: setup refresh of other cards
			//TODO: Autoload Singleton? https://docs.godotengine.org/en/stable/tutorials/scripting/singletons_autoload.html
			energy = energy-card.Cost;
			((Label) GetParent().GetNode("PlayerZone/PlayerEnergyPanel/PlayerEnergy")).Text = energy.ToString();
		} 
		else if (_played_zone.Zone == CardZone.Mid)
		{
			CardsInDropZone.PlayerMid.Add(card);
			CardsInDropZone.PlayerHand.Remove(card);
		} 
		else if (_played_zone.Zone == CardZone.Right)
		{
			CardsInDropZone.PlayerRight.Add(card);
			CardsInDropZone.PlayerHand.Remove(card);
		} 
		else {
			CardsInDropZone.PlayerHand.Add(card);
			energy = energy+card.Cost;
		}

		if (old_area == "Left")
		{
			CardsInDropZone.PlayerLeft.Remove(card);
			CardsInDropZone.PlayerHand.Add(card);
		} 
		else if (old_area == "Mid")
		{
			CardsInDropZone.PlayerMid.Remove(card);
			CardsInDropZone.PlayerHand.Add(card);
		} 
		else if (old_area == "Right")
		{
			CardsInDropZone.PlayerRight.Remove(card);
			CardsInDropZone.PlayerHand.Add(card);
		}
		
		//Refresh cards
		foreach (CardPanel child in GetChildren()){
			GD.Print(child.Card.Id+":"+child.Card.InPlay);
			child._Ready();
		} 
		EmitSignal(nameof(CardDropped), _played_zone.Zone.ToString(), _played_zone.SubZone.ToString(), card.GetJSON());
		EmitSignal(nameof(EvaluateScore));
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

		//signal to let client object know card has been dropped
	[Signal]
	public delegate void EvaluateScore();
}
