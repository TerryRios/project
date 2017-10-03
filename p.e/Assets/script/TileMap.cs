﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {

	public Map[] maps;
	public int mapIndex;

	public Transform tilePrefab;
	public Transform obstaclePrefab;
	public Transform navmeshFloor;
	public Transform navmeshMaskPrefab;
	public Vector2 MaxmapSize;
	[Range(0,1)]
	public float outlinePercent;
	public float tileSize;

	List<Coord> allTileCoords;
	Queue<Coord> shuffleTileCoords;

	Map currentMap;

	void Start(){

		GenerateMap ();
	}

	public void GenerateMap(){
		currentMap = maps [mapIndex];
		GetComponent<BoxCollider> ().size = new Vector3 (currentMap.mapSize.x * tileSize, .05f, currentMap.mapSize.y * tileSize);

		allTileCoords = new List<Coord> ();
		for (int x = 0; x < currentMap.mapSize.x; x++) {
			for (int y = 0; y < currentMap.mapSize.y; y++) {
				allTileCoords.Add (new Coord (x, y));
			}
		}
		shuffleTileCoords = new Queue<Coord> (Utility.shuffleArray (allTileCoords.ToArray (),currentMap.seed));

		string holderName = "Generated Map";
		if (transform.Find(holderName)) {
			DestroyImmediate (transform.Find (holderName).gameObject);
		}
		Transform mapHolder = new GameObject (holderName).transform;
		mapHolder.parent = transform;

		for (int x = 0; x < currentMap.mapSize.x; x++) {
			for (int y = 0; y < currentMap.mapSize.y; y++) {
				Vector3 tilePosition = CoordToPosition(x,y);
				Transform newTile = Instantiate (tilePrefab, tilePosition, Quaternion.Euler (Vector3.right * 90))as Transform;
				newTile.localScale = Vector3.one * (1 - outlinePercent)*tileSize;
				newTile.parent = mapHolder;
			}
		}

		bool[,] obstacleMap = new bool[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];

		int obstacleCount = (int)(currentMap.mapSize.x *currentMap.mapSize.y*currentMap.obstaclePercent);
		int currentObstacleCount = 0;
		for (int i = 0; i < obstacleCount; i++) {
			Coord randomCoord = GetRandomCoord ();
			obstacleMap [randomCoord.x, randomCoord.y] = true;
			currentObstacleCount++;
			if (randomCoord != currentMap.mapCentre && MapIsFullAccessible (obstacleMap, currentObstacleCount)) {

				Vector3 obstaclePosition = CoordToPosition (randomCoord.x, randomCoord.y);

				Transform newObstacle = Instantiate (obstaclePrefab, obstaclePosition + Vector3.up * .5f, Quaternion.identity) as Transform;
				newObstacle.parent = mapHolder;
				newObstacle.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
			} else {
				obstacleMap [randomCoord.x, randomCoord.y] = false;
				currentObstacleCount--;
			}
		}
		Transform maskleft = Instantiate (navmeshMaskPrefab, Vector3.left * (currentMap.mapSize.x + MaxmapSize.x) / 4 * tileSize, Quaternion.identity)as Transform;
		maskleft.parent = mapHolder;
		maskleft.localScale = new Vector3 ((MaxmapSize.x - currentMap.mapSize.x) / 2, 1, currentMap.mapSize.y) * tileSize;

		Transform maskRight = Instantiate (navmeshMaskPrefab, Vector3.right * (currentMap.mapSize.x + MaxmapSize.x) / 4 * tileSize, Quaternion.identity)as Transform;
		maskRight.parent = mapHolder;
		maskRight.localScale = new Vector3 ((MaxmapSize.x - currentMap.mapSize.x) / 2, 1, currentMap.mapSize.y) * tileSize;

		Transform maskTop = Instantiate (navmeshMaskPrefab, Vector3.forward * (currentMap.mapSize.y + MaxmapSize.y) / 4 * tileSize, Quaternion.identity)as Transform;
		maskTop.parent = mapHolder;
		maskTop.localScale = new Vector3 (MaxmapSize.x, 1,(MaxmapSize.y- currentMap.mapSize.y)/2) * tileSize;

		Transform maskBottom = Instantiate (navmeshMaskPrefab, Vector3.back * (currentMap.mapSize.y + MaxmapSize.y) / 4 * tileSize, Quaternion.identity)as Transform;
		maskBottom.parent = mapHolder;
		maskBottom.localScale = new Vector3 (MaxmapSize.x, 1,(MaxmapSize.y- currentMap.mapSize.y)/2) * tileSize;

		navmeshFloor.localScale = new Vector3 (MaxmapSize.x, MaxmapSize.y) * tileSize;
	}

	bool MapIsFullAccessible(bool[,] obstacleMap,int currentObstacleCount){

		bool[,] mapFlags = new bool[obstacleMap.GetLength (0), obstacleMap.GetLength (1)];
		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (currentMap.mapCentre);
		mapFlags [currentMap.mapCentre.x, currentMap.mapCentre.y] = true;
		int accessibleTileCount = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue ();

			for (int x = -1; x <= 1; x++) {
				for (int y = -1; y <= 1; y++) {
					int neighbourX = tile.x + x;
					int neighbourY = tile.y + y;
					if (x==0||y==0) {
						if (neighbourX >=0&&neighbourX < obstacleMap.GetLength(0)&&neighbourY >=0&&neighbourY < obstacleMap.GetLength(1)) {
							if (!mapFlags[neighbourX,neighbourY]&& !obstacleMap[neighbourX,neighbourY]) {
								mapFlags [neighbourX, neighbourY] = true;
								queue.Enqueue (new Coord (neighbourX, neighbourY));
								accessibleTileCount++;
							}
						}
					}					
				}				
			}
		}

		int targetAccessibleTileCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y - currentObstacleCount);
		return targetAccessibleTileCount == accessibleTileCount;
	}

	Vector3 CoordToPosition(int x,int y){
		return new Vector3 (-currentMap.mapSize.x / 2 + 0.5f + x, 0, -currentMap.mapSize.y / 2 + 0.5f + y)*tileSize;
	}

	public Coord GetRandomCoord(){
		Coord randomCoord = shuffleTileCoords.Dequeue();
		shuffleTileCoords.Enqueue(randomCoord);
		return randomCoord;
	}

	[System.Serializable]
	public struct Coord{
		public int x;
		public int y;

		public Coord(int _x,int _y){
			x= _x;
			y= _y;
		}

		public static bool operator ==(Coord c1,Coord c2){
			return c1.x == c2.x && c1.y == c2.y;
		}
		public static bool operator !=(Coord c1,Coord c2){
			return !(c1 == c2);

		}
	}

	[System.Serializable]
	public class Map{

		public Coord mapSize;
		[Range(0,1)]
		public float obstaclePercent;
		public int seed;
		public float minObstacleHeight;
		public float maxObstacleHeight;

		public Coord mapCentre{
			get{ 
				return new Coord (mapSize.x / 2, mapSize.y / 2);			
			}			
		}
	}
		
}
