using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {
  public int width = 10;

  public int height = 24;

  public int[,] matrix;

  private void EachElement(Action<int, int> action) {
    for (int col = 0; col < width; col++) {
      for (int row = 0; row < height; row++) {
        action(col, row);
      }
    }
  }

  public void MergeMatrix(Transform parent, Color color, GameObject blockPrefab) {
    foreach (Transform child in parent) {
      Vector2Int pos = RoundPos(child.position);
      SetMatrixValue(pos, 1);
      GameObject block = Instantiate(blockPrefab, transform);
      block.transform.Translate(pos.x, pos.y, 0);
      block.GetComponent<Renderer>().material.color = color;
    }
  }

  private void SetMatrixValue(Vector2 pos, int value) {
    if (IsValidPosition(pos)) {
      matrix[(int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y)] = value;
    }
  }

  private Vector2Int RoundPos(Vector2 pos) {
    return new Vector2Int((int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y));
  }

  private int GetMatrixValue(Vector2Int pos) {
    if (IsValidPosition(pos)) {
      return matrix[pos.x, pos.y];
    }
    return 0;
  }

  public int GetWidth(int offset = 0) {
    return width + offset; ;
  }

  public int GetHeight(int offset = 0) {
    return height + offset;
  }

  public bool IsValidX(float x) {
    return Mathf.Round(x) >= 0 && Mathf.Round(x) < width;
  }

  public bool IsValidY(float y) {
    return Mathf.Round(y) >= 0 && Mathf.Round(y) < height;
  }

  public bool IsValidPosition(Vector2 pos) {
    return IsValidX(pos.x) && IsValidY(pos.y);
  }

  public bool CollidesHorizontal(Vector2 pos) {
    return IsValidPosition(pos) && GetMatrixValue(RoundPos(pos)) == 1;
  }

  public bool CollidesVertical(Transform matrix) {
    bool result = false;
    foreach (Transform child in matrix) {
      Vector2Int pos = RoundPos(child.position);
      if (GetMatrixValue(pos) > 0 || pos.y < 0) {
        result = true;
        break;
      }
    }
    return result;
  }

  public bool OutOfBounds(Vector2Int pos) {
    return pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height;
  }

  public bool Collides(Transform matrix) {
    bool result = false;
    foreach (Transform child in matrix) {
      Vector2Int pos = RoundPos(child.position);
      if (GetMatrixValue(pos) > 0 || OutOfBounds(pos)) {
        result = true;
        break;
      }
    }
    return result;
  }

  void Start() {
    matrix = new int[width, height];
    EachElement((x, y) => {
      matrix[x, y] = 0;
    });
  }
}
