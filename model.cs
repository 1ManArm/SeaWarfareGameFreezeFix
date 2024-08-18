using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaWarfareGame
{
    public enum ShotStatus 
    { 
        Miss, Wounded, Kill, EndBattle
    }
    public enum CoordStatus
    {
        None, Ship, Shot, Hit
    }
    public enum ShipType
    {
        x4, x3, x2, x1
    }
    public enum Direction
    {
        Horizontal, Vertical
    }
    public class model
    {
        //Массив кораблей игрока
        public CoordStatus [,] PlayerShips = new CoordStatus[10,10];
        //Массив кораблей противника
        public CoordStatus [,] EnemyShips = new CoordStatus[10, 10];
        
        //Кол-во необнаруженных клеток противника
        public int UndiscoveredCells = 20;
        
        //Поле статуса последнего выстрела
        public ShotStatus LastShot;
        //Поле статуса ранения
        public bool WoundedStatus;
        //Поле статуса первого попадания
        public bool FirstHit;
        //Поле координат последнего выстрела
        public string LastShotCoord;

        //Конструктор. Инициализация полей модели
        public model()
        {
            LastShot = ShotStatus.Miss;
            WoundedStatus = false;
            for (int i = 0; i < 10;  i++)
                for (int j = 0; j < 10; j++)
                {
                    PlayerShips[i, j] = CoordStatus.None;
                    EnemyShips[i, j] = CoordStatus.None;
                }
        }
        //Выстрел игрока. Входящий параметр - координаты выстрела в виде строки из 2х цифр
        public ShotStatus Shot(string ShotCoord)
        {
            ShotStatus result = ShotStatus.Miss;
            int x, y;
            x = int.Parse(ShotCoord.Substring(0, 1));
            y = int.Parse(ShotCoord.Substring(1));

            if (x < 0 || x >= PlayerShips.GetLength(0) || y < 0 || y >= PlayerShips.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("Координаты находятся вне диапазона.");
            }

            if (PlayerShips[x, y] == CoordStatus.None)
                { result = ShotStatus.Miss; }
            else 
            { 
                result = ShotStatus.Kill;
                bool isWounded = (x != 9 && PlayerShips[x + 1, y] == CoordStatus.Ship) ||
                          (y != 9 && PlayerShips[x, y + 1] == CoordStatus.Ship) ||
                          (x != 0 && PlayerShips[x - 1, y] == CoordStatus.Ship) ||
                          (y != 0 && PlayerShips[x, y - 1] == CoordStatus.Ship) ||
                          (x < 8 && PlayerShips[x + 2, y] == CoordStatus.Ship) ||
                          (y < 8 && PlayerShips[x, y + 2] == CoordStatus.Ship) ||
                          (x > 1 && PlayerShips[x - 2, y] == CoordStatus.Ship) ||
                          (y > 1 && PlayerShips[x, y - 2] == CoordStatus.Ship) ||
                          (x < 7 && PlayerShips[x + 3, y] == CoordStatus.Ship) ||
                          (y < 7 && PlayerShips[x, y + 3] == CoordStatus.Ship) ||
                          (x > 2 && PlayerShips[x - 3, y] == CoordStatus.Ship) ||
                          (y > 2 && PlayerShips[x, y - 3] == CoordStatus.Ship);

                if (isWounded)
                {
                    result = ShotStatus.Wounded;
                }

                PlayerShips[x, y] = CoordStatus.Hit;
                UndiscoveredCells--;

                if (UndiscoveredCells == 0)
                {
                    result = ShotStatus.EndBattle;
                }

            }
            return result;
        }

        //Генерация выстрела - Ошибка с зависанием была здесь
        public string ShotGen()
        {
            Random rand = new Random();
            int x, y;

            for (int attempt = 0; attempt < 100; attempt++)
            {
                x = rand.Next(0, 10);
                y = rand.Next(0, 10);

                if (EnemyShips[x, y] == CoordStatus.None)
                {
                    LastShotCoord = $"{x}{y}";
                    return LastShotCoord;
                }
            }

            throw new InvalidOperationException("Нет доступных координат для выстрела.");
        }
    }
}
