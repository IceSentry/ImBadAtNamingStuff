using System.Linq;

public class WorldActionManager {
    public static bool MineAt(Tile tile) {
        Tile[] neihbours = tile.GetNeighbours();
        if (tile.Type == TileTypes.Rock && (neihbours.Any(x => x.Type == TileTypes.Grass) || neihbours.Any(x => x.Type == TileTypes.Dirt))) {
            tile.Type = TileTypes.Dirt;
            return true;
        }
        return false;
    }
}