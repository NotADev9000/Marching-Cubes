﻿static const uint numThreads = 8;

uint _ChunkSize;

int indexFromCoord(int x, int y, int z)
{
    return x + _ChunkSize * (y + _ChunkSize * z);
}
