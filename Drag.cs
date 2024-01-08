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

	//WTF!!!!
	private bool _isOverLeftLandTopLeft = false;
	private bool _isOverLeftLandTopRight = false;
	private bool _isOverLeftLandMid = false;
	private bool _isOverLeftLandBottomLeft = false;
	private bool _isOverLeftLandBottomRight = false;
	private bool _isOverMidLandTopLeft = false;
	private bool _isOverMidLandTopRight = false;
	private bool _isOverMidLandMid = false;
	private bool _isOverMidLandBottomLeft = false;
	private bool _isOverMidLandBottomRight = false;
	private bool _isOverRightLandTopLeft = false;
	private bool _isOverRightLandTopRight = false;
	private bool _isOverRightLandMid = false;
	private bool _isOverRightLandBottomLeft = false;
	private bool _isOverRightLandBottomRight = false;
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
				}
				else if (_isOverMidLandTopLeft)
				{
					//TODO: make method
					//Top left
					RectPosition = new Vector2(338, 772);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Mid,
							SubZone = GameManager.CardSubZone.TopLeft
						}
					);
				} 
				else if (_isOverMidLandTopRight) 
				{
					//Top right
					RectPosition = new Vector2(494, 772);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Mid,
							SubZone = GameManager.CardSubZone.TopRight
						}
					);
				} 
				else if (_isOverMidLandMid) 
				{
					//TODO: make method
					//Mid
					RectPosition = new Vector2(416, 837);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Mid,
							SubZone = GameManager.CardSubZone.Mid
						}
					);
				} 
				else if (_isOverMidLandBottomLeft) 
				{
					//TODO: make method
					//Bottom left
					RectPosition = new Vector2(338, 907);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Mid,
							SubZone = GameManager.CardSubZone.BottomLeft
						}
					);
				} 
				else if (_isOverMidLandBottomRight) 
				{
					//TODO: make method
					//Bottom right
					RectPosition = new Vector2(494, 907);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Mid,
							SubZone = GameManager.CardSubZone.BottomRight
						}
					);
				} 
				else if (_isOverRightLandTopLeft)
				{
					//TODO: make method
					//Top left
					RectPosition = new Vector2(642, 782);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Right,
							SubZone = GameManager.CardSubZone.TopLeft
						}
					);
				} 
				else if (_isOverRightLandTopRight) 
				{
					//Top right
					RectPosition = new Vector2(798, 782);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Right,
							SubZone = GameManager.CardSubZone.TopRight
						}
					);
				} 
				else if (_isOverRightLandMid) 
				{
					//TODO: make method
					//Mid
					RectPosition = new Vector2(720, 847);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Right,
							SubZone = GameManager.CardSubZone.Mid
						}
					);
				} 
				else if (_isOverRightLandBottomLeft) 
				{
					//TODO: make method
					//Bottom left
					RectPosition = new Vector2(642, 917);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Right,
							SubZone = GameManager.CardSubZone.BottomLeft
						}
					);
				} 
				else if (_isOverRightLandBottomRight) 
				{
					//TODO: make method
					//Bottom right
					RectPosition = new Vector2(798, 917);
					RectScale = new Vector2((float)0.7,(float)0.7);
					_gm.Drop(
						Card, 
						new GameManager.CardPlayedZone(){
							Zone = GameManager.CardZone.Right,
							SubZone = GameManager.CardSubZone.BottomRight
						}
					);
				} 
				else 
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

				_isOverMidLandTopLeft = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandTopLeft")).Position = new Vector2(81,112);
				_isOverMidLandTopRight = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandTopRight")).Position = new Vector2(230,112);
				_isOverMidLandMid = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandMid")).Position = new Vector2(157,166);
				_isOverMidLandBottomLeft = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandBottomLeft")).Position = new Vector2(81,222);
				_isOverMidLandBottomRight = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandBottomRight")).Position = new Vector2(230,222);

				_isOverRightLandTopLeft = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandTopLeft")).Position = new Vector2(81,128);
				_isOverRightLandTopRight = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandTopRight")).Position = new Vector2(230,128);
				_isOverRightLandMid = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandMid")).Position = new Vector2(157,174);
				_isOverRightLandBottomLeft = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandBottomLeft")).Position = new Vector2(81,230);
				_isOverRightLandBottomRight = false;
				((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandBottomRight")).Position = new Vector2(230,230);
				
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
		else if (area.Name == "Area2DMidLandTopLeft"){
			GD.Print("Entered Mid TopLeft");
			_isOverMidLandTopLeft = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandTopLeft")).Position = new Vector2(81,102);
		}
		else if (area.Name == "Area2DMidLandTopRight"){
			GD.Print("Entered Mid TopRight");
			_isOverMidLandTopRight = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandTopRight")).Position = new Vector2(230,102);
		}
		else if (area.Name == "Area2DMidLandMid"){
			GD.Print("Entered Mid Mid");
			_isOverMidLandMid = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandMid")).Position = new Vector2(157,156);
		}
		else if (area.Name == "Area2DMidLandBottomLeft"){
			GD.Print("Entered Mid BottomLeft");
			_isOverMidLandBottomLeft = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandBottomLeft")).Position = new Vector2(81,212);
		}
		else if (area.Name == "Area2DMidLandBottomRight"){
			GD.Print("Entered Mid BottomRight");
			_isOverMidLandBottomRight = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandBottomRight")).Position = new Vector2(230,212);
		}
		else if (area.Name == "Area2DRightLandTopLeft"){
			GD.Print("Entered Right TopLeft");
			_isOverRightLandTopLeft = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandTopLeft")).Position = new Vector2(81,118);
		}
		else if (area.Name == "Area2DRightLandTopRight"){
			GD.Print("Entered Right TopRight");
			_isOverRightLandTopRight = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandTopRight")).Position = new Vector2(230,118);
		}
		else if (area.Name == "Area2DRightLandMid"){
			GD.Print("Entered Right Mid");
			_isOverRightLandMid = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandMid")).Position = new Vector2(157,164);
		}
		else if (area.Name == "Area2DRightLandBottomLeft"){
			GD.Print("Entered Right BottomLeft");
			_isOverRightLandBottomLeft = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandBottomLeft")).Position = new Vector2(81,220);
		}
		else if (area.Name == "Area2DRightLandBottomRight"){
			GD.Print("Entered Right BottomRight");
			_isOverRightLandBottomRight = true;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandBottomRight")).Position = new Vector2(230,220);
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
		else if (area.Name == "Area2DMidLandTopLeft"){
			GD.Print("Exited Mid TopLeft");
			_isOverMidLandTopLeft = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandTopLeft")).Position = new Vector2(81,112);
		}
		else if (area.Name == "Area2DMidLandTopRight"){
			GD.Print("Exited Mid TopRight");
			_isOverMidLandTopRight = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandTopRight")).Position = new Vector2(230,112);
		}
		else if (area.Name == "Area2DMidLandMid"){
			GD.Print("Exited Mid Mid");
			_isOverMidLandMid = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandMid")).Position = new Vector2(157,166);
		}
		else if (area.Name == "Area2DMidLandBottomLeft"){
			GD.Print("Exited Mid BottomLeft");
			_isOverMidLandBottomLeft = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandBottomLeft")).Position = new Vector2(81,222);
		}
		else if (area.Name == "Area2DMidLandBottomRight"){
			GD.Print("Exited Mid BottomRight");
			_isOverMidLandBottomRight = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneMid/Lands/LandBottomRight")).Position = new Vector2(230,222);
		}
		else if (area.Name == "Area2DRightLandTopLeft"){
			GD.Print("Exited Right TopLeft");
			_isOverRightLandTopLeft = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandTopLeft")).Position = new Vector2(81,128);
		}
		else if (area.Name == "Area2DRightLandTopRight"){
			GD.Print("Exited Right TopRight");
			_isOverRightLandTopRight = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandTopRight")).Position = new Vector2(230,128);
		}
		else if (area.Name == "Area2DRightLandMid"){
			GD.Print("Exited Right Mid");
			_isOverRightLandMid = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandMid")).Position = new Vector2(157,174);
		}
		else if (area.Name == "Area2DRightLandBottomLeft"){
			GD.Print("Exited Right BottomLeft");
			_isOverRightLandBottomLeft = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandBottomLeft")).Position = new Vector2(81,230);
		}
		else if (area.Name == "Area2DRightLandBottomRight"){
			GD.Print("Exited Right BottomRight");
			_isOverRightLandBottomRight = false;
			((Sprite)_gm.GetParent().GetNode<Sprite>("DropZone/DropZoneRight/Lands/LandBottomRight")).Position = new Vector2(230,230);
		}
	}
}
