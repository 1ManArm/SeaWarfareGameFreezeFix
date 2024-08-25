using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public CoordStatus [,] PlayerShips = new CoordStatus[10, 10];
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
            y = int.Parse(ShotCoord.Substring(1, 1));
            if (PlayerShips[x, y] == CoordStatus.None)
                { result = ShotStatus.Miss; }
            else 
            { 
                result = ShotStatus.Kill;
                if ((x != 9 && PlayerShips[x + 1, y] == CoordStatus.Ship) ||
                    (y != 9 && PlayerShips[x, y + 1] == CoordStatus.Ship) ||
                    (x != 0 && PlayerShips[x - 1, y] == CoordStatus.Ship) ||
                    (y != 0 && PlayerShips[x, y - 1] == CoordStatus.Ship)||
                    (x < 8 && PlayerShips[x + 2, y] == CoordStatus.Ship) ||
                    (y < 8 && PlayerShips[x, y + 2] == CoordStatus.Ship) ||
                    (x > 1 && PlayerShips[x - 2, y] == CoordStatus.Ship) ||
                    (y > 1 && PlayerShips[x, y - 2] == CoordStatus.Ship) ||
                    (x < 7 && PlayerShips[x + 3, y] == CoordStatus.Ship) ||
                    (y < 7 && PlayerShips[x, y + 3] == CoordStatus.Ship) ||
                    (x > 2 && PlayerShips[x - 3, y] == CoordStatus.Ship) ||
                    (y > 2 && PlayerShips[x, y - 3] == CoordStatus.Ship) )
                        result = ShotStatus.Wounded;
                PlayerShips[x, y] = CoordStatus.Hit;
                UndiscoveredCells--;
                if (UndiscoveredCells == 0)
                {
                    result = ShotStatus.EndBattle;
                }

            }
            return result;
        }

        //Генерация выстрела
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
        public bool CheckCoord(string xy, ShipType Type, Direction dir = Direction.Vertical)
        {
            bool result = true;
            return result;
        }
        //Добавляет или удаляет корабль
        // xy - координаты корабля, Type - тип корабля, dir - направление размещения корабля, deleting - удалить или добавить
        //В случае успешной операции возвращает true
        public bool AddDelShip(string xy, ShipType Type, Direction dir = Direction.Vertical, bool deleting = false)
        {
            bool result = true;
            if (deleting || CheckCoord(xy, Type, dir))
            {
                int x, y;
                x = int.Parse(xy.Substring(0, 1));
                y = int.Parse(xy.Substring(1, 1));
                CoordStatus status = new CoordStatus();
                if (deleting) status = CoordStatus.None; else status = CoordStatus.Ship;

                PlayerShips[x, y] = status;
                if (dir == Direction.Vertical)
                {
                    switch (Type)
                    {
                        case ShipType.x2:
                            PlayerShips[x, y + 1] = status;
                            break;
                        case ShipType.x3:
                            PlayerShips[x, y + 1] = status;
                            PlayerShips[x, y + 2] = status;
                            break;
                        case ShipType.x4:
                            PlayerShips[x, y + 1] = status;
                            PlayerShips[x, y + 2] = status;
                            PlayerShips[x, y + 3] = status;
                            break;
                    }

                }
                else
                {
                    switch (Type)
                    {
                        case ShipType.x2:
                            PlayerShips[x + 1, y] = status;
                            break;
                        case ShipType.x3:
                            PlayerShips[x + 1, y] = status;
                            PlayerShips[x + 2, y] = status;
                            break;
                        case ShipType.x4:
                            PlayerShips[x + 1, y] = status;
                            PlayerShips[x + 2, y] = status;
                            PlayerShips[x + 3, y] = status;
                            break;
                    }
                }

            }
            else result = false;
            return result;
        }
        public void DelShips()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    PlayerShips[i, j] = CoordStatus.None;
                }
        }


    }
}
