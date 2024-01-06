using Cards;
using Godot;
using System;

public class Drag : CardPanel
{
	//create private variables to store initial data
	private bool _mouseIn = false;
	private bool _isDragging = false;
	private bool _isPlayed = false;
	private enum Played {Hand, Played, Discarded, Destroyed}
	private enum Turn {Start,Mid,End}
	private bool _isOverLeftLandTopLeft = false;
	private bool _isOverLeftLandTopRight = false;
	private bool _isOverLeftLandMid = false;
	private bool _isOverLeftLandBottomLeft = false;
	private bool _isOverLeftLandBottomRight = false;

	private Vector2 _startPosition;
	private GameManager _gm;
	private GameManager.CardPlayedZone ZoneState;
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
				RectScale = new Vector2((float)0.8,(float)0.8);
			}
			//handle dropping or return card to start position if not over dropzone
			if (Input.IsActionJustReleased("left_click"))
			{
				_isDragging = false;
				if (_isOverLeftLandTopLeft)
				{
					//TODO: make method
					//Top left
					RectPosition = new Vector2(34, 782);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Left,
							SubZone = GameManager.CardSubZone.TopLeft
						}
					);
				} 
				else if (_isOverLeftLandTopRight) 
				{
					//Top right
					RectPosition = new Vector2(188, 782);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Left,
							SubZone = GameManager.CardSubZone.TopRight
						}
					);
				} 
				else if (_isOverLeftLandMid) 
				{
					//TODO: make method
					//Mid
					RectPosition = new Vector2(112, 847);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Left,
							SubZone = GameManager.CardSubZone.Mid
						}
					);
				} 
				else if (_isOverLeftLandBottomLeft) 
				{
					//TODO: make method
					//Bottom left
					RectPosition = new Vector2(34, 917);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Left,
							SubZone = GameManager.CardSubZone.BottomLeft
						}
					);
				} 
				else if (_isOverLeftLandBottomRight) 
				{
					//TODO: make method
					//Bottom right
					RectPosition = new Vector2(188, 917);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Left,
							SubZone = GameManager.CardSubZone.BottomRight
						}
					);
				} else 
				{
					//TODO: make method
					RectPosition = _startPosition;
					RectScale = new Vector2(1,1);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Hand,
						}
					);
				}

				//Reset
				_isOverLeftLandTopLeft = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandTopLeft")).Position = new Vector2(81,128);
				_isOverLeftLandBottomRight = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandBottomRight")).Position = new Vector2(230,230);
				_isOverLeftLandTopRight = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandTopRight")).Position = new Vector2(230,128);
				_isOverLeftLandMid = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandMid")).Position = new Vector2(157,174);
				_isOverLeftLandBottomLeft = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandBottomLeft")).Position = new Vector2(81,230);

				
				/* else if (_isOverMidDropZone)
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
				} */
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
		//TODO: Make OnArea2DEntered a dynamic method
		if (area.Name == "Area2DLeftLandTopLeft"){
			GD.Print("Entered TopLeft");
			_isOverLeftLandTopLeft = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandTopLeft")).Position = new Vector2(81,118);
		}
		else if (area.Name == "Area2DLeftLandTopRight"){
			GD.Print("Entered TopRight");
			_isOverLeftLandTopRight = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandTopRight")).Position = new Vector2(230,118);
		}
		else if (area.Name == "Area2DLeftLandMid"){
			GD.Print("Entered Mid");
			_isOverLeftLandMid = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandMid")).Position = new Vector2(157,164);
		}
		else if (area.Name == "Area2DLeftLandBottomLeft"){
			GD.Print("Entered BottomLeft");
			_isOverLeftLandBottomLeft = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandBottomLeft")).Position = new Vector2(81,220);
		}
		else if (area.Name == "Area2DLeftLandBottomRight"){
			GD.Print("Entered BottomRight");
			_isOverLeftLandBottomRight = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandBottomRight")).Position = new Vector2(230,220);
		}
	}

	//handle exit collision with dropzone signal
	private void OnArea2DExited(Area2D area)
	{
		//TODO: Make OnArea2DExited a dynamic method
		if (area.Name == "Area2DLeftLandTopLeft"){
			GD.Print("Exited TopLeft");
			_isOverLeftLandTopLeft = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandTopLeft")).Position = new Vector2(81,128);
		}
		else if (area.Name == "Area2DLeftLandTopRight"){
			GD.Print("Exited TopRight");
			_isOverLeftLandTopRight = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandTopRight")).Position = new Vector2(230,128);
		}
		else if (area.Name == "Area2DLeftLandMid"){
			GD.Print("Exited Mid");
			_isOverLeftLandMid = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandMid")).Position = new Vector2(157,174);
		}
		else if (area.Name == "Area2DLeftLandBottomLeft"){
			GD.Print("Exited BottomLeft");
			_isOverLeftLandBottomLeft = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandBottomLeft")).Position = new Vector2(81,230);
		}
		else if (area.Name == "Area2DLeftLandBottomRight"){
			GD.Print("Exited BottomRight");
			_isOverLeftLandBottomRight = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneLeft/Lands/LandBottomRight")).Position = new Vector2(230,230);
		}
	}
}
