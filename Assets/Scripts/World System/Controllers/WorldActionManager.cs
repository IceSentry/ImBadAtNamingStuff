public class WorldActionManager {
    public static bool MineAt(Tile tile) {
        WorldManager worldManager = WorldManager.Instance;
        if (tile.Type == TileTypes.Rock) {
            tile.Type = TileTypes.Dirt;
            return true;
        }
        return false;
    }
}
