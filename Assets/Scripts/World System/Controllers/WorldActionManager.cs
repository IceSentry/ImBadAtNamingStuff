public class WorldActionManager {

    public static bool MineAt(Tile tile) {
        if (tile.Type == TileTypes.Rock) {
            tile.Type = TileTypes.Dirt;
            return true;
        }
        return false;
    }
}