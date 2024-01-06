using Cards;
using Godot;
using System;

public class Drag : CardPanel
{
	//create private variables to store initial data
	private bool _mouseIn = false;
	private bool _isDragging = false;
	private bool _isPlayed = false;
	public enum Zone {Hand, Left, Mid, Right}
	private enum Played {Hand, Played, Discarded, Destroyed}
	private enum Turn {Start,Mid,End}
	public Zone ZoneState;
	
	private bool _isOverLeftLand1 = false;
	private bool _isOverLeftLand2 = false;
	private bool _isOverLeftLand3 = false;
	private bool _isOverLeftLand4 = false;
	private bool _isOverLeftLand5 = false;

	private Vector2 _startPosition;
	private GameManager _gm;
	private string _id;

	public override void _Ready()
	{
		//store start position and locate GameManager
		_startPosition = RectPosition;
		_gm = GetParent<GameManager>();
		((Label) GetNode("Id")).Text = Card.Id;
		((Label) GetNode("BottomBarContainer/NameCenterContainer/Name")).Text = Card.Name;
		((Label) GetNode("LeftMarginContainer/CenterContainer/Cost")).Text = Card.Cost.ToString();
		((Label) GetNode("RightMarginContainer/CenterContainer/Power")).Text = Card.Power.ToString();
	}

	public override void _Process(float delta)
	{
		if (_mouseIn)
		{
			//handle dragging and render card over other game objects
			//GD.Print("Mouse Entered");
			if (Input.IsActionPressed("left_click"))
			{
				_isDragging = true;
				Vector2 _mousePosition = new Vector2(GetViewport().GetMousePosition());
				RectPosition = new Vector2(_mousePosition.x - 40, _mousePosition.y - 40);
				GetParent().MoveChild(this, GetParent().GetChildCount());
			}
			//handle dropping or return card to start position if not over dropzone
			if (Input.IsActionJustReleased("left_click"))
			{
				_isDragging = false;
				if (_isOverLeftDropZone)
				{
					//TODO: make method
					if (_gm.CardsInDropZone.PlayerLeft.Count < 2) {
						//TODO: make method
						RectPosition = new Vector2((_gm.CardsInDropZone.PlayerLeft.Count * 152) + 34, 782);
						RectScale = new Vector2((float)0.7,(float)0.7);
						_gm.Drop(Card,"Left",ZoneState.ToString());
						ZoneState = Zone.Left;
					} else if (_gm.CardsInDropZone.PlayerLeft.Count < 3) {
						//TODO: make method
						RectPosition = new Vector2(112, 847);
						RectScale = new Vector2((float)0.7,(float)0.7);
						_gm.Drop(Card,"Left", ZoneState.ToString());
						ZoneState = Zone.Left;
					} else if (_gm.CardsInDropZone.PlayerLeft.Count < 5) {
						//TODO: make method
						RectPosition = new Vector2(((_gm.CardsInDropZone.PlayerLeft.Count-3) * 152) + 34, 917);
						RectScale = new Vector2((float)0.7,(float)0.7);
						_gm.Drop(Card,"Left", ZoneState.ToString());
						ZoneState = Zone.Left;
					} else {
						//TODO: make method
						RectPosition = _startPosition;
						RectScale = new Vector2(1,1);
						_gm.Drop(Card, "Hand", ZoneState.ToString());
						ZoneState = Zone.Hand;
					}
				}
				else if (_isOverMidDropZone)
				{
					//TODO: make method
					if (_gm.CardsInDropZone.PlayerMid.Count < 2) {
						//TODO: make method
						RectPosition = new Vector2((_gm.CardsInDropZone.PlayerMid.Count * 110) + 355, 784);
						RectScale = new Vector2((float)0.7,(float)0.7);
						_gm.Drop(Card,"Mid", ZoneState.ToString());
						ZoneState = Zone.Mid;
					} else if (_gm.CardsInDropZone.PlayerMid.Count < 4) {
						//TODO: make method
						RectPosition = new Vector2(((_gm.CardsInDropZone.PlayerMid.Count-2) * 110) + 355, 909);
						RectScale = new Vector2((float)0.7,(float)0.7);
						_gm.Drop(Card,"Mid", ZoneState.ToString());
						ZoneState = Zone.Mid;
					} else {
						//TODO: make method
						RectPosition = _startPosition;
						RectScale = new Vector2(1,1);
						_gm.Drop(Card, "Hand", ZoneState.ToString());
						ZoneState = Zone.Hand;
					}
					
				}
				else if (_isOverRightDropZone)
				{
					if (_gm.CardsInDropZone.PlayerRight.Count < 2) {
						RectPosition = new Vector2((_gm.CardsInDropZone.PlayerRight.Count * 110) + 660, 792);
						RectScale = new Vector2((float)0.7,(float)0.7);
						_gm.Drop(Card,"Right", ZoneState.ToString());
						ZoneState = Zone.Right;
					} else if (_gm.CardsInDropZone.PlayerRight.Count < 4) {
						RectPosition = new Vector2(((_gm.CardsInDropZone.PlayerRight.Count-2) * 110) + 660, 917);
						RectScale = new Vector2((float)0.7,(float)0.7);
						_gm.Drop(Card,"Right", ZoneState.ToString());
						ZoneState = Zone.Right;
					} else {
						//TODO: make method
						RectPosition = _startPosition;
						RectScale = new Vector2(1,1);
						_gm.Drop(Card, "Hand", ZoneState.ToString());
						ZoneState = Zone.Hand;
					}
				}
				else
				{
					RectPosition = _startPosition;
					RectScale = new Vector2(1,1);
					_gm.Drop(Card, "Hand", ZoneState.ToString());
					ZoneState = Zone.Hand;
				}
			}
		}
		base._Process(delta);
	}

	//handle mouse enter signal
	private void OnMouseEntered()
	{
		if (_isDragging) return;
		_mouseIn = true;
	}
	//handle mouse exit signal
	private void OnMouseExited()
	{
		_mouseIn = false;
	}

	//handle enter collision with dropzone signal
	private void OnArea2DEntered(Area2D area)
	{
		if (area.Name == "Area2DLeftLand1"){
			_isOverLeftLand1 = true;
		}
		else if (area.Name == "Area2DLeftLand2"){
			_isOverLeftLand2 = true;
		}
		else if (area.Name == "Area2DLeftLand3"){
			_isOverLeftLand3 = true;
		}
		else if (area.Name == "Area2DLeftLand4"){
			_isOverLeftLand4 = true;
		}
		else if (area.Name == "Area2DLeftLand5"){
			_isOverLeftLand5 = true;
		}
	}

	//handle exit collision with dropzone signal
	private void OnArea2DExited(Area2D area)
	{
		if (area.Name == "Area2DLeftLand1"){
			_isOverLeftLand1 = false;
		}
		else if (area.Name == "Area2DLeftLand2"){
			_isOverLeftLand2 = false;
		}
		else if (area.Name == "Area2DLeftLand3"){
			_isOverLeftLand3 = false;
		}
		else if (area.Name == "Area2DLeftLand4"){
			_isOverLeftLand4 = false;
		}
		else if (area.Name == "Area2DLeftLand5"){
			_isOverLeftLand5 = false;
		}
	}
}
