using Godot;
using System;

public partial class Path : TileMapLayer
{
	private TileMapLayer _foliageLayer;

	public override void _Ready()
	{
		// Get reference to the Foliage TileMapLayer
		_foliageLayer = GetNode<TileMapLayer>("/root/Game/Map/TileMap/foliage");
		if (_foliageLayer == null)
		{
			GD.PrintErr("Foliage TileMapLayer not found!");
		}
		GD.Print("Foliage TileMap found");
	}

	public bool UseTileDataRuntimeUpdate(Vector2I coords)
	{
		// Check if the current cell has foliage
		if (_foliageLayer != null && _foliageLayer.GetUsedCellsById(0).Contains(coords))
		{
			return true;
		}
		return false;
	}

	public void TileDataRuntimeUpdate(Vector2I coords, TileData tileData)
	{
		// Modify the navigation polygon if the cell has foliage
		if (_foliageLayer != null && _foliageLayer.GetUsedCellsById(0).Contains(coords))
		{
			tileData.SetNavigationPolygon(0, null); // Replace '8' with the appropriate polygon ID if needed
		}
	}
}
